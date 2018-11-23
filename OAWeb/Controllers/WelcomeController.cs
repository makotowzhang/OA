using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OAWeb.Controllers
{
    public class WelcomeController : BaseController
    {
        // GET: Welcome
        public ActionResult Index()
        {
            return View();
        }
    }
}