using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.SystemModel;
using Business.SystemBusiness;
using Model.EnumModel;

namespace OAWeb.Controllers
{
    public class AuthorizeController : BaseController
    {
        AuthorizeBusiness business = new AuthorizeBusiness();
        // GET: Authorize
        [PageAuthorizeFilter]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetAuthorize(Guid roleId)
        {
            return Json(business.GetRoleAuthorize(roleId));
        }

        [LogFilter("设置", "授权管理", LogActionType.Operation)]
        public ActionResult SetAuthorize(AuthorizeModel model)
        {
            model.CreateUser = CurrentUser.Id;
            return Json(new JsonMessage(business.SetAuthorize(model)));
        }
    }
}