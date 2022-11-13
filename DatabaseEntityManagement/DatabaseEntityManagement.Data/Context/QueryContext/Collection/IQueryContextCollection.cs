using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseEntityManagement.Data.Context.QueryContext.Collection
{
    public interface IQueryContextCollection : IDisposable
    {
        /// <summary>
        /// Get or create a QueryContext instance of the specified type.
        /// </summary>
        TQueryContext Get<TQueryContext>() where TQueryContext : DbContext;
    }
}
