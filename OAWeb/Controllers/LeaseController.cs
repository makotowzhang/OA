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
    public class LeaseController : BaseController
    {
        private LeaseBusiness business = new LeaseBusiness();

        // GET: Employee
        [PageAuthorizeFilter]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetLeaList(LeaseFilter filter)
        {
            var data = business.GetLeaList(filter, out int total);
            return Json(new TableDataModel(total, data));
        }

        public ActionResult GetLeaModel(int? id)
        {
            return Json(business.GetModel(id));
        }

        [LogFilter("编辑", "借物管理", LogActionType.Operation)]
        public ActionResult Save(LeaseModel lea)
        {
            try
            {
                if (!lea.Id.HasValue)
                {
                    lea.CreateUser = CurrentUser.Id;
                }
                else
                {
                    lea.UpdateUser = CurrentUser.Id;
                }
                return Json(new JsonMessage(business.Save(lea)));
            }
            catch (Exception e)
            {
                return Json(new JsonMessage(false, e.Message));
            }
        }


        [LogFilter("删除", "借物管理", LogActionType.Operation)]
        public ActionResult Delete(List<LeaseModel> lea)
        {
            try
            {
                lea.ForEach(m => m.UpdateUser = CurrentUser.Id);
                return Json(new JsonMessage(business.Delete(lea)));
            }
            catch (Exception e)
            {
                return Json(new JsonMessage(false, e.Message));
            }
        }

        public ActionResult GetAllLease()
        {
            return Json(business.GetAllLease());
        }

    }
}