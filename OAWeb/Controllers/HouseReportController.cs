using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.RMModel;

namespace OAWeb.Controllers
{
    public class HouseReportController : BaseController
    {
        // GET: HouseReport
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ReportCreate()
        {
            return View();
        }

        public ActionResult SaveReport(HouseReportModel model)
        {
            return Content("");
        }
    }
}