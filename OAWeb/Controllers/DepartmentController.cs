using Model.EnumModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.PMModel;
using Business.PMBusiness;
using Model.SystemModel;

namespace OAWeb.Controllers
{
    public class DepartmentController : BaseController
    {
        private DepartmentBusiness business = new DepartmentBusiness();
        
        // GET: Department
        [PageAuthorizeFilter]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetDepList(DepartmentFilter filter)
        {
            var data = business.GetDepList(filter, out int total);
            return Json(new TableDataModel(total, data));
        }

        public ActionResult GetDepModel(Guid id)
        {
            return Json(business.GetModel(id));
        }

        [LogFilter("编辑", "部门管理", LogActionType.Operation)]
        public ActionResult Save(DepartmentModel dep)
        {
            try
            {
                if (dep.Id == null)
                {
                    dep.CreateUser = CurrentUser.Id;
                }
                else
                {
                    dep.UpdateUser = CurrentUser.Id;
                }
                return Json(new JsonMessage(business.Save(dep)));
            }
            catch(Exception e)
            {
                return Json(new JsonMessage(false, e.Message));
            }
        }


        [LogFilter("删除", "部门管理", LogActionType.Operation)]
        public ActionResult Delete(List<DepartmentModel> dep)
        {
            try
            {
                dep.ForEach(m => m.UpdateUser = CurrentUser.Id);
                return Json(new JsonMessage(business.Delete(dep)));
            }
            catch(Exception e)
            {
                return Json(new JsonMessage(false, e.Message));
            }
        }
    }
}