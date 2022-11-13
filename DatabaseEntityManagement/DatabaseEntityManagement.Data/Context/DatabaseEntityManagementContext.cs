using DatabaseEntityManagement.Data.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseEntityManagement.Data.Context
{
    public class DatabaseEntityManagementContext : DbContext
    {
        public DbSet<Person> Persons { get; set; }

        public DatabaseEntityManagementContext() : base()
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}