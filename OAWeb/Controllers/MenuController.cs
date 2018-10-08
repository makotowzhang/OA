using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Business.SystemBusiness;
using Model.EnumModel;
using Model.SystemModel;


namespace OAWeb.Controllers
{
    public class MenuController : BaseController
    {
        MenuBusiness business = new MenuBusiness();
        // GET: Menu
        [PageAuthorizeFilter]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetAllMenu()
        {
            return Json(business.GetAllMenu());
        }
        [LogFilter("新增", "菜单管理", LogActionType.Operation)]
        public ActionResult AddMenu(MenuModel model)
        {
            model.CreateUser = CurrentUser.Id;
            Guid menuId = Guid.Empty;
            return Json(new JsonMessage(business.AddMenu(model,out menuId),menuId.ToString()));
        }

        [LogFilter("修改", "菜单管理", LogActionType.Operation)]
        public ActionResult EditMenu(MenuModel model)
        {
            model.UpdateUser = CurrentUser.Id;
            return Json(new JsonMessage(business.EditMenu(model)));
        }

        [LogFilter("删除", "菜单管理", LogActionType.Operation)]
        public ActionResult DeleteMenu(MenuModel model)
        {
            model.UpdateUser = CurrentUser.Id;
            return Json(new JsonMessage(business.DeleteMenu(model)));
        }

        public ActionResult GetMenu(Guid menuId)
        {
            return Json(business.GetMenuById(menuId));
        }

    }
}