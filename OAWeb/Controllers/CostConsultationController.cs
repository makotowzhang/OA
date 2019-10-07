using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.CCModel;
using Model.SystemModel;
using Business.CCBusiness;
using Model.EnumModel;

namespace OAWeb.Controllers
{
    public class CostConsultationController : BaseController
    {
        CostConsultationBusiness service = new CostConsultationBusiness();
        // GET: CostConsultation
        public ActionResult Index()
        {
            return View();
        }

        #region 预算编制
        public ActionResult BudgetingIndex()
        {
            return View();
        }

        public ActionResult BudgetingAuditIndex() 
        {
            return View();
        }

        public ActionResult AddBudgetingIndex(Guid? reportid, int? isauidt, int? isedit)
        {
            ViewBag.reportid = reportid;
            ViewBag.isaudit = isauidt;
            ViewBag.isedit = isedit;
            return View();
        }

        public ActionResult GetBudgetingList(BudgetingFilter filter)
        {
            if (filter.ListType == ListType.Personal)
            {
                filter.CreateUserId = CurrentUser.Id;
            }
            if (filter.ListType == ListType.Auditor)
            {
                filter.AuditUserId = CurrentUser.Id;
            }
            var data = service.GetBudgetingList(filter, out int total);
            return Json(new TableDataModel(total, data));
        }

        public ActionResult GetBudgetingCreateReport()
        {
            return EnumJson(service.GetOrCreateBudgeting(CurrentUser.Id));
        }

        public ActionResult GetBudgetingModel(Guid id)
        {
            return EnumJson(service.GetBudgetingModel(id));
        }

        public ActionResult SaveBudgeting(BudgetingModel model)
        {
            model.UpdateUser = CurrentUser.Id;
            return Json(new JsonMessage(service.SaveBudgeting(model)));
        }


        public ActionResult SubmitBudgeting(BudgetingModel model)
        {
            model.UpdateUser = CurrentUser.Id;
            return Json(new JsonMessage(service.SaveBudgeting(model, true)));
        }

        public ActionResult AuditBudgeting(BudgetingModel model)
        {
            try
            {
                model.AuditUser = CurrentUser.Id;
                return Json(new JsonMessage(service.AuditBudgeting(model)));
            }
            catch (Exception e)
            {
                return Json(new JsonMessage(false, e.Message));
            }
        }
        #endregion

        #region 概算编制

        public ActionResult EstimateIndex()
        {
            return View();
        }

        public ActionResult EstimateAuditIndex()
        {
            return View();
        }

        public ActionResult AddEstimateIndex(Guid? reportid, int? isauidt, int? isedit)
        {
            ViewBag.reportid = reportid;
            ViewBag.isaudit = isauidt;
            ViewBag.isedit = isedit;
            return View();
        }

        public ActionResult GetEstimateList(EstimateFilter filter)
        {
            if (filter.ListType == ListType.Personal)
            {
                filter.CreateUserId = CurrentUser.Id;
            }
            if (filter.ListType == ListType.Auditor)
            {
                filter.AuditUserId = CurrentUser.Id;
            }
            var data = service.GetEstimateList(filter, out int total);
            return Json(new TableDataModel(total, data));
        }

        public ActionResult GetEstimateCreateReport()
        {
            return EnumJson(service.GetOrCreateEstimate(CurrentUser.Id));
        }

        public ActionResult GetEstimateModel(Guid id)
        {
            return EnumJson(service.GetEstimateModel(id));
        }

        public ActionResult SaveEstimate(EstimateModel model)
        {
            model.UpdateUser = CurrentUser.Id;
            return Json(new JsonMessage(service.SaveEstimate(model)));
        }


        public ActionResult SubmitEstimate(EstimateModel model)
        {
            model.UpdateUser = CurrentUser.Id;
            return Json(new JsonMessage(service.SaveEstimate(model, true)));
        }

        public ActionResult AuditEstimate(EstimateModel model)
        {
            try
            {
                model.AuditUser = CurrentUser.Id;
                return Json(new JsonMessage(service.AuditEstimate(model)));
            }
            catch (Exception e)
            {
                return Json(new JsonMessage(false, e.Message));
            }
        }
        #endregion

        #region 结算审计

        public ActionResult SettlementIndex()
        {
            return View();
        }

        public ActionResult SettlementAuditIndex()
        {
            return View();
        }

        public ActionResult AddSettlementIndex(Guid? reportid, int? isauidt, int? isedit)
        {
            ViewBag.reportid = reportid;
            ViewBag.isaudit = isauidt;
            ViewBag.isedit = isedit;
            return View();
        }

        public ActionResult GetSettlementList(SettlementFilter filter)
        {
            if (filter.ListType == ListType.Personal)
            {
                filter.CreateUserId = CurrentUser.Id;
            }
            if (filter.ListType == ListType.Auditor)
            {
                filter.AuditUserId = CurrentUser.Id;
            }
            var data = service.GetSettlementList(filter, out int total);
            return Json(new TableDataModel(total, data));
        }

        public ActionResult GetSettlementCreateReport()
        {
            return EnumJson(service.GetOrCreateSettlement(CurrentUser.Id));
        }

        public ActionResult GetSettlementModel(Guid id)
        {
            return EnumJson(service.GetSettlementModel(id));
        }

        public ActionResult SaveSettlement(SettlementModel model)
        {
            model.UpdateUser = CurrentUser.Id;
            return Json(new JsonMessage(service.SaveSettlement(model)));
        }


        public ActionResult SubmitSettlement(SettlementModel model)
        {
            model.UpdateUser = CurrentUser.Id;
            return Json(new JsonMessage(service.SaveSettlement(model, true)));
        }

        public ActionResult AuditSettlement(SettlementModel model)
        {
            try
            {
                model.AuditUser = CurrentUser.Id;
                return Json(new JsonMessage(service.AuditSettlement(model)));
            }
            catch (Exception e)
            {
                return Json(new JsonMessage(false, e.Message));
            }
        }
        #endregion

        #region 投资评审

        public ActionResult InvestmentIndex()
        {
            return View();
        }

        public ActionResult InvestmentAuditIndex()
        {
            return View();
        }

        public ActionResult AddInvestmentIndex(Guid? reportid, int? isauidt, int? isedit)
        {
            ViewBag.reportid = reportid;
            ViewBag.isaudit = isauidt;
            ViewBag.isedit = isedit;
            return View();
        }

        public ActionResult GetInvestmentList(InvestmentFilter filter)
        {
            if (filter.ListType == ListType.Personal)
            {
                filter.CreateUserId = CurrentUser.Id;
            }
            if (filter.ListType == ListType.Auditor)
            {
                filter.AuditUserId = CurrentUser.Id;
            }
            var data = service.GetInvestmentList(filter, out int total);
            return Json(new TableDataModel(total, data));
        }

        public ActionResult GetInvestmentCreateReport()
        {
            return EnumJson(service.GetOrCreateInvestment(CurrentUser.Id));
        }

        public ActionResult GetInvestmentModel(Guid id)
        {
            return EnumJson(service.GetInvestmentModel(id));
        }

        public ActionResult SaveInvestment(InvestmentModel model)
        {
            model.UpdateUser = CurrentUser.Id;
            return Json(new JsonMessage(service.SaveInvestment(model)));
        }


        public ActionResult SubmitInvestment(InvestmentModel model)
        {
            model.UpdateUser = CurrentUser.Id;
            return Json(new JsonMessage(service.SaveInvestment(model, true)));
        }

        public ActionResult AuditInvestment(InvestmentModel model)
        {
            try
            {
                model.AuditUser = CurrentUser.Id;
                return Json(new JsonMessage(service.AuditInvestment(model)));
            }
            catch (Exception e)
            {
                return Json(new JsonMessage(false, e.Message));
            }
        }
        #endregion

        #region 司法鉴定

        public ActionResult AppraisalIndex()
        {
            return View();
        }

        public ActionResult AppraisalAuditIndex()
        {
            return View();
        }

        public ActionResult AddAppraisalIndex(Guid? reportid, int? isauidt, int? isedit)
        {
            ViewBag.reportid = reportid;
            ViewBag.isaudit = isauidt;
            ViewBag.isedit = isedit;
            return View();
        }

        public ActionResult GetAppraisalList(AppraisalFilter filter)
        {
            if (filter.ListType == ListType.Personal)
            {
                filter.CreateUserId = CurrentUser.Id;
            }
            if (filter.ListType == ListType.Auditor)
            {
                filter.AuditUserId = CurrentUser.Id;
            }
            var data = service.GetAppraisalList(filter, out int total);
            return Json(new TableDataModel(total, data));
        }

        public ActionResult GetAppraisalCreateReport()
        {
            return EnumJson(service.GetOrCreateAppraisal(CurrentUser.Id));
        }

        public ActionResult GetAppraisalModel(Guid id)
        {
            return EnumJson(service.GetAppraisalModel(id));
        }

        public ActionResult SaveAppraisal(AppraisalModel model)
        {
            model.UpdateUser = CurrentUser.Id;
            return Json(new JsonMessage(service.SaveAppraisal(model)));
        }


        public ActionResult SubmitAppraisal(AppraisalModel model)
        {
            model.UpdateUser = CurrentUser.Id;
            return Json(new JsonMessage(service.SaveAppraisal(model, true)));
        }

        public ActionResult AuditAppraisal(AppraisalModel model)
        {
            try
            {
                model.AuditUser = CurrentUser.Id;
                return Json(new JsonMessage(service.AuditAppraisal(model)));
            }
            catch (Exception e)
            {
                return Json(new JsonMessage(false, e.Message));
            }
        }
        #endregion
    }
}