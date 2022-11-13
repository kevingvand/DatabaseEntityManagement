using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities = DatabaseEntityManagement.Data.Entities;


namespace DatabaseEntityManagement.Services.Person
{
    public interface IPersonService
    {
        Entities.Person Create(Entities.Person person);
        Entities.Person GetById(int id);
    }
}
