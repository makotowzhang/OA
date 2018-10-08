using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Business.SystemBusiness;
using Model.SystemModel;

namespace OAWeb.Controllers
{
    public class SysMessageController : BaseController
    {
        private SysMessageBusiness business = new SysMessageBusiness();
        // GET: SysMessage
        [PageAuthorizeFilter]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetMsgList(SysMessageFilter filter)
        {
            filter.ToUser = CurrentUser.Id;
            var data = business.GetSysMessageList(filter, out int total);
            return Json(new TableDataModel(total, data));
        }

        public ActionResult GetNotReadCount()
        {
            return Json(business.GetNotReadCount(CurrentUser.Id));
        }
    }
}