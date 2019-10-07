using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;
using Model.CCModel;
using AutoMapper;
using Model.EnumModel;
using Common;

namespace Business.CCBusiness
{
    public class CostConsultationBusiness
    {
        #region 预算编制
        public BudgetingModel GetBudgetingModel(Guid id)
        {
            using (DataProvider dp = new DataProvider())
            {
                BudgetingModel model = Mapper.Map<BudgetingModel>(dp.CC_Budgeting.FirstOrDefault(m => m.Id == id));
                model.AuditDep = dp.PM_Department.Where(m => !m.IsDel &&
                                                       dp.RM_ReportAudit.Where(x => x.ReportId == model.Id).Select(x => x.AuditId).Contains(m.Id))
                                                     .Select(m => m.Id).ToList();
                model.AuditUserIds = dp.System_User.Where(m => m.IsDel == false &&
                                                         dp.RM_ReportAudit.Where(x => x.ReportId == model.Id).Select(x => x.AuditId).Contains(m.Id))
                                                      .Select(m => m.Id).ToList();
                model.AuditUserName = dp.System_User.Where(m => m.Id == model.AuditUser).Select(m => m.TrueName).FirstOrDefault();
                return model;
            }

        }

        public BudgetingModel GetOrCreateBudgeting(Guid createUserId)
        {
            Guid? modelId = null;
            using (DataProvider dp = new DataProvider())
            {
                string reportStatus = ReportStatus.WaitSubmit.ToString();
                if (dp.CC_Budgeting.Count(m => m.CreateUser == createUserId && !m.IsDel && m.ReportStatus == reportStatus) > 0)
                {
                    modelId = dp.CC_Budgeting.Where(m => m.CreateUser == createUserId && !m.IsDel && m.ReportStatus == reportStatus).Select(m => m.Id).FirstOrDefault();
                }
                else
                {
                    CC_Budgeting model = new CC_Budgeting()
                    {
                        Id = Guid.NewGuid(),
                        IsDel = false,
                        ChargeStatus = true,
                        IsInvoice = true,
                        ReportType = ReportType.Formal.ToString(),
                        ReportStatus = ReportStatus.WaitSubmit.ToString(),
                        CreateUser = createUserId,
                        CreateTime = DateTime.Now,
                    };
                    dp.CC_Budgeting.Add(model);
                    dp.SaveChanges();
                    return Mapper.Map<BudgetingModel>(model);
                }
            }
            return GetBudgetingModel(modelId.Value);
        }

        public bool SaveBudgeting(BudgetingModel model, bool isSubmit = false)
        {
            using (DataProvider dp = new DataProvider())
            {
                CC_Budgeting entity = dp.CC_Budgeting.FirstOrDefault(m => m.Id == model.Id);
                entity.ReportCode = model.ReportCode;
                entity.ReportName = model.ReportName;
                entity.EntrustingParty = model.EntrustingParty;
                entity.BudgetAmount = model.BudgetAmount;
                entity.CostConsultantor = model.CostConsultantor;
                entity.CostChecker = model.CostChecker;
                entity.BZDate = model.BZDate;
                entity.ReportDesc = model.ReportDesc;
                entity.Operator = model.Operator;
                entity.Salesman = model.Salesman;
                entity.Contacts = model.Contacts;
                entity.ContactsPhone = model.ContactsPhone;
                entity.ChargeAmount = model.ChargeAmount;
                entity.ChargeStatus = model.ChargeStatus;
                entity.IsInvoice = model.IsInvoice;
                entity.ReportType = model.ReportType.ToString();
                if (isSubmit)
                {
                    entity.ReportStatus = ReportStatus.WaitAudit.ToString();
                    entity.SubmitTime = DateTime.Now;
                    entity.AuditFileName = null;
                    entity.AuditFilePath = null;
                    List<Guid> msgUserList = dp.System_User.Where(m => model.AuditUserIds.Contains(m.Id) ||
                    dp.PM_Employee.Where(emp => model.AuditDep.Contains(emp.DepartmentId.Value)).Select(emp => emp.RelateUserId).Contains(m.Id)
                    ).Select(m => m.Id).ToList();
                    new Data.SystemData.SysMessageData().SendMessage(dp, new System_Message()
                    {
                        CreateTime = DateTime.Now,
                        CreateUser = Guid.Empty,
                        IsDel = false,
                        MsgTitle = "预算编制审批",
                        MsgType = SysMessageType.Audit.ToString(),
                        MsgContent = dp.System_User.Where(m => m.Id == model.CreateUser).Select(m => m.TrueName).FirstOrDefault() +
                                   $"于{DateTime.Now.ToString("yyyy-MM-dd") }向您提交一个报告（{entity.ReportName}），请尽快查阅。",
                        Url = "/CostConsultation/BudgetingAuditIndex"
                    }, msgUserList);
                }
                entity.UpdateTime = DateTime.Now;
                entity.UpdateUser = model.UpdateUser;

                //删除审核人员部门
                dp.RM_ReportAudit.RemoveRange(dp.RM_ReportAudit.Where(m => m.ReportId == entity.Id));
                //添加审核部门
                if (model.AuditDep.IsNotNullAndCountGtZero())
                {
                    model.AuditDep.ForEach(m => dp.RM_ReportAudit.Add(new RM_ReportAudit()
                    {
                        ReportId = entity.Id,
                        AuditId = m
                    }));
                }
                //添加审核人员
                if (model.AuditUserIds.IsNotNullAndCountGtZero())
                {
                    model.AuditUserIds.ForEach(m => dp.RM_ReportAudit.Add(new RM_ReportAudit()
                    {
                        ReportId = entity.Id,
                        AuditId = m
                    }));
                }
                try
                {
                    dp.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }


        public List<BudgetingModel> GetBudgetingList(BudgetingFilter filter, out int total, bool isPage = true)
        {
            using (DataProvider dp = new DataProvider())
            {
                string waitsubmit = ReportStatus.WaitSubmit.ToString();
                var list = from hr in dp.CC_Budgeting.Where(m => !m.IsDel && m.ReportStatus != waitsubmit)
                           join su1 in dp.System_User.Where(m => m.IsDel == false) on hr.CreateUser equals su1.Id
                           join su2 in dp.System_User.Where(m => m.IsDel == false) on hr.AuditUser equals su2.Id into su2c
                           from su2ci in su2c.DefaultIfEmpty()
                           join emp in dp.PM_Employee on hr.CreateUser equals emp.RelateUserId into empc
                           from empci in empc.DefaultIfEmpty()
                           join dep in dp.PM_Department on empci.DepartmentId equals dep.Id into depc
                           from depci in depc.DefaultIfEmpty()
                           select new BudgetingModel
                           {
                               Id = hr.Id,
                               ReportCode = hr.ReportCode,
                               ReportName = hr.ReportName,
                               EntrustingParty = hr.EntrustingParty,
                               BudgetAmount = hr.BudgetAmount,
                               CostConsultantor = hr.CostConsultantor,
                               CostChecker = hr.CostChecker,
                               BZDate = hr.BZDate,
                               ChargeAmount = hr.ChargeAmount,
                               ChargeStatus = hr.ChargeStatus,
                               SubmitTime = hr.SubmitTime,
                               CreateTime = hr.CreateTime,
                               CreateUser = hr.CreateUser,
                               CreateUserName = su1.TrueName,
                               AuditTime = hr.AuditTime,
                               AuditUser = hr.AuditUser,
                               AuditUserName = su2ci.TrueName,
                               AuditReason = hr.AuditReason,
                               CreateDepName = depci.DepName,
                               Salesman = hr.Salesman,
                               ReportStatus = hr.ReportStatus
                           };
                if (filter.AuditStatus.IsNotNullAndCountGtZero())
                {
                    list = list.Where(m => filter.AuditStatus.Contains(m.ReportStatus));
                }
                if (filter.ReportCode.IsNotNullOrWhiteSpace())
                {
                    list = list.Where(m => m.ReportCode.Contains(filter.ReportCode));
                }
                if (filter.ReportName.IsNotNullOrWhiteSpace())
                {
                    list = list.Where(m => m.ReportName.Contains(filter.ReportName));
                }
                if (filter.ChargeStatus.IsNotNullAndCountGtZero())
                {
                    list = list.Where(m => filter.ChargeStatus.Contains(m.ChargeStatus));
                }
                if (filter.CreateUserName.IsNotNullOrWhiteSpace())
                {
                    list = list.Where(m => m.CreateUserName.Contains(filter.CreateUserName));
                }
                if (filter.AuditUserName.IsNotNullOrWhiteSpace())
                {
                    list = list.Where(m => m.AuditUserName.Contains(filter.AuditUserName));
                }
                if (filter.CreateUserId.HasValue)
                {
                    list = list.Where(m => m.CreateUser == filter.CreateUserId.Value);
                }
                if (filter.AuditUserId.HasValue)
                {

                    list = list.Where(m => dp.RM_ReportAudit.Where(x => x.ReportId == m.Id)
                                                          .Select(x => x.AuditId)
                                                          .Contains(filter.AuditUserId.Value) ||
                                           dp.PM_Employee.Where(x => (dp.RM_ReportAudit.Where(y => y.ReportId == m.Id)
                                                                                     .Select(y => y.AuditId)).Contains(x.DepartmentId.Value))
                                                         .Select(x => x.RelateUserId)
                                                         .Contains(filter.AuditUserId.Value));
                }
                if (filter.CreateBeginTime.HasValue)
                {
                    list = list.Where(m => m.SubmitTime >= filter.CreateBeginTime.Value);
                }
                if (filter.CreateEndTime.HasValue)
                {
                    list = list.Where(m => m.SubmitTime <= filter.CreateEndTime.Value);
                }
                if (filter.AuditBeginTime.HasValue)
                {
                    list = list.Where(m => m.AuditTime >= filter.AuditBeginTime.Value);
                }
                if (filter.AuditEndTime.HasValue)
                {
                    list = list.Where(m => m.AuditTime <= filter.AuditEndTime.Value);
                }
                list = list.OrderBy(m => m.CreateTime);
                total = list.Count();
                if (isPage)
                {
                    return list.Skip(filter.Skip).Take(filter.PageSize).ToList();
                }
                else
                {
                    return list.ToList();
                }
            }
        }


        public bool AuditBudgeting(BudgetingModel model)
        {
            if (model == null || !model.Id.HasValue)
            {
                return false;
            }
            using (DataProvider dp = new DataProvider())
            {
                var entity = dp.CC_Budgeting.FirstOrDefault(m => m.Id == model.Id.Value);
                entity.ReportStatus = model.ReportStatus.ToString();
                entity.AuditReason = model.AuditReason;
                entity.AuditFileName = model.AuditFileName;
                entity.AuditFilePath = model.AuditFilePath;
                entity.AuditTime = DateTime.Now;
                entity.AuditUser = model.AuditUser;
                string auditTxt = model.ReportStatus == ReportStatus.Passed.ToString() ? "审核通过" : "被驳回";
                new Data.SystemData.SysMessageData().SendMessage(dp, new System_Message()
                {
                    CreateTime = DateTime.Now,
                    CreateUser = Guid.Empty,
                    IsDel = false,
                    MsgTitle = $"预算编制报告{auditTxt}",
                    MsgType = SysMessageType.Audit.ToString(),
                    MsgContent = $@"您的报告：{entity.ReportName}，{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}{auditTxt}",
                    Url = "/CostConsultation/BudgetingIndex"
                }, new List<Guid>() { entity.CreateUser });
                try
                {
                    dp.SaveChanges();
                    return true;
                }
                catch
                {
                    throw;
                }
            }
        }

        #endregion

        #region 概算编制
        public EstimateModel GetEstimateModel(Guid id)
        {
            using (DataProvider dp = new DataProvider())
            {
                EstimateModel model = Mapper.Map<EstimateModel>(dp.CC_Estimate.FirstOrDefault(m => m.Id == id));
                model.AuditDep = dp.PM_Department.Where(m => !m.IsDel &&
                                                       dp.RM_ReportAudit.Where(x => x.ReportId == model.Id).Select(x => x.AuditId).Contains(m.Id))
                                                     .Select(m => m.Id).ToList();
                model.AuditUserIds = dp.System_User.Where(m => m.IsDel == false &&
                                                         dp.RM_ReportAudit.Where(x => x.ReportId == model.Id).Select(x => x.AuditId).Contains(m.Id))
                                                      .Select(m => m.Id).ToList();
                model.AuditUserName = dp.System_User.Where(m => m.Id == model.AuditUser).Select(m => m.TrueName).FirstOrDefault();
                return model;
            }

        }

        public EstimateModel GetOrCreateEstimate(Guid createUserId)
        {
            Guid? modelId = null;
            using (DataProvider dp = new DataProvider())
            {
                string reportStatus = ReportStatus.WaitSubmit.ToString();
                if (dp.CC_Estimate.Count(m => m.CreateUser == createUserId && !m.IsDel && m.ReportStatus == reportStatus) > 0)
                {
                    modelId = dp.CC_Estimate.Where(m => m.CreateUser == createUserId && !m.IsDel && m.ReportStatus == reportStatus).Select(m => m.Id).FirstOrDefault();
                }
                else
                {
                    CC_Estimate model = new CC_Estimate()
                    {
                        Id = Guid.NewGuid(),
                        IsDel = false,
                        ChargeStatus = true,
                        IsInvoice = true,
                        ReportType = ReportType.Formal.ToString(),
                        ReportStatus = ReportStatus.WaitSubmit.ToString(),
                        CreateUser = createUserId,
                        CreateTime = DateTime.Now,
                    };
                    dp.CC_Estimate.Add(model);
                    dp.SaveChanges();
                    return Mapper.Map<EstimateModel>(model);
                }
            }
            return GetEstimateModel(modelId.Value);
        }

        public bool SaveEstimate(EstimateModel model, bool isSubmit = false)
        {
            using (DataProvider dp = new DataProvider())
            {
                CC_Estimate entity = dp.CC_Estimate.FirstOrDefault(m => m.Id == model.Id);
                entity.ReportCode = model.ReportCode;
                entity.ReportName = model.ReportName;
                entity.EntrustingParty = model.EntrustingParty;
                entity.EstimateAmount = model.EstimateAmount;
                entity.CostConsultantor = model.CostConsultantor;
                entity.CostChecker = model.CostChecker;
                entity.BZDate = model.BZDate;
                entity.ReportDesc = model.ReportDesc;
                entity.Operator = model.Operator;
                entity.Salesman = model.Salesman;
                entity.Contacts = model.Contacts;
                entity.ContactsPhone = model.ContactsPhone;
                entity.ChargeAmount = model.ChargeAmount;
                entity.ChargeStatus = model.ChargeStatus;
                entity.IsInvoice = model.IsInvoice;
                entity.ReportType = model.ReportType.ToString();
                if (isSubmit)
                {
                    entity.ReportStatus = ReportStatus.WaitAudit.ToString();
                    entity.SubmitTime = DateTime.Now;
                    entity.AuditFileName = null;
                    entity.AuditFilePath = null;
                    List<Guid> msgUserList = dp.System_User.Where(m => model.AuditUserIds.Contains(m.Id) ||
                    dp.PM_Employee.Where(emp => model.AuditDep.Contains(emp.DepartmentId.Value)).Select(emp => emp.RelateUserId).Contains(m.Id)
                    ).Select(m => m.Id).ToList();
                    new Data.SystemData.SysMessageData().SendMessage(dp, new System_Message()
                    {
                        CreateTime = DateTime.Now,
                        CreateUser = Guid.Empty,
                        IsDel = false,
                        MsgTitle = "概算编制审批",
                        MsgType = SysMessageType.Audit.ToString(),
                        MsgContent = dp.System_User.Where(m => m.Id == model.CreateUser).Select(m => m.TrueName).FirstOrDefault() +
                                   $"于{DateTime.Now.ToString("yyyy-MM-dd") }向您提交一个报告（{entity.ReportName}），请尽快查阅。",
                        Url = "/CostConsultation/EstimateAuditIndex"
                    }, msgUserList);
                }
                entity.UpdateTime = DateTime.Now;
                entity.UpdateUser = model.UpdateUser;

                //删除审核人员部门
                dp.RM_ReportAudit.RemoveRange(dp.RM_ReportAudit.Where(m => m.ReportId == entity.Id));
                //添加审核部门
                if (model.AuditDep.IsNotNullAndCountGtZero())
                {
                    model.AuditDep.ForEach(m => dp.RM_ReportAudit.Add(new RM_ReportAudit()
                    {
                        ReportId = entity.Id,
                        AuditId = m
                    }));
                }
                //添加审核人员
                if (model.AuditUserIds.IsNotNullAndCountGtZero())
                {
                    model.AuditUserIds.ForEach(m => dp.RM_ReportAudit.Add(new RM_ReportAudit()
                    {
                        ReportId = entity.Id,
                        AuditId = m
                    }));
                }
                try
                {
                    dp.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }


        public List<EstimateModel> GetEstimateList(EstimateFilter filter, out int total, bool isPage = true)
        {
            using (DataProvider dp = new DataProvider())
            {
                string waitsubmit = ReportStatus.WaitSubmit.ToString();
                var list = from hr in dp.CC_Estimate.Where(m => !m.IsDel && m.ReportStatus != waitsubmit)
                           join su1 in dp.System_User.Where(m => m.IsDel == false) on hr.CreateUser equals su1.Id
                           join su2 in dp.System_User.Where(m => m.IsDel == false) on hr.AuditUser equals su2.Id into su2c
                           from su2ci in su2c.DefaultIfEmpty()
                           join emp in dp.PM_Employee on hr.CreateUser equals emp.RelateUserId into empc
                           from empci in empc.DefaultIfEmpty()
                           join dep in dp.PM_Department on empci.DepartmentId equals dep.Id into depc
                           from depci in depc.DefaultIfEmpty()
                           select new EstimateModel
                           {
                               Id = hr.Id,
                               ReportCode = hr.ReportCode,
                               ReportName = hr.ReportName,
                               EntrustingParty = hr.EntrustingParty,
                               EstimateAmount = hr.EstimateAmount,
                               CostConsultantor = hr.CostConsultantor,
                               CostChecker = hr.CostChecker,
                               BZDate = hr.BZDate,
                               ChargeAmount = hr.ChargeAmount,
                               ChargeStatus = hr.ChargeStatus,
                               SubmitTime = hr.SubmitTime,
                               CreateTime = hr.CreateTime,
                               CreateUser = hr.CreateUser,
                               CreateUserName = su1.TrueName,
                               AuditTime = hr.AuditTime,
                               AuditUser = hr.AuditUser,
                               AuditUserName = su2ci.TrueName,
                               AuditReason = hr.AuditReason,
                               CreateDepName = depci.DepName,
                               Salesman = hr.Salesman,
                               ReportStatus = hr.ReportStatus
                           };
                if (filter.AuditStatus.IsNotNullAndCountGtZero())
                {
                    list = list.Where(m => filter.AuditStatus.Contains(m.ReportStatus));
                }
                if (filter.ReportCode.IsNotNullOrWhiteSpace())
                {
                    list = list.Where(m => m.ReportCode.Contains(filter.ReportCode));
                }
                if (filter.ReportName.IsNotNullOrWhiteSpace())
                {
                    list = list.Where(m => m.ReportName.Contains(filter.ReportName));
                }
                if (filter.ChargeStatus.IsNotNullAndCountGtZero())
                {
                    list = list.Where(m => filter.ChargeStatus.Contains(m.ChargeStatus));
                }
                if (filter.CreateUserName.IsNotNullOrWhiteSpace())
                {
                    list = list.Where(m => m.CreateUserName.Contains(filter.CreateUserName));
                }
                if (filter.AuditUserName.IsNotNullOrWhiteSpace())
                {
                    list = list.Where(m => m.AuditUserName.Contains(filter.AuditUserName));
                }
                if (filter.CreateUserId.HasValue)
                {
                    list = list.Where(m => m.CreateUser == filter.CreateUserId.Value);
                }
                if (filter.AuditUserId.HasValue)
                {

                    list = list.Where(m => dp.RM_ReportAudit.Where(x => x.ReportId == m.Id)
                                                          .Select(x => x.AuditId)
                                                          .Contains(filter.AuditUserId.Value) ||
                                           dp.PM_Employee.Where(x => (dp.RM_ReportAudit.Where(y => y.ReportId == m.Id)
                                                                                     .Select(y => y.AuditId)).Contains(x.DepartmentId.Value))
                                                         .Select(x => x.RelateUserId)
                                                         .Contains(filter.AuditUserId.Value));
                }
                if (filter.CreateBeginTime.HasValue)
                {
                    list = list.Where(m => m.SubmitTime >= filter.CreateBeginTime.Value);
                }
                if (filter.CreateEndTime.HasValue)
                {
                    list = list.Where(m => m.SubmitTime <= filter.CreateEndTime.Value);
                }
                if (filter.AuditBeginTime.HasValue)
                {
                    list = list.Where(m => m.AuditTime >= filter.AuditBeginTime.Value);
                }
                if (filter.AuditEndTime.HasValue)
                {
                    list = list.Where(m => m.AuditTime <= filter.AuditEndTime.Value);
                }
                list = list.OrderBy(m => m.CreateTime);
                total = list.Count();
                if (isPage)
                {
                    return list.Skip(filter.Skip).Take(filter.PageSize).ToList();
                }
                else
                {
                    return list.ToList();
                }
            }
        }


        public bool AuditEstimate(EstimateModel model)
        {
            if (model == null || !model.Id.HasValue)
            {
                return false;
            }
            using (DataProvider dp = new DataProvider())
            {
                var entity = dp.CC_Estimate.FirstOrDefault(m => m.Id == model.Id.Value);
                entity.ReportStatus = model.ReportStatus.ToString();
                entity.AuditReason = model.AuditReason;
                entity.AuditFileName = model.AuditFileName;
                entity.AuditFilePath = model.AuditFilePath;
                entity.AuditTime = DateTime.Now;
                entity.AuditUser = model.AuditUser;
                string auditTxt = model.ReportStatus == ReportStatus.Passed.ToString() ? "审核通过" : "被驳回";
                new Data.SystemData.SysMessageData().SendMessage(dp, new System_Message()
                {
                    CreateTime = DateTime.Now,
                    CreateUser = Guid.Empty,
                    IsDel = false,
                    MsgTitle = $"概算编制报告{auditTxt}",
                    MsgType = SysMessageType.Audit.ToString(),
                    MsgContent = $@"您的报告：{entity.ReportName}，{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}{auditTxt}",
                    Url = "/CostConsultation/EstimateIndex"
                }, new List<Guid>() { entity.CreateUser });
                try
                {
                    dp.SaveChanges();
                    return true;
                }
                catch
                {
                    throw;
                }
            }
        }

        #endregion


        #region 结算审计
        public SettlementModel GetSettlementModel(Guid id)
        {
            using (DataProvider dp = new DataProvider())
            {
                SettlementModel model = Mapper.Map<SettlementModel>(dp.CC_Settlement.FirstOrDefault(m => m.Id == id));
                model.AuditDep = dp.PM_Department.Where(m => !m.IsDel &&
                                                       dp.RM_ReportAudit.Where(x => x.ReportId == model.Id).Select(x => x.AuditId).Contains(m.Id))
                                                     .Select(m => m.Id).ToList();
                model.AuditUserIds = dp.System_User.Where(m => m.IsDel == false &&
                                                         dp.RM_ReportAudit.Where(x => x.ReportId == model.Id).Select(x => x.AuditId).Contains(m.Id))
                                                      .Select(m => m.Id).ToList();
                model.AuditUserName = dp.System_User.Where(m => m.Id == model.AuditUser).Select(m => m.TrueName).FirstOrDefault();
                return model;
            }

        }

        public SettlementModel GetOrCreateSettlement(Guid createUserId)
        {
            Guid? modelId = null;
            using (DataProvider dp = new DataProvider())
            {
                string reportStatus = ReportStatus.WaitSubmit.ToString();
                if (dp.CC_Settlement.Count(m => m.CreateUser == createUserId && !m.IsDel && m.ReportStatus == reportStatus) > 0)
                {
                    modelId = dp.CC_Settlement.Where(m => m.CreateUser == createUserId && !m.IsDel && m.ReportStatus == reportStatus).Select(m => m.Id).FirstOrDefault();
                }
                else
                {
                    CC_Settlement model = new CC_Settlement()
                    {
                        Id = Guid.NewGuid(),
                        IsDel = false,
                        ChargeStatus = true,
                        IsInvoice = true,
                        ReportType = ReportType.Formal.ToString(),
                        ReportStatus = ReportStatus.WaitSubmit.ToString(),
                        CreateUser = createUserId,
                        CreateTime = DateTime.Now,
                    };
                    dp.CC_Settlement.Add(model);
                    dp.SaveChanges();
                    return Mapper.Map<SettlementModel>(model);
                }
            }
            return GetSettlementModel(modelId.Value);
        }

        public bool SaveSettlement(SettlementModel model, bool isSubmit = false)
        {
            using (DataProvider dp = new DataProvider())
            {
                CC_Settlement entity = dp.CC_Settlement.FirstOrDefault(m => m.Id == model.Id);
                entity.ReportCode = model.ReportCode;
                entity.ReportName = model.ReportName;
                entity.EntrustingParty = model.EntrustingParty;
                entity.ReportAmount = model.ReportAmount;
                entity.ApprovalAmount = model.ApprovalAmount;
                entity.ReductionAmount = model.ReductionAmount;
                entity.CostConsultantor = model.CostConsultantor;
                entity.CostChecker = model.CostChecker;
                entity.BZDate = model.BZDate;
                entity.ReportDesc = model.ReportDesc;
                entity.Operator = model.Operator;
                entity.Salesman = model.Salesman;
                entity.Contacts = model.Contacts;
                entity.ContactsPhone = model.ContactsPhone;
                entity.ChargeAmount = model.ChargeAmount;
                entity.ChargeStatus = model.ChargeStatus;
                entity.IsInvoice = model.IsInvoice;
                entity.ReportType = model.ReportType.ToString();
                if (isSubmit)
                {
                    entity.ReportStatus = ReportStatus.WaitAudit.ToString();
                    entity.SubmitTime = DateTime.Now;
                    entity.AuditFileName = null;
                    entity.AuditFilePath = null;
                    List<Guid> msgUserList = dp.System_User.Where(m => model.AuditUserIds.Contains(m.Id) ||
                    dp.PM_Employee.Where(emp => model.AuditDep.Contains(emp.DepartmentId.Value)).Select(emp => emp.RelateUserId).Contains(m.Id)
                    ).Select(m => m.Id).ToList();
                    new Data.SystemData.SysMessageData().SendMessage(dp, new System_Message()
                    {
                        CreateTime = DateTime.Now,
                        CreateUser = Guid.Empty,
                        IsDel = false,
                        MsgTitle = "结算审计审批",
                        MsgType = SysMessageType.Audit.ToString(),
                        MsgContent = dp.System_User.Where(m => m.Id == model.CreateUser).Select(m => m.TrueName).FirstOrDefault() +
                                   $"于{DateTime.Now.ToString("yyyy-MM-dd") }向您提交一个报告（{entity.ReportName}），请尽快查阅。",
                        Url = "/CostConsultation/SettlementAuditIndex"
                    }, msgUserList);
                }
                entity.UpdateTime = DateTime.Now;
                entity.UpdateUser = model.UpdateUser;

                //删除审核人员部门
                dp.RM_ReportAudit.RemoveRange(dp.RM_ReportAudit.Where(m => m.ReportId == entity.Id));
                //添加审核部门
                if (model.AuditDep.IsNotNullAndCountGtZero())
                {
                    model.AuditDep.ForEach(m => dp.RM_ReportAudit.Add(new RM_ReportAudit()
                    {
                        ReportId = entity.Id,
                        AuditId = m
                    }));
                }
                //添加审核人员
                if (model.AuditUserIds.IsNotNullAndCountGtZero())
                {
                    model.AuditUserIds.ForEach(m => dp.RM_ReportAudit.Add(new RM_ReportAudit()
                    {
                        ReportId = entity.Id,
                        AuditId = m
                    }));
                }
                try
                {
                    dp.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }


        public List<SettlementModel> GetSettlementList(SettlementFilter filter, out int total, bool isPage = true)
        {
            using (DataProvider dp = new DataProvider())
            {
                string waitsubmit = ReportStatus.WaitSubmit.ToString();
                var list = from hr in dp.CC_Settlement.Where(m => !m.IsDel && m.ReportStatus != waitsubmit)
                           join su1 in dp.System_User.Where(m => m.IsDel == false) on hr.CreateUser equals su1.Id
                           join su2 in dp.System_User.Where(m => m.IsDel == false) on hr.AuditUser equals su2.Id into su2c
                           from su2ci in su2c.DefaultIfEmpty()
                           join emp in dp.PM_Employee on hr.CreateUser equals emp.RelateUserId into empc
                           from empci in empc.DefaultIfEmpty()
                           join dep in dp.PM_Department on empci.DepartmentId equals dep.Id into depc
                           from depci in depc.DefaultIfEmpty()
                           select new SettlementModel
                           {
                               Id = hr.Id,
                               ReportCode = hr.ReportCode,
                               ReportName = hr.ReportName,
                               EntrustingParty = hr.EntrustingParty,
                               ReportAmount = hr.ReportAmount,
                               ApprovalAmount = hr.ApprovalAmount,
                               ReductionAmount = hr.ReductionAmount,
                               CostConsultantor = hr.CostConsultantor,
                               CostChecker = hr.CostChecker,
                               BZDate = hr.BZDate,
                               ChargeAmount = hr.ChargeAmount,
                               ChargeStatus = hr.ChargeStatus,
                               SubmitTime = hr.SubmitTime,
                               CreateTime = hr.CreateTime,
                               CreateUser = hr.CreateUser,
                               CreateUserName = su1.TrueName,
                               AuditTime = hr.AuditTime,
                               AuditUser = hr.AuditUser,
                               AuditUserName = su2ci.TrueName,
                               AuditReason = hr.AuditReason,
                               CreateDepName = depci.DepName,
                               Salesman = hr.Salesman,
                               ReportStatus = hr.ReportStatus
                           };
                if (filter.AuditStatus.IsNotNullAndCountGtZero())
                {
                    list = list.Where(m => filter.AuditStatus.Contains(m.ReportStatus));
                }
                if (filter.ReportCode.IsNotNullOrWhiteSpace())
                {
                    list = list.Where(m => m.ReportCode.Contains(filter.ReportCode));
                }
                if (filter.ReportName.IsNotNullOrWhiteSpace())
                {
                    list = list.Where(m => m.ReportName.Contains(filter.ReportName));
                }
                if (filter.ChargeStatus.IsNotNullAndCountGtZero())
                {
                    list = list.Where(m => filter.ChargeStatus.Contains(m.ChargeStatus));
                }
                if (filter.CreateUserName.IsNotNullOrWhiteSpace())
                {
                    list = list.Where(m => m.CreateUserName.Contains(filter.CreateUserName));
                }
                if (filter.AuditUserName.IsNotNullOrWhiteSpace())
                {
                    list = list.Where(m => m.AuditUserName.Contains(filter.AuditUserName));
                }
                if (filter.CreateUserId.HasValue)
                {
                    list = list.Where(m => m.CreateUser == filter.CreateUserId.Value);
                }
                if (filter.AuditUserId.HasValue)
                {

                    list = list.Where(m => dp.RM_ReportAudit.Where(x => x.ReportId == m.Id)
                                                          .Select(x => x.AuditId)
                                                          .Contains(filter.AuditUserId.Value) ||
                                           dp.PM_Employee.Where(x => (dp.RM_ReportAudit.Where(y => y.ReportId == m.Id)
                                                                                     .Select(y => y.AuditId)).Contains(x.DepartmentId.Value))
                                                         .Select(x => x.RelateUserId)
                                                         .Contains(filter.AuditUserId.Value));
                }
                if (filter.CreateBeginTime.HasValue)
                {
                    list = list.Where(m => m.SubmitTime >= filter.CreateBeginTime.Value);
                }
                if (filter.CreateEndTime.HasValue)
                {
                    list = list.Where(m => m.SubmitTime <= filter.CreateEndTime.Value);
                }
                if (filter.AuditBeginTime.HasValue)
                {
                    list = list.Where(m => m.AuditTime >= filter.AuditBeginTime.Value);
                }
                if (filter.AuditEndTime.HasValue)
                {
                    list = list.Where(m => m.AuditTime <= filter.AuditEndTime.Value);
                }
                list = list.OrderBy(m => m.CreateTime);
                total = list.Count();
                if (isPage)
                {
                    return list.Skip(filter.Skip).Take(filter.PageSize).ToList();
                }
                else
                {
                    return list.ToList();
                }
            }
        }


        public bool AuditSettlement(SettlementModel model)
        {
            if (model == null || !model.Id.HasValue)
            {
                return false;
            }
            using (DataProvider dp = new DataProvider())
            {
                var entity = dp.CC_Settlement.FirstOrDefault(m => m.Id == model.Id.Value);
                entity.ReportStatus = model.ReportStatus.ToString();
                entity.AuditReason = model.AuditReason;
                entity.AuditFileName = model.AuditFileName;
                entity.AuditFilePath = model.AuditFilePath;
                entity.AuditTime = DateTime.Now;
                entity.AuditUser = model.AuditUser;
                string auditTxt = model.ReportStatus == ReportStatus.Passed.ToString() ? "审核通过" : "被驳回";
                new Data.SystemData.SysMessageData().SendMessage(dp, new System_Message()
                {
                    CreateTime = DateTime.Now,
                    CreateUser = Guid.Empty,
                    IsDel = false,
                    MsgTitle = $"结算审计报告{auditTxt}",
                    MsgType = SysMessageType.Audit.ToString(),
                    MsgContent = $@"您的报告：{entity.ReportName}，{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}{auditTxt}",
                    Url = "/CostConsultation/SettlementIndex"
                }, new List<Guid>() { entity.CreateUser });
                try
                {
                    dp.SaveChanges();
                    return true;
                }
                catch
                {
                    throw;
                }
            }
        }

        #endregion

        #region 投资评审
        public InvestmentModel GetInvestmentModel(Guid id)
        {
            using (DataProvider dp = new DataProvider())
            {
                InvestmentModel model = Mapper.Map<InvestmentModel>(dp.CC_Investment.FirstOrDefault(m => m.Id == id));
                model.AuditDep = dp.PM_Department.Where(m => !m.IsDel &&
                                                       dp.RM_ReportAudit.Where(x => x.ReportId == model.Id).Select(x => x.AuditId).Contains(m.Id))
                                                     .Select(m => m.Id).ToList();
                model.AuditUserIds = dp.System_User.Where(m => m.IsDel == false &&
                                                         dp.RM_ReportAudit.Where(x => x.ReportId == model.Id).Select(x => x.AuditId).Contains(m.Id))
                                                      .Select(m => m.Id).ToList();
                model.AuditUserName = dp.System_User.Where(m => m.Id == model.AuditUser).Select(m => m.TrueName).FirstOrDefault();
                return model;
            }

        }

        public InvestmentModel GetOrCreateInvestment(Guid createUserId)
        {
            Guid? modelId = null;
            using (DataProvider dp = new DataProvider())
            {
                string reportStatus = ReportStatus.WaitSubmit.ToString();
                if (dp.CC_Investment.Count(m => m.CreateUser == createUserId && !m.IsDel && m.ReportStatus == reportStatus) > 0)
                {
                    modelId = dp.CC_Investment.Where(m => m.CreateUser == createUserId && !m.IsDel && m.ReportStatus == reportStatus).Select(m => m.Id).FirstOrDefault();
                }
                else
                {
                    CC_Investment model = new CC_Investment()
                    {
                        Id = Guid.NewGuid(),
                        IsDel = false,
                        ChargeStatus = true,
                        IsInvoice = true,
                        ReportType = ReportType.Formal.ToString(),
                        ReportStatus = ReportStatus.WaitSubmit.ToString(),
                        CreateUser = createUserId,
                        CreateTime = DateTime.Now,
                    };
                    dp.CC_Investment.Add(model);
                    dp.SaveChanges();
                    return Mapper.Map<InvestmentModel>(model);
                }
            }
            return GetInvestmentModel(modelId.Value);
        }

        public bool SaveInvestment(InvestmentModel model, bool isSubmit = false)
        {
            using (DataProvider dp = new DataProvider())
            {
                CC_Investment entity = dp.CC_Investment.FirstOrDefault(m => m.Id == model.Id);
                entity.ReportCode = model.ReportCode;
                entity.ReportName = model.ReportName;
                entity.EntrustingParty = model.EntrustingParty;
                entity.ReportAmount = model.ReportAmount;
                entity.ApprovalAmount = model.ApprovalAmount;
                entity.ReductionAmount = model.ReductionAmount;
                entity.CostConsultantor = model.CostConsultantor;
                entity.CostChecker = model.CostChecker;
                entity.BZDate = model.BZDate;
                entity.ReportDesc = model.ReportDesc;
                entity.Operator = model.Operator;
                entity.Salesman = model.Salesman;
                entity.Contacts = model.Contacts;
                entity.ContactsPhone = model.ContactsPhone;
                entity.ChargeAmount = model.ChargeAmount;
                entity.ChargeStatus = model.ChargeStatus;
                entity.IsInvoice = model.IsInvoice;
                entity.ReportType = model.ReportType.ToString();
                if (isSubmit)
                {
                    entity.ReportStatus = ReportStatus.WaitAudit.ToString();
                    entity.SubmitTime = DateTime.Now;
                    entity.AuditFileName = null;
                    entity.AuditFilePath = null;
                    List<Guid> msgUserList = dp.System_User.Where(m => model.AuditUserIds.Contains(m.Id) ||
                    dp.PM_Employee.Where(emp => model.AuditDep.Contains(emp.DepartmentId.Value)).Select(emp => emp.RelateUserId).Contains(m.Id)
                    ).Select(m => m.Id).ToList();
                    new Data.SystemData.SysMessageData().SendMessage(dp, new System_Message()
                    {
                        CreateTime = DateTime.Now,
                        CreateUser = Guid.Empty,
                        IsDel = false,
                        MsgTitle = "投资评审审批",
                        MsgType = SysMessageType.Audit.ToString(),
                        MsgContent = dp.System_User.Where(m => m.Id == model.CreateUser).Select(m => m.TrueName).FirstOrDefault() +
                                   $"于{DateTime.Now.ToString("yyyy-MM-dd") }向您提交一个报告（{entity.ReportName}），请尽快查阅。",
                        Url = "/CostConsultation/InvestmentAuditIndex"
                    }, msgUserList);
                }
                entity.UpdateTime = DateTime.Now;
                entity.UpdateUser = model.UpdateUser;

                //删除审核人员部门
                dp.RM_ReportAudit.RemoveRange(dp.RM_ReportAudit.Where(m => m.ReportId == entity.Id));
                //添加审核部门
                if (model.AuditDep.IsNotNullAndCountGtZero())
                {
                    model.AuditDep.ForEach(m => dp.RM_ReportAudit.Add(new RM_ReportAudit()
                    {
                        ReportId = entity.Id,
                        AuditId = m
                    }));
                }
                //添加审核人员
                if (model.AuditUserIds.IsNotNullAndCountGtZero())
                {
                    model.AuditUserIds.ForEach(m => dp.RM_ReportAudit.Add(new RM_ReportAudit()
                    {
                        ReportId = entity.Id,
                        AuditId = m
                    }));
                }
                try
                {
                    dp.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }


        public List<InvestmentModel> GetInvestmentList(InvestmentFilter filter, out int total, bool isPage = true)
        {
            using (DataProvider dp = new DataProvider())
            {
                string waitsubmit = ReportStatus.WaitSubmit.ToString();
                var list = from hr in dp.CC_Investment.Where(m => !m.IsDel && m.ReportStatus != waitsubmit)
                           join su1 in dp.System_User.Where(m => m.IsDel == false) on hr.CreateUser equals su1.Id
                           join su2 in dp.System_User.Where(m => m.IsDel == false) on hr.AuditUser equals su2.Id into su2c
                           from su2ci in su2c.DefaultIfEmpty()
                           join emp in dp.PM_Employee on hr.CreateUser equals emp.RelateUserId into empc
                           from empci in empc.DefaultIfEmpty()
                           join dep in dp.PM_Department on empci.DepartmentId equals dep.Id into depc
                           from depci in depc.DefaultIfEmpty()
                           select new InvestmentModel
                           {
                               Id = hr.Id,
                               ReportCode = hr.ReportCode,
                               ReportName = hr.ReportName,
                               EntrustingParty = hr.EntrustingParty,
                               ReportAmount = hr.ReportAmount,
                               ApprovalAmount = hr.ApprovalAmount,
                               ReductionAmount = hr.ReductionAmount,
                               CostConsultantor = hr.CostConsultantor,
                               CostChecker = hr.CostChecker,
                               BZDate = hr.BZDate,
                               ChargeAmount = hr.ChargeAmount,
                               ChargeStatus = hr.ChargeStatus,
                               SubmitTime = hr.SubmitTime,
                               CreateTime = hr.CreateTime,
                               CreateUser = hr.CreateUser,
                               CreateUserName = su1.TrueName,
                               AuditTime = hr.AuditTime,
                               AuditUser = hr.AuditUser,
                               AuditUserName = su2ci.TrueName,
                               AuditReason = hr.AuditReason,
                               CreateDepName = depci.DepName,
                               Salesman = hr.Salesman,
                               ReportStatus = hr.ReportStatus
                           };
                if (filter.AuditStatus.IsNotNullAndCountGtZero())
                {
                    list = list.Where(m => filter.AuditStatus.Contains(m.ReportStatus));
                }
                if (filter.ReportCode.IsNotNullOrWhiteSpace())
                {
                    list = list.Where(m => m.ReportCode.Contains(filter.ReportCode));
                }
                if (filter.ReportName.IsNotNullOrWhiteSpace())
                {
                    list = list.Where(m => m.ReportName.Contains(filter.ReportName));
                }
                if (filter.ChargeStatus.IsNotNullAndCountGtZero())
                {
                    list = list.Where(m => filter.ChargeStatus.Contains(m.ChargeStatus));
                }
                if (filter.CreateUserName.IsNotNullOrWhiteSpace())
                {
                    list = list.Where(m => m.CreateUserName.Contains(filter.CreateUserName));
                }
                if (filter.AuditUserName.IsNotNullOrWhiteSpace())
                {
                    list = list.Where(m => m.AuditUserName.Contains(filter.AuditUserName));
                }
                if (filter.CreateUserId.HasValue)
                {
                    list = list.Where(m => m.CreateUser == filter.CreateUserId.Value);
                }
                if (filter.AuditUserId.HasValue)
                {

                    list = list.Where(m => dp.RM_ReportAudit.Where(x => x.ReportId == m.Id)
                                                          .Select(x => x.AuditId)
                                                          .Contains(filter.AuditUserId.Value) ||
                                           dp.PM_Employee.Where(x => (dp.RM_ReportAudit.Where(y => y.ReportId == m.Id)
                                                                                     .Select(y => y.AuditId)).Contains(x.DepartmentId.Value))
                                                         .Select(x => x.RelateUserId)
                                                         .Contains(filter.AuditUserId.Value));
                }
                if (filter.CreateBeginTime.HasValue)
                {
                    list = list.Where(m => m.SubmitTime >= filter.CreateBeginTime.Value);
                }
                if (filter.CreateEndTime.HasValue)
                {
                    list = list.Where(m => m.SubmitTime <= filter.CreateEndTime.Value);
                }
                if (filter.AuditBeginTime.HasValue)
                {
                    list = list.Where(m => m.AuditTime >= filter.AuditBeginTime.Value);
                }
                if (filter.AuditEndTime.HasValue)
                {
                    list = list.Where(m => m.AuditTime <= filter.AuditEndTime.Value);
                }
                list = list.OrderBy(m => m.CreateTime);
                total = list.Count();
                if (isPage)
                {
                    return list.Skip(filter.Skip).Take(filter.PageSize).ToList();
                }
                else
                {
                    return list.ToList();
                }
            }
        }


        public bool AuditInvestment(InvestmentModel model)
        {
            if (model == null || !model.Id.HasValue)
            {
                return false;
            }
            using (DataProvider dp = new DataProvider())
            {
                var entity = dp.CC_Investment.FirstOrDefault(m => m.Id == model.Id.Value);
                entity.ReportStatus = model.ReportStatus.ToString();
                entity.AuditReason = model.AuditReason;
                entity.AuditFileName = model.AuditFileName;
                entity.AuditFilePath = model.AuditFilePath;
                entity.AuditTime = DateTime.Now;
                entity.AuditUser = model.AuditUser;
                string auditTxt = model.ReportStatus == ReportStatus.Passed.ToString() ? "审核通过" : "被驳回";
                new Data.SystemData.SysMessageData().SendMessage(dp, new System_Message()
                {
                    CreateTime = DateTime.Now,
                    CreateUser = Guid.Empty,
                    IsDel = false,
                    MsgTitle = $"投资评审报告{auditTxt}",
                    MsgType = SysMessageType.Audit.ToString(),
                    MsgContent = $@"您的报告：{entity.ReportName}，{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}{auditTxt}",
                    Url = "/CostConsultation/InvestmentIndex"
                }, new List<Guid>() { entity.CreateUser });
                try
                {
                    dp.SaveChanges();
                    return true;
                }
                catch
                {
                    throw;
                }
            }
        }

        #endregion


        #region 司法鉴定
        public AppraisalModel GetAppraisalModel(Guid id)
        {
            using (DataProvider dp = new DataProvider())
            {
                AppraisalModel model = Mapper.Map<AppraisalModel>(dp.CC_Appraisal.FirstOrDefault(m => m.Id == id));
                model.AuditDep = dp.PM_Department.Where(m => !m.IsDel &&
                                                       dp.RM_ReportAudit.Where(x => x.ReportId == model.Id).Select(x => x.AuditId).Contains(m.Id))
                                                     .Select(m => m.Id).ToList();
                model.AuditUserIds = dp.System_User.Where(m => m.IsDel == false &&
                                                         dp.RM_ReportAudit.Where(x => x.ReportId == model.Id).Select(x => x.AuditId).Contains(m.Id))
                                                      .Select(m => m.Id).ToList();
                model.AuditUserName = dp.System_User.Where(m => m.Id == model.AuditUser).Select(m => m.TrueName).FirstOrDefault();
                return model;
            }

        }

        public AppraisalModel GetOrCreateAppraisal(Guid createUserId)
        {
            Guid? modelId = null;
            using (DataProvider dp = new DataProvider())
            {
                string reportStatus = ReportStatus.WaitSubmit.ToString();
                if (dp.CC_Appraisal.Count(m => m.CreateUser == createUserId && !m.IsDel && m.ReportStatus == reportStatus) > 0)
                {
                    modelId = dp.CC_Appraisal.Where(m => m.CreateUser == createUserId && !m.IsDel && m.ReportStatus == reportStatus).Select(m => m.Id).FirstOrDefault();
                }
                else
                {
                    CC_Appraisal model = new CC_Appraisal()
                    {
                        Id = Guid.NewGuid(),
                        IsDel = false,
                        ChargeStatus = true,
                        IsInvoice = true,
                        ReportType = ReportType.Formal.ToString(),
                        ReportStatus = ReportStatus.WaitSubmit.ToString(),
                        CreateUser = createUserId,
                        CreateTime = DateTime.Now,
                    };
                    dp.CC_Appraisal.Add(model);
                    dp.SaveChanges();
                    return Mapper.Map<AppraisalModel>(model);
                }
            }
            return GetAppraisalModel(modelId.Value);
        }

        public bool SaveAppraisal(AppraisalModel model, bool isSubmit = false)
        {
            using (DataProvider dp = new DataProvider())
            {
                CC_Appraisal entity = dp.CC_Appraisal.FirstOrDefault(m => m.Id == model.Id);
                entity.ReportCode = model.ReportCode;
                entity.ReportName = model.ReportName;
                entity.EntrustingParty = model.EntrustingParty;
                entity.AppraisalAmount = model.AppraisalAmount;
                entity.CostConsultantor = model.CostConsultantor;
                entity.CostChecker = model.CostChecker;
                entity.BZDate = model.BZDate;
                entity.ReportDesc = model.ReportDesc;
                entity.Operator = model.Operator;
                entity.Salesman = model.Salesman;
                entity.Contacts = model.Contacts;
                entity.ContactsPhone = model.ContactsPhone;
                entity.ChargeAmount = model.ChargeAmount;
                entity.ChargeStatus = model.ChargeStatus;
                entity.IsInvoice = model.IsInvoice;
                entity.ReportType = model.ReportType.ToString();
                if (isSubmit)
                {
                    entity.ReportStatus = ReportStatus.WaitAudit.ToString();
                    entity.SubmitTime = DateTime.Now;
                    entity.AuditFileName = null;
                    entity.AuditFilePath = null;
                    List<Guid> msgUserList = dp.System_User.Where(m => model.AuditUserIds.Contains(m.Id) ||
                    dp.PM_Employee.Where(emp => model.AuditDep.Contains(emp.DepartmentId.Value)).Select(emp => emp.RelateUserId).Contains(m.Id)
                    ).Select(m => m.Id).ToList();
                    new Data.SystemData.SysMessageData().SendMessage(dp, new System_Message()
                    {
                        CreateTime = DateTime.Now,
                        CreateUser = Guid.Empty,
                        IsDel = false,
                        MsgTitle = "司法鉴定审批",
                        MsgType = SysMessageType.Audit.ToString(),
                        MsgContent = dp.System_User.Where(m => m.Id == model.CreateUser).Select(m => m.TrueName).FirstOrDefault() +
                                   $"于{DateTime.Now.ToString("yyyy-MM-dd") }向您提交一个报告（{entity.ReportName}），请尽快查阅。",
                        Url = "/CostConsultation/AppraisalAuditIndex"
                    }, msgUserList);
                }
                entity.UpdateTime = DateTime.Now;
                entity.UpdateUser = model.UpdateUser;

                //删除审核人员部门
                dp.RM_ReportAudit.RemoveRange(dp.RM_ReportAudit.Where(m => m.ReportId == entity.Id));
                //添加审核部门
                if (model.AuditDep.IsNotNullAndCountGtZero())
                {
                    model.AuditDep.ForEach(m => dp.RM_ReportAudit.Add(new RM_ReportAudit()
                    {
                        ReportId = entity.Id,
                        AuditId = m
                    }));
                }
                //添加审核人员
                if (model.AuditUserIds.IsNotNullAndCountGtZero())
                {
                    model.AuditUserIds.ForEach(m => dp.RM_ReportAudit.Add(new RM_ReportAudit()
                    {
                        ReportId = entity.Id,
                        AuditId = m
                    }));
                }
                try
                {
                    dp.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }


        public List<AppraisalModel> GetAppraisalList(AppraisalFilter filter, out int total, bool isPage = true)
        {
            using (DataProvider dp = new DataProvider())
            {
                string waitsubmit = ReportStatus.WaitSubmit.ToString();
                var list = from hr in dp.CC_Appraisal.Where(m => !m.IsDel && m.ReportStatus != waitsubmit)
                           join su1 in dp.System_User.Where(m => m.IsDel == false) on hr.CreateUser equals su1.Id
                           join su2 in dp.System_User.Where(m => m.IsDel == false) on hr.AuditUser equals su2.Id into su2c
                           from su2ci in su2c.DefaultIfEmpty()
                           join emp in dp.PM_Employee on hr.CreateUser equals emp.RelateUserId into empc
                           from empci in empc.DefaultIfEmpty()
                           join dep in dp.PM_Department on empci.DepartmentId equals dep.Id into depc
                           from depci in depc.DefaultIfEmpty()
                           select new AppraisalModel
                           {
                               Id = hr.Id,
                               ReportCode = hr.ReportCode,
                               ReportName = hr.ReportName,
                               EntrustingParty = hr.EntrustingParty,
                               AppraisalAmount = hr.AppraisalAmount,
                               CostConsultantor = hr.CostConsultantor,
                               CostChecker = hr.CostChecker,
                               BZDate = hr.BZDate,
                               ChargeAmount = hr.ChargeAmount,
                               ChargeStatus = hr.ChargeStatus,
                               SubmitTime = hr.SubmitTime,
                               CreateTime = hr.CreateTime,
                               CreateUser = hr.CreateUser,
                               CreateUserName = su1.TrueName,
                               AuditTime = hr.AuditTime,
                               AuditUser = hr.AuditUser,
                               AuditUserName = su2ci.TrueName,
                               AuditReason = hr.AuditReason,
                               CreateDepName = depci.DepName,
                               Salesman = hr.Salesman,
                               ReportStatus = hr.ReportStatus
                           };
                if (filter.AuditStatus.IsNotNullAndCountGtZero())
                {
                    list = list.Where(m => filter.AuditStatus.Contains(m.ReportStatus));
                }
                if (filter.ReportCode.IsNotNullOrWhiteSpace())
                {
                    list = list.Where(m => m.ReportCode.Contains(filter.ReportCode));
                }
                if (filter.ReportName.IsNotNullOrWhiteSpace())
                {
                    list = list.Where(m => m.ReportName.Contains(filter.ReportName));
                }
                if (filter.ChargeStatus.IsNotNullAndCountGtZero())
                {
                    list = list.Where(m => filter.ChargeStatus.Contains(m.ChargeStatus));
                }
                if (filter.CreateUserName.IsNotNullOrWhiteSpace())
                {
                    list = list.Where(m => m.CreateUserName.Contains(filter.CreateUserName));
                }
                if (filter.AuditUserName.IsNotNullOrWhiteSpace())
                {
                    list = list.Where(m => m.AuditUserName.Contains(filter.AuditUserName));
                }
                if (filter.CreateUserId.HasValue)
                {
                    list = list.Where(m => m.CreateUser == filter.CreateUserId.Value);
                }
                if (filter.AuditUserId.HasValue)
                {

                    list = list.Where(m => dp.RM_ReportAudit.Where(x => x.ReportId == m.Id)
                                                          .Select(x => x.AuditId)
                                                          .Contains(filter.AuditUserId.Value) ||
                                           dp.PM_Employee.Where(x => (dp.RM_ReportAudit.Where(y => y.ReportId == m.Id)
                                                                                     .Select(y => y.AuditId)).Contains(x.DepartmentId.Value))
                                                         .Select(x => x.RelateUserId)
                                                         .Contains(filter.AuditUserId.Value));
                }
                if (filter.CreateBeginTime.HasValue)
                {
                    list = list.Where(m => m.SubmitTime >= filter.CreateBeginTime.Value);
                }
                if (filter.CreateEndTime.HasValue)
                {
                    list = list.Where(m => m.SubmitTime <= filter.CreateEndTime.Value);
                }
                if (filter.AuditBeginTime.HasValue)
                {
                    list = list.Where(m => m.AuditTime >= filter.AuditBeginTime.Value);
                }
                if (filter.AuditEndTime.HasValue)
                {
                    list = list.Where(m => m.AuditTime <= filter.AuditEndTime.Value);
                }
                list = list.OrderBy(m => m.CreateTime);
                total = list.Count();
                if (isPage)
                {
                    return list.Skip(filter.Skip).Take(filter.PageSize).ToList();
                }
                else
                {
                    return list.ToList();
                }
            }
        }


        public bool AuditAppraisal(AppraisalModel model)
        {
            if (model == null || !model.Id.HasValue)
            {
                return false;
            }
            using (DataProvider dp = new DataProvider())
            {
                var entity = dp.CC_Appraisal.FirstOrDefault(m => m.Id == model.Id.Value);
                entity.ReportStatus = model.ReportStatus.ToString();
                entity.AuditReason = model.AuditReason;
                entity.AuditFileName = model.AuditFileName;
                entity.AuditFilePath = model.AuditFilePath;
                entity.AuditTime = DateTime.Now;
                entity.AuditUser = model.AuditUser;
                string auditTxt = model.ReportStatus == ReportStatus.Passed.ToString() ? "审核通过" : "被驳回";
                new Data.SystemData.SysMessageData().SendMessage(dp, new System_Message()
                {
                    CreateTime = DateTime.Now,
                    CreateUser = Guid.Empty,
                    IsDel = false,
                    MsgTitle = $"司法鉴定报告{auditTxt}",
                    MsgType = SysMessageType.Audit.ToString(),
                    MsgContent = $@"您的报告：{entity.ReportName}，{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}{auditTxt}",
                    Url = "/CostConsultation/AppraisalIndex"
                }, new List<Guid>() { entity.CreateUser });
                try
                {
                    dp.SaveChanges();
                    return true;
                }
                catch
                {
                    throw;
                }
            }
        }

        #endregion
    }
}
