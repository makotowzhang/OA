using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Business.PMBusiness;
using Model.SystemModel;

namespace OAWeb.Controllers
{
    public class WelcomeController : BaseController
    {
        // GET: Welcome
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult GetWeekThingsTodo()
        {
          
            var data = new ThingsTodoBusiness().GetWeekThingsTodo(CurrentUser.Id);
            return Json(data);
        }

        public ActionResult GetDailyThingsTodo()
        {
            var data = new ThingsTodoBusiness().GetDailyThingsTodo(CurrentUser.Id);
            return Json(data);
        }

        public ActionResult DailyAlert(int? id)
        {
            try
            {
                return Json(new JsonMessage(new ThingsTodoBusiness().DailyAlert(id)));
            }
            catch (Exception e)
            {
                return Json(new JsonMessage(false, e.Message));
            }
        }
    }
}