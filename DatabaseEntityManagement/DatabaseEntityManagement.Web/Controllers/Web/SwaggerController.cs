using DatabaseEntityManagement.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Description;
using System.Web.Mvc;

namespace DatabaseEntityManagement.Web.Controllers.Web
{
    [RoutePrefix("api")]
    public class SwaggerController : Controller
    {
        [Route("")]
        public ActionResult Index()
        {
            return Redirect("/swagger");
        }
    }
}