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
    public class MoneyApplyController : BaseController
    {
        MoneyApplyBusiness business = new MoneyApplyBusiness();
        // GET: MaterielApply
        public ActionResult Index()
        {
            return View();
        }
        [PageAuthorizeFilter]
        public ActionResult PersonalIndex()
        {
            return View();
        }
        [PageAuthorizeFilter]
        public ActionResult AuditorIndex()
        {
            return View();
        }

        public ActionResult GetList(MoneyApplyFilter filter)
        {
            if (filter.ListType == ListType.Personal)
            {
                filter.CreateUserId = CurrentUser.Id;
            }
            if (filter.ListType == ListType.Auditor)
            {
                filter.AuditUserId = CurrentUser.Id;
            }
            var data = business.GetPMAList(filter, out int total);
            return Json(new TableDataModel(total, data));
        }

        public ActionResult GetMATModel(Guid id)
        {
            return Json(business.GetModel(id));
        }

        [LogFilter("申请", "物料申请", LogActionType.Apply)]
        public ActionResult Apply(MoneyApplyModel model)
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

        [LogFilter("修改", "物料申请", LogActionType.Operation)]
        public ActionResult Update(MoneyApplyModel model)
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

        [LogFilter("删除", "物料申请", LogActionType.Operation)]
        public ActionResult Delete(List<MoneyApplyModel> list)
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


        [LogFilter("审核", "物料审核", LogActionType.Operation)]
        public ActionResult Audit(MoneyApplyModel model)
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
    }
}