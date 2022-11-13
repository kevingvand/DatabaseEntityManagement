using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DatabaseEntityManagement.Web.Controllers.Web
{
    [RoutePrefix("")]
    public class HomeController : Controller
    {
        // GET: Home

        [Route("")]
        public ActionResult Index()
        {
            return View();
        }
    }
}