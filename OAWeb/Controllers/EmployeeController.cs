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
    public class EmployeeController : BaseController
    {
        private EmployeeBusiness business = new EmployeeBusiness();

        // GET: Employee
        [PageAuthorizeFilter]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetEmpList(EmployeeFilter filter)
        {
            var data = business.GetEmpList(filter, out int total);
            return Json(new TableDataModel(total, data));
        }

        public ActionResult GetEmpModel(Guid id)
        {
            return Json(business.GetModel(id));
        }

        [LogFilter("编辑", "员工管理", LogActionType.Operation)]
        public ActionResult Save(EmployeeModel emp)
        {
            try
            {
                if (!emp.Id.HasValue)
                {
                    emp.CreateUser = CurrentUser.Id;
                }
                else
                {
                    emp.UpdateUser = CurrentUser.Id;
                }
                return Json(new JsonMessage(business.Save(emp)));
            }
            catch (Exception e)
            {
                return Json(new JsonMessage(false, e.Message));
            }
        }


        [LogFilter("删除", "员工管理", LogActionType.Operation)]
        public ActionResult Delete(List<EmployeeModel> emp)
        {
            try
            {
                emp.ForEach(m => m.UpdateUser = CurrentUser.Id);
                return Json(new JsonMessage(business.Delete(emp)));
            }
            catch (Exception e)
            {
                return Json(new JsonMessage(false, e.Message));
            }
        }

        public ActionResult CheckEmpCode(string empCode,Guid? id)
        {
            return Json(new JsonMessage(business.CheckEmpCode(empCode, id)));
        }

        public ActionResult GetUserEmp()
        {
            return Json(business.GetUserEmpList());
        }
    }
}