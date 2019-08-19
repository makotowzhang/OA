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
        [PageAuthorizeFilter]
        public ActionResult Index()
        {
            return View();
        }
        [PageAuthorizeFilter]
        public ActionResult ReportCreate()
        {
            return View();
        }
        [PageAuthorizeFilter]
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
            return Json(new TableDataModel(total, data));
        }

        public ActionResult DownloadReportFile(Guid? reportId)
        {
            if (!reportId.HasValue)
            {
                return Content("参数错误");
            }
            byte[] file = service.DownloadReportFile(reportId.Value, Server.MapPath("~"), out string fileName);
            if (file == null)
            {
                return Content("报告不存在");
            }
            return File(file, "application/octet-stream", fileName);
        }

        public ActionResult ReportMobileDetail(Guid? reportId)
        {
            if (!reportId.HasValue)
            {
                return Content("参数错误");
            }
            return View(service.GetModel(reportId)??new AreaReportModel());
        }
        public ActionResult ReportDetail()
        {
            return View();
        }
    }
}