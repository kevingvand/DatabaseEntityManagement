using DatabaseEntityManagement.Data.Context.QueryContext.Scope;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseEntityManagement.Data.Context.QueryContext.Service
{
    public interface IQueryContextService
    {
        IQueryContextScope Create(EQueryScopeJoinMethod joiningMethod = EQueryScopeJoinMethod.JOIN_EXISTING);

        TQueryContext Get<TQueryContext>() where TQueryContext : DbContext;
    }
}
