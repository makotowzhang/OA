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
    public class AreaReportController : BaseController
    {
        AreaReportBusiness service = new AreaReportBusiness();
        // GET: AreaReport
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ReportCreate()
        {
            return View();
        }

        public ActionResult ReportAudit()
        {
            return View();
        }

        public ActionResult GetUserCreateReport()
        {
            return EnumJson(service.GetOrCreateModel(CurrentUser.Id));
        }

        public ActionResult GetRoportModel(Guid id)
        {
            return EnumJson(service.GetModel(id));
        }

        public ActionResult SaveReport(AreaReportModel model)
        {
            model.UpdateUser = CurrentUser.Id;
            return Json(new JsonMessage(service.SaveReport(model)));
        }

        public ActionResult SubmitReport(AreaReportModel model)
        {
            model.UpdateUser = CurrentUser.Id;
            return Json(new JsonMessage(service.SaveReport(model,true)));
        }

        public ActionResult Audit(AreaReportModel model)
        {
            try
            {
                model.AuditUser = CurrentUser.Id;
                return Json(new JsonMessage(service.Audit(model)));
            }
            catch (Exception e)
            {
                return Json(new JsonMessage(false, e.Message));
            }
        }

        public ActionResult GetList(AreaReportFilter filter)
        {
            if (filter.ListType == ListType.Personal)
            {
                filter.CreateUserId = CurrentUser.Id;
            }
            if (filter.ListType == ListType.Auditor)
            {
                filter.AuditUserId = CurrentUser.Id;
            }
            var data = service.GetReportList(filter, out int total);
            return Json(new TableDataModel(0, data));
        }
    }
}