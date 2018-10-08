using Model.SystemModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Business.SystemBusiness;

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
            return Json(new AuthorizeBusiness().GetAuthorizeAction(menuId,CurrentUser.UserRole).Select(m=>m.ActionCode ));
        }
    }
}