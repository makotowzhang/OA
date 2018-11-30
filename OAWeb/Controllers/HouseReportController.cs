using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.RMModel;
using Business.RMBusiness;
using Model.SystemModel;
using Model.EnumModel;

namespace OAWeb.Controllers
{
    public class HouseReportController : BaseController
    {
        HouseReportBusiness service = new HouseReportBusiness();
        // GET: HouseReport
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ReportCreate()
        {
            return View();
        }

        public ActionResult GetUserCreateReport()
        {
            return EnumJson(service.GetOrCreateModel(CurrentUser.Id));
        }

        public ActionResult SaveReport(HouseReportModel model)
        {
            model.UpdateUser = CurrentUser.Id;
            return Json(new JsonMessage(service.SaveReport(model)));
        }

        public ActionResult SubmitReport(HouseReportModel model)
        {
            model.UpdateUser = CurrentUser.Id;
            return Json(new JsonMessage(service.SaveReport(model,true)));
        }

        public ActionResult GetList(HouseReportFilter filter)
        {
            if (filter.ListType == ListType.Personal)
            {
                filter.CreateUserId = CurrentUser.Id;
            }
            if (filter.ListType == ListType.Auditor)
            {
                filter.AuditUserId = CurrentUser.Id;
            }
            //var data = service.GetLEAList(filter, out int total);
            var data = new List<object>();
            return Json(new TableDataModel(0, data));
        }
    }
}