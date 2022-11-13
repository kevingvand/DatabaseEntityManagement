using DatabaseEntityManagement.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DatabaseEntityManagement.Web.Controllers.API
{
    [RoutePrefix(Constants.API.Prefix + "/ApplicationInfo")]
    public class ApplicationInfoController : ApiController
    {
        /// <summary>
        /// Retrieves the application version from the web configuration
        /// </summary>
        /// <returns>A string with the application version</returns>
        [HttpGet]
        [Route("Version")]
        public IHttpActionResult GetVersion()
        {
            var version = ConfigurationManager.AppSettings["AppVersion"];

            if (version == null) return InternalServerError(new Exception("Could not read application version from the configuration."));

            return Ok(version);
        }
    }
}
