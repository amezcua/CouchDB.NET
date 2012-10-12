using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TestWebSite.Controllers
{
    [HandleError]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewData["Message"] = "ASP.NET MVC with CouchDB!";

            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        [Authorize(Roles="Admin")]
        public ActionResult Admin()
        {
            return View();
        }
    }
}
