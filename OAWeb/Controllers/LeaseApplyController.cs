using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.PMModel;
using Business.PMBusiness;
using Model.SystemModel;
using Model.EnumModel;

namespace OAWeb.Controllers
{
    public class LeaseApplyController : BaseController
    {
        LeaseApplyBusiness business = new LeaseApplyBusiness();
        // GET: LeaseApply
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult PersonalIndex()
        {
            return View();
        }

        public ActionResult AuditorIndex()
        {
            return View();
        }

        public ActionResult GetList(LeaseApplyFilter filter)
        {
            if (filter.ListType == ListType.Personal)
            {
                filter.CreateUserId = CurrentUser.Id;
            }
            if (filter.ListType == ListType.Auditor)
            {
                filter.AuditUserId = CurrentUser.Id;
            }
            var data = business.GetLEAList(filter, out int total);
            return Json(new TableDataModel(total, data));
        }

        public ActionResult GetLEAModel(Guid id)
        {
            return Json(business.GetModel(id));
        }

        [LogFilter("申请", "借物申请", LogActionType.Apply)]
        public ActionResult Apply(LeaseApplyModel model)
        {
            try
            {
                model.CreateUser = CurrentUser.Id;
                return Json(new JsonMessage(business.Save(model)));
            }
            catch (Exception e)
            {
                return Json(new JsonMessage(false, e.Message));
            }
        }

        [LogFilter("修改", "借物申请", LogActionType.Operation)]
        public ActionResult Update(LeaseApplyModel model)
        {
            try
            {
                model.UpdateUser = CurrentUser.Id;
                return Json(new JsonMessage(business.Save(model)));
            }
            catch (Exception e)
            {
                return Json(new JsonMessage(false, e.Message));
            }
        }

        [LogFilter("删除", "借物申请", LogActionType.Operation)]
        public ActionResult Delete(List<LeaseApplyModel> list)
        {
            try
            {
                list.ForEach(m => m.UpdateUser = CurrentUser.Id);
                return Json(new JsonMessage(business.Delete(list)));
            }
            catch (Exception e)
            {
                return Json(new JsonMessage(false, e.Message));
            }
        }


        [LogFilter("审核", "借物审核", LogActionType.Operation)]
        public ActionResult Audit(LeaseApplyModel model)
        {
            try
            {
                model.AuditUser = CurrentUser.Id;
                return Json(new JsonMessage(business.Audit(model)));
            }
            catch (Exception e)
            {
                return Json(new JsonMessage(false, e.Message));
            }
        }

        [LogFilter("归还", "借物审核", LogActionType.Operation)]
        public ActionResult Return(LeaseApplyModel model)
        {
            try
            {
                return Json(new JsonMessage(business.Return(model)));
            }
            catch (Exception e)
            {
                return Json(new JsonMessage(false, e.Message));
            }
        }
    }
}