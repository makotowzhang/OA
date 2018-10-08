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
    public class RoleController : BaseController
    {
        private RoleBusiness business = new RoleBusiness();
        // GET: Role
        [PageAuthorizeFilter]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetRole(Guid roleId)
        {
            return Json(business.GetRole(roleId));
        }
        [LogFilter("新增", "角色管理", LogActionType.Operation)]
        public ActionResult AddRole(RoleModel model)
        {
            try
            {
                model.CreateUser = CurrentUser.Id;
                bool success = business.AddRole(model);
                return Json(new JsonMessage(success));
            }
            catch
            {
                return Json(new JsonMessage(false));
            }
        }

        [LogFilter("修改", "角色管理", LogActionType.Operation)]
        public ActionResult EditRole(RoleModel model)
        {
            try
            {
                model.UpdateUser = CurrentUser.Id;
                bool success = business.EditRole(model);
                return Json(new JsonMessage(success));
            }
            catch
            {
                return Json(new JsonMessage(false));
            }
        }

        public ActionResult GetRoleList(RoleFilter filter)
        {
            int total = 0;
            var data = business.GetRoleList(filter, out total);
            return Json(new TableDataModel(total, data));
        }

        public ActionResult GetAllRole()
        {
            return Json(business.GetAllRole(new RoleFilter()).Where(m=>m.IsEnabled==true));
        }

        [LogFilter("删除", "角色管理", LogActionType.Operation)]
        public ActionResult DeleteRole(List<RoleModel> model)
        {
            Guid updateUser = CurrentUser.Id;
            if (model != null)
            {
                foreach (var m in model)
                {
                    m.UpdateUser = updateUser;
                }
            }
            return Json(new JsonMessage(business.DeleteRole(model)));
        }
    }
}