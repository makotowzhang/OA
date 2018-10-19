using Model.SystemModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Business.SystemBusiness;
using Newtonsoft.Json;

namespace OAWeb.Controllers
{
    public class BaseController : Controller
    {
        // GET: Base
        public UserModel CurrentUser
        {
            get
            {
                if (string.IsNullOrWhiteSpace(User.Identity.Name))
                {
                    return null;
                }
                return new LoginBusiness().GetUserInfoById(Guid.Parse(User.Identity.Name));
            }
        }

        public ActionResult GetPageAuthorize(Guid menuId)
        {
            return Json(new AuthorizeBusiness().GetAuthorizeAction(menuId, CurrentUser.UserRole).Select(m => m.ActionCode));
        }

        public EnumJsonResult EnumJson(object data)
        {
            EnumJsonResult result = new EnumJsonResult();
            result.Data = data;
            return result;
        }
    }

    public class EnumJsonResult : ActionResult
    {
        public object Data
        {
            get;
            set;
        }
        public override void ExecuteResult(ControllerContext context)
        {
            HttpResponseBase response = context.HttpContext.Response;
            response.ContentType = "application/json";
            if (this.Data != null)
            {
                response.Write(JsonConvert.SerializeObject(Data));
            }
        }
    }
}