using DatabaseEntityManagement.Common;
using DatabaseEntityManagement.Data.Context.QueryContext.Service;
using DatabaseEntityManagement.Data.Entities;
using DatabaseEntityManagement.Services.Person;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DatabaseEntityManagement.Web.Controllers.API
{
    [RoutePrefix(Constants.API.Prefix + "/Persons")]
    public class PersonsController : ApiController
    {
        private readonly IPersonService _personService;

        public PersonsController(IPersonService personService)
        {
            _personService = personService;
        }

        /// <summary>
        /// Retrieves a single element by the specified ID.
        /// </summary>
        /// <param name="id">The ID of the element to retrieve</param>
        [HttpGet]
        [Route("{id}")]
        public IHttpActionResult GetById(int id)
        {
            var person = _personService.GetById(id);
            if (person == null) return NotFound();

            return Ok(person);
        }

        /// <summary>
        /// Inserts the specified object to the database
        /// </summary>
        /// <param name="person">The object to add</param>
        [HttpPost]
        [Route("")]
        public IHttpActionResult Create(Person person)
        {
            var createdPerson = _personService.Create(person);
            if(createdPerson == null) return BadRequest(); //TODO: proper validation

            return Ok(createdPerson);
        }
    }
}
