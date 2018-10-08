using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Business.SystemBusiness;

namespace OAWeb.Controllers
{
    public class SystemLogController : BaseController
    {
        private LogBusiness business = new LogBusiness();
        // GET: SystemLog
        [PageAuthorizeFilter]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetLogList(Model.SystemModel.LogFilter filter)
        {
            return Json(business.GetLogList(filter));
        }
    }
}