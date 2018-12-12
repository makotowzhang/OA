using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.RMData;
using AutoMapper;
using Model.RMModel;
using Model.EnumModel;
using Entity;
using Common;

namespace Business.RMBusiness
{
    public class AreaReportBusiness
    {
        AreaReportData data = new AreaReportData();

        public AreaReportModel GetModel(Guid? id)
        {
            if (!id.HasValue)
            {
                return null;
            }
            else
            {
                using (DataProvider dp = new DataProvider())
                { 
                    AreaReportModel model = Mapper.Map<AreaReportModel>(data.GetEntity(dp,id));
                    model.ReportStatusString = model.ReportStatus.ToString();
                    var dicValue = dp.RM_ReportDicItem.Where(m => m.ReportId == model.Id).ToList();
                    model.ValuationObjective = dicValue.Where(m => m.DicGroupCode == DicGroupCode.ValuationObjective.ToString()).Select(m => m.DicItemId).ToList();
                    model.ValuationMethods = dicValue.Where(m => m.DicGroupCode == DicGroupCode.ValuationMethods.ToString()).Select(m => m.DicItemId).ToList();
                    model.SignAppraiser = dicValue.Where(m => m.DicGroupCode == "SignAppraiser").Select(m => m.DicItemId).ToList();
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
        }

        public AreaReportModel GetOrCreateModel(Guid createUserId)
        {
            Guid? modelId = null;
            using (DataProvider dp = new DataProvider())
            {
                string reportStatus = ReportStatus.WaitSubmit.ToString();
                if (dp.RM_AreaReport.Count(m => m.CreateUser == createUserId && !m.IsDel && m.ReportStatus == reportStatus) > 0)
                {
                    modelId = dp.RM_AreaReport.Where(m => m.CreateUser == createUserId && !m.IsDel && m.ReportStatus == reportStatus).Select(m => m.Id).FirstOrDefault();
                }
                else
                {
                    AreaReportModel model = new AreaReportModel()
                    {
                        Id = Guid.NewGuid(),
                        IsDel = false,
                        CreateUser = createUserId,
                        CreateTime = DateTime.Now
                    };
                    dp.RM_AreaReport.Add(Mapper.Map<RM_AreaReport>(model));
                    dp.SaveChanges();
                    return model;
                }
            }
            return GetModel(modelId);
        }

        public bool SaveReport(AreaReportModel model,bool isSubmit=false)
        {
            using (DataProvider dp = new DataProvider())
            {
                var entity = data.GetEntity(dp, model.Id);
                entity.ReportCode = model.ReportCode;
                entity.ReportName = model.ReportName;
                entity.ReportEntruster = model.ReportEntruster;
                entity.ReportUser = model.ReportUser;
                entity.Obligee = model.Obligee;
                entity.LocationAddress = model.LocationAddress;
                entity.AreaNumber = model.AreaNumber;
                entity.AreaRegLand = model.AreaRegLand;
                entity.AreaValuationLand = model.AreaValuationLand;
                entity.AreaEndTime = model.AreaEndTime;
                entity.LiftUseYear = model.LiftUseYear;
                entity.AreaPurpose = model.AreaPurpose;
                entity.SetFloorAreaRatio = model.SetFloorAreaRatio;
                entity.SetLevelDev = model.SetLevelDev;
                entity.ActualDevelopmentLevel = model.ActualDevelopmentLevel;
                entity.ValuationPrice = model.ValuationPrice;
                entity.ValuationValue = model.ValuationValue;
                entity.ValuationPurpose = model.ValuationPurpose;
                entity.ValuationStruct = model.ValuationStruct;
                entity.ValueTime = model.ValueTime;
                entity.ReportTime = model.ReportTime;
                entity.ReportDesc = model.ReportDesc;
                entity.WorkName = model.WorkName;
                entity.WorkDesc = model.WorkDesc;
                entity.EnclosureName = model.EnclosureName;
                entity.EnclusurePath = model.EnclusurePath;
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
                //删除估价目的、估价方法、签字估价师
                dp.RM_ReportDicItem.RemoveRange(dp.RM_ReportDicItem.Where(m => m.ReportId == entity.Id));
                //添加估价目的
                if (model.ValuationObjective.IsNotNullAndCountGtZero())
                {
                    model.ValuationObjective.ForEach(m => dp.RM_ReportDicItem.Add(new RM_ReportDicItem()
                    {
                        DicGroupCode=DicGroupCode.ValuationObjective.ToString(),
                        DicItemId=m,
                        ReportId=entity.Id
                    }));
                }
                //添加估价方法
                if (model.ValuationMethods.IsNotNullAndCountGtZero())
                {
                    model.ValuationMethods.ForEach(m => dp.RM_ReportDicItem.Add(new RM_ReportDicItem()
                    {
                        DicGroupCode = DicGroupCode.ValuationMethods.ToString(),
                        DicItemId = m,
                        ReportId = entity.Id
                    }));
                }
                //添加签字估价师
                if (model.SignAppraiser.IsNotNullAndCountGtZero())
                {
                    model.SignAppraiser.ForEach(m => dp.RM_ReportDicItem.Add(new RM_ReportDicItem()
                    {
                        DicGroupCode = "SignAppraiser",
                        DicItemId = m,
                        ReportId = entity.Id
                    }));
                }

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

        public List<AreaReportModel> GetReportList(AreaReportFilter filter, out int total, bool isPage = true)
        {
            using (DataProvider dp = new DataProvider())
            {
                var list = data.GetReportList(dp, filter, out total, isPage);
                return list;
            }
        }

        public bool Audit(AreaReportModel model)
        {
            if (model == null || !model.Id.HasValue)
            {
                return false;
            }
            using (DataProvider dp = new DataProvider())
            {
                var entity = data.GetEntity(dp, model.Id);
                entity.ReportStatus = model.ReportStatus.ToString();
                entity.AuditReason = model.AuditReason;
                entity.AuditFileName = model.AuditFileName;
                entity.AuditFilePath = model.AuditFilePath;
                entity.AuditTime = DateTime.Now;
                entity.AuditUser = model.AuditUser;
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
    }
}
