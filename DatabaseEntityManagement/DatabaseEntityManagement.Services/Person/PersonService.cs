using DatabaseEntityManagement.Data.Context.QueryContext.Service;
using DatabaseEntityManagement.Data.Repositories.Person;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseEntityManagement.Services.Person
{
    public class PersonService : IPersonService
    {
        private readonly IQueryContextService _queryContextService;
        private readonly IPersonRepository _personRepository;

        public PersonService(IQueryContextService queryContextService, IPersonRepository personRepository)
        {
            _queryContextService = queryContextService;
            _personRepository = personRepository;
        }

        public Data.Entities.Person Create(Data.Entities.Person person)
        {
            //TODO: Validation
            using (var context = _queryContextService.Create())
            {
                _personRepository.Insert(person);
                return person;
            }
        }

        public Data.Entities.Person GetById(int id)
        {
            using (var context = _queryContextService.Create())
            {
                return _personRepository.GetById(id);
            }
        }
    }
}
