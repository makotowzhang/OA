using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OAWeb.Controllers
{
    public class AchievementsController : BaseController
    {
        // GET: Achievements
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Personal()
        {
            return View();
        }

        public ActionResult Summary()
        {
            return View();
        }
    }
}