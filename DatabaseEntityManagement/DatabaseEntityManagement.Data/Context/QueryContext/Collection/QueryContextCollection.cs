using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseEntityManagement.Data.Context.QueryContext.Collection
{
    public class QueryContextCollection : IQueryContextCollection
    {
        private Dictionary<Type, DbContext> _initializedContexts;
        private Dictionary<DbContext, DbContextTransaction> _transactions;
        private IsolationLevel? _isolationLevel;
        private bool _disposed;
        private bool _completed;
        private bool _readOnly;

        internal Dictionary<Type, DbContext> InitializedContexts => _initializedContexts;

        public QueryContextCollection(bool readOnly = false, IsolationLevel? isolationLevel = null)
        {
            _readOnly = readOnly;
            _isolationLevel = isolationLevel;

            _disposed = false;
            _completed = false;

            _initializedContexts = new Dictionary<Type, DbContext>();
            _transactions = new Dictionary<DbContext, DbContextTransaction>();
        }

        public TQueryContext Get<TQueryContext>() where TQueryContext : DbContext
        {
            if (_disposed) throw new ObjectDisposedException(nameof(QueryContextCollection));

            var requestedType = typeof(TQueryContext);

            // If there is no context for the specified type already, create one
            if (!_initializedContexts.ContainsKey(requestedType))
            {
                // Create an instance of the specified query context type and add it to our initialized contexts
                var queryContext = Activator.CreateInstance<TQueryContext>();
                _initializedContexts.Add(requestedType, queryContext);

                // Configure whether the context is read-only or not
                if (_readOnly)
                {
                    queryContext.Configuration.AutoDetectChangesEnabled = false;
                }

                // If an isolation level was specified, start a new transaction and add it to the list of transactions
                if (_isolationLevel.HasValue)
                {
                    var transaction = queryContext.Database.BeginTransaction(_isolationLevel.Value);
                    _transactions.Add(queryContext, transaction);
                }
            }

            // Retrieve the context by the requwested type and return it
            return _initializedContexts[requestedType] as TQueryContext;
        }

        public int Commit()
        {
            if (_disposed) throw new ObjectDisposedException(nameof(QueryContextCollection));

            if (_completed) throw new InvalidOperationException("Commit can only be executed once per context. Start a new context if you wish to make more changes.");

            ExceptionDispatchInfo lastError = null;
            var savedEntries = 0;

            foreach (var context in _initializedContexts.Values)
            {
                try
                {
                    if (!_readOnly)
                    {
                        savedEntries += context.SaveChanges();
                    }


                    if (_transactions.TryGetValue(context, out var transaction))
                    {
                        transaction.Commit();
                        transaction.Dispose();
                    }

                }
                catch (Exception exception)
                {
                    lastError = ExceptionDispatchInfo.Capture(exception);
                }
            }

            _transactions.Clear();
            _completed = true;

            if (lastError != null)
            {
                lastError.Throw();
            }

            return savedEntries;
        }

        public void Rollback()
        {
            if (_disposed) throw new ObjectDisposedException(nameof(QueryContextCollection));

            if (_completed) throw new InvalidOperationException("Rollback can only be executed once per context. Start a new context if you wish to make more changes.");

            ExceptionDispatchInfo lastError = null;
            foreach (var context in _initializedContexts.Values)
            {
                if (_transactions.TryGetValue(context, out var transaction))
                {
                    try
                    {
                        transaction.Rollback();
                        transaction.Dispose();
                    }
                    catch (Exception exception)
                    {
                        lastError = ExceptionDispatchInfo.Capture(exception);
                    }
                }

                _transactions.Clear();
                _completed = true;

                if (lastError != null)
                {
                    lastError.Throw();
                }
            }
        }

        public void Dispose()
        {
            if (_disposed) return;

            if (!_completed)
            {
                try
                {
                    if (_readOnly) Commit();
                    else Rollback();
                }
                catch (Exception exception)
                {
                    System.Diagnostics.Debug.WriteLine(exception);
                }
            }

            foreach (var context in _initializedContexts.Values)
            {
                try
                {
                    context.Dispose();
                }
                catch (Exception exception)
                {
                    System.Diagnostics.Debug.WriteLine(exception);
                }
            }

            _initializedContexts.Clear();
            _disposed = true;
        }

    }
}
