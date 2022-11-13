using DatabaseEntityManagement.Data.Context.QueryContext.Collection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseEntityManagement.Data.Context.QueryContext.Scope
{
    public class QueryContextScope : IQueryContextScope
    {
        private static readonly string AmbientQueryContextScopeKey = $"AmbientQueryContext_{Guid.NewGuid()}";

        private static readonly ConditionalWeakTable<InstanceIdentifier, QueryContextScope> QueryContextScopeInstances = new ConditionalWeakTable<InstanceIdentifier, QueryContextScope>();

        private bool _disposed;
        private bool _readOnly;
        private bool _completed;
        private bool _nested;

        private QueryContextScope _parentScope;
        private QueryContextCollection _queryContexts;
        private InstanceIdentifier _instanceIdentifier = new InstanceIdentifier();

        public IQueryContextCollection QueryContexts => _queryContexts;

        public QueryContextScope() : this(EQueryScopeJoinMethod.JOIN_EXISTING, false, null) { }

        public QueryContextScope(bool readOnly) : this(EQueryScopeJoinMethod.JOIN_EXISTING, readOnly, null) { }

        public QueryContextScope(EQueryScopeJoinMethod joinMethod, bool readOnly, IsolationLevel? isolationLevel)
        {
            if (isolationLevel.HasValue && joinMethod == EQueryScopeJoinMethod.JOIN_EXISTING)
                throw new ArgumentException("By specifying the isolation level, you are requesting a transaction. A transaction can only be handled in its own context. Please use tje CREATE_NEW join method instead.");

            _disposed = false;
            _completed = false;
            _readOnly = readOnly;

            _parentScope = GetAmbientScope();
            if (_parentScope != null && joinMethod == EQueryScopeJoinMethod.JOIN_EXISTING)
            {
                if (_parentScope._readOnly && !_readOnly) throw new InvalidOperationException("Cannot nest a read/write contect within a read-only context.");

                _nested = true;
                _queryContexts = _parentScope._queryContexts;
            }
            else
            {
                _nested = true;
                _queryContexts = new QueryContextCollection(readOnly, isolationLevel);
            }

            SetAmbientScope(this);

        }

        public int SaveChanges()
        {
            if (_disposed) throw new ObjectDisposedException(nameof(QueryContextScope));

            if (_completed) throw new InvalidOperationException("You cannot call SaveChanges more than once in a QueryContextScope.");

            var savedEntries = _nested ? 0 : CommitInternal();

            _completed = true;

            return savedEntries;
        }

        public static QueryContextScope GetAmbientScope()
        {
            InstanceIdentifier instanceIdentifier = CallContext.LogicalGetData(AmbientQueryContextScopeKey) as InstanceIdentifier;

            if (instanceIdentifier == null) return null;

            if (QueryContextScopeInstances.TryGetValue(instanceIdentifier, out var ambientScope)) return ambientScope;

            return null;
        }

        public void Dispose()
        {
            if (_disposed) return;

            if (!_nested)
            {
                if (!_completed)
                {
                    try
                    {
                        if (_readOnly) CommitInternal();
                        else RollbackInternal();
                    }
                    catch (Exception exception)
                    {
                        System.Diagnostics.Debug.WriteLine(exception);
                    }

                    _completed = true;
                }

                _queryContexts.Dispose();

            }

            var currentAmbientScope = GetAmbientScope();
            if (currentAmbientScope != this) throw new InvalidOperationException("QueryContextScope instances must be disposed of in the order in which they were created!");

            RemoveAmbientScope();

            if (_parentScope != null)
            {
                if (_parentScope._disposed)
                {
                    throw new InvalidOperationException("QueryContextScope instances must be disposed of in the order in which they were created!");
                }

                SetAmbientScope(_parentScope);
            }

            _disposed = true;
        }

        private int CommitInternal()
        {
            return _queryContexts.Commit();
        }

        private void RollbackInternal()
        {
            _queryContexts.Rollback();
        }


        private void SetAmbientScope(QueryContextScope newAmbientScope)
        {
            if (newAmbientScope == null) throw new ArgumentNullException(nameof(newAmbientScope));

            var current = CallContext.LogicalGetData(AmbientQueryContextScopeKey) as InstanceIdentifier;

            if (current == newAmbientScope._instanceIdentifier) return;

            CallContext.LogicalSetData(AmbientQueryContextScopeKey, newAmbientScope._instanceIdentifier);

            QueryContextScopeInstances.GetValue(newAmbientScope._instanceIdentifier, key => newAmbientScope);
        }

        private void RemoveAmbientScope()
        {
            var currentContext = CallContext.LogicalGetData(AmbientQueryContextScopeKey) as InstanceIdentifier;
            CallContext.LogicalSetData(AmbientQueryContextScopeKey, null);

            if (currentContext != null)
            {
                QueryContextScopeInstances.Remove(currentContext);
            }
        }

        internal class InstanceIdentifier : MarshalByRefObject { }
    }
}
