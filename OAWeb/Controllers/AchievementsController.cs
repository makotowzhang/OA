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
        [PageAuthorizeFilter]
        public ActionResult Personal()
        {
            return View();
        }
        [PageAuthorizeFilter]
        public ActionResult Summary()
        {
            return View();
        }

        public ActionResult GetPersonalAchievementsList(AchievementsFilter filter)
        {
            filter.CreateUser = CurrentUser.Id;
            var data = service.GetAchievementsList(filter, out int total);
            return Json(new TableDataModel(total, data));
        }

        public ActionResult GetAllAchievementsList(AchievementsFilter filter)
        {
            var data = service.GetAchievementsList(filter, out int total);
            return Json(new TableDataModel(total, data));
        }

        public ActionResult GetPersonalChartData(AchievementsFilter filter)
        {
            filter.CreateUser = CurrentUser.Id;
            var data = service.GetChartData(filter);
            return Json( data);
        }

        public ActionResult GetAllChartData(AchievementsFilter filter)
        {
            var data = service.GetSummaryChartData(filter);
            return Json(data);
        }

        public ActionResult ExportPersonalAchievementsList(AchievementsFilter filter)
        {
            filter.CreateUser = CurrentUser.Id;
            return File(service.ExportPersonalAchievementsList(filter), "application/octet-stream", "绩效列表.xlsx");
        }

        public ActionResult ExportAllAchievementsList(AchievementsFilter filter)
        {
            return File(service.ExportPersonalAchievementsList(filter,true), "application/octet-stream", "绩效列表.xlsx");
        }
    }
}