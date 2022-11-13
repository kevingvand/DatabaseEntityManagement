using DatabaseEntityManagement.Data.Context.QueryContext.Scope;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseEntityManagement.Data.Context.QueryContext.Service
{
    public class QueryContextService : IQueryContextService
    {
        public IQueryContextScope Create(EQueryScopeJoinMethod joiningMethod = EQueryScopeJoinMethod.JOIN_EXISTING)
        {
            return new QueryContextScope(joiningMethod, false, null);
        }

        public TQueryContext Get<TQueryContext>() where TQueryContext : DbContext
        {
            return QueryContextScope.GetAmbientScope()?.QueryContexts.Get<TQueryContext>();
        }
    }
}
