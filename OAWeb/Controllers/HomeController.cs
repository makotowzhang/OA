using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.IO;
using System.Xml;
using System.Text;
using System.Xml.Serialization;
using Model.SystemModel;
using Business.SystemBusiness;

namespace OAWeb.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetMenu()
        {
            return Json(new MenuBusiness().GetNavMenu(CurrentUser.UserRole));
        }

        public ActionResult Test()
        {
            return Json(CurrentUser,JsonRequestBehavior.AllowGet);
        }

        public ActionResult CheckCurrentPwd(string password)
        {
            if (Common.MD5Encrypt.MD5Encrypt64(password) == CurrentUser.Password)
            {
                return Json(new JsonMessage(true));
            }
            else
            {
                return Json(new JsonMessage(false));
            }
        }

        public ActionResult ChangePassword(ChangePwdModel model)
        {
            if (Common.MD5Encrypt.MD5Encrypt64(model.CurrentPwd) != CurrentUser.Password)
            {
                return Json(new JsonMessage(false,"原密码不正确"));
            }
            if (model.ConfirmPwd != model.NewPwd)
            {
                return Json(new JsonMessage(false, "确认密码与新密码不一致"));
            }
            bool res = new UserBusiness().ChangePwd(CurrentUser.Id, Common.MD5Encrypt.MD5Encrypt64(model.NewPwd));
            return Json(new JsonMessage(res));
        }
    }
}