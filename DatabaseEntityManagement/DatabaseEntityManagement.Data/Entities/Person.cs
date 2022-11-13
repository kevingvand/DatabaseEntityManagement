using DatabaseEntityManagement.Data.Entities._Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseEntityManagement.Data.Entities
{
    public class Person : BaseEntity
    {
        public string Name { get; set; }
    }
}
