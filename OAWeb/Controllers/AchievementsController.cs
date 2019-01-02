using Business.PMBusiness;
using Model.PMModel;
using Model.SystemModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OAWeb.Controllers
{
    public class AchievementsController : BaseController
    {
        private readonly AchievementsBusiness service = new AchievementsBusiness();
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

        public ActionResult GetPersonalAchievementsList(AchievementsFilter filter) {
            filter.CreateUser = CurrentUser.Id;
            var data = service.GetAchievementsList(filter, out int total);
            return Json(new TableDataModel(total, data));
        }

        public ActionResult GetPersonalChartData(AchievementsFilter filter) {
            filter.CreateUser = CurrentUser.Id;
            var data = service.GetChartData(filter);
            return Json( data);
        }
    }
}