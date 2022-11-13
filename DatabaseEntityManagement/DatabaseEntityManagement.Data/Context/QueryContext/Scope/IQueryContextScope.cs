using DatabaseEntityManagement.Data.Context.QueryContext.Collection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseEntityManagement.Data.Context.QueryContext.Scope
{
    public interface IQueryContextScope : IDisposable
    {
        /// <summary>
        /// Saves the changes in all the QueryContext instances that were created within this scope.
        /// </summary>
        int SaveChanges();

        /// <summary>
        /// The QueryContext instances that this Context Scope manages.
        /// </summary>
        IQueryContextCollection QueryContexts { get; }
    }
}
