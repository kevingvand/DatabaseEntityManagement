using DatabaseEntityManagement.Data.Context;
using DatabaseEntityManagement.Data.Context.QueryContext.Service;
using DatabaseEntityManagement.Data.Repositories._Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseEntityManagement.Data.Repositories.Person
{
    public class PersonRepository : BaseRepository<DatabaseEntityManagementContext, Entities.Person>, IPersonRepository
    {
        public PersonRepository(IQueryContextService queryContextService) : base(queryContextService)
        {
        }
    }
}
