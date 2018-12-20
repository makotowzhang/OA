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
using Ionic.Zip;
using System.IO;

namespace Business.RMBusiness
{
    public class HouseReportBusiness
    {
        HouseReportData data = new HouseReportData();

        public HouseReportModel GetModel(Guid? id)
        {
            if (!id.HasValue)
            {
                return null;
            }
            else
            {
                using (DataProvider dp = new DataProvider())
                { 
                    HouseReportModel model = Mapper.Map<HouseReportModel>(data.GetEntity(dp,id));
                    if (model == null)
                    {
                        return null;
                    }
                    model.ReportStatusString = model.ReportStatus.ToString();
                    var dicValue = dp.RM_ReportDicItem.Where(m => m.ReportId == model.Id).ToList();
                    //估价目的
                    model.ValuationObjective = dicValue.Where(m => m.DicGroupCode == DicGroupCode.ValuationObjective.ToString()).Select(m => m.DicItemId).ToList();
                    model.ValuationObjectiveStr = dp.System_DicItem.Where(m => model.ValuationObjective.Contains(m.Id)).Select(m => m.ItemDesc).ToList();
                    //估价方法
                    model.ValuationMethods = dicValue.Where(m => m.DicGroupCode == DicGroupCode.ValuationMethods.ToString()).Select(m => m.DicItemId).ToList();
                    model.ValuationMethodsStr = dp.System_DicItem.Where(m => model.ValuationMethods.Contains(m.Id)).Select(m => m.ItemDesc).ToList();
                    //签字估价师
                    model.SignAppraiser = dicValue.Where(m => m.DicGroupCode == "SignAppraiser").Select(m => m.DicItemId).ToList();
                    model.SignAppraiserName= dp.System_User.Where(m => model.SignAppraiser.Contains(m.Id)).Select(m => m.TrueName).ToList();

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

        public HouseReportModel GetOrCreateModel(Guid createUserId)
        {
            Guid? modelId = null;
            using (DataProvider dp = new DataProvider())
            {
                string reportStatus = ReportStatus.WaitSubmit.ToString();
                if (dp.RM_HouseReport.Count(m => m.CreateUser == createUserId && !m.IsDel && m.ReportStatus == reportStatus) > 0)
                {
                    modelId = dp.RM_HouseReport.Where(m => m.CreateUser == createUserId && !m.IsDel && m.ReportStatus == reportStatus).Select(m => m.Id).FirstOrDefault();
                }
                else
                {
                    HouseReportModel model = new HouseReportModel()
                    {
                        Id = Guid.NewGuid(),
                        IsDel = false,
                        CreateUser = createUserId,
                        CreateTime = DateTime.Now
                    };
                    dp.RM_HouseReport.Add(Mapper.Map<RM_HouseReport>(model));
                    dp.SaveChanges();
                    return model;
                }
            }
            return GetModel(modelId);
        }

        public bool SaveReport(HouseReportModel model,bool isSubmit=false)
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
                entity.HouseNumber = model.HouseNumber;
                entity.BuildArea = model.BuildArea;
                entity.LandNumber = model.LandNumber;
                entity.LandArea = model.LandArea;
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
                    List<Guid> msgUserList = dp.System_User.Where(m => model.AuditUserIds.Contains(m.Id) ||
                    dp.PM_Employee.Where(emp=>model.AuditDep.Contains(emp.DepartmentId.Value)).Select(emp=>emp.RelateUserId).Contains(m.Id)
                    ).Select(m => m.Id).ToList();
                    new Data.SystemData.SysMessageData().SendMessage(dp, new System_Message
                    {
                        CreateTime=DateTime.Now,
                        CreateUser=Guid.Empty,
                        IsDel=false,
                        MsgTitle="房产报告待审批",
                        MsgType=SysMessageType.Audit.ToString(),
                        MsgContent=dp.System_User.Where(m=>m.Id==entity.CreateUser).Select(m=>m.TrueName).FirstOrDefault()+
                                   $"的报告{entity.ReportName},需要您的审核。",
                        Url= "/HouseReport/ReportAudit"
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

        public List<HouseReportModel> GetReportList(HouseReportFilter filter, out int total, bool isPage = true)
        {
            using (DataProvider dp = new DataProvider())
            {
                var list = data.GetReportList(dp, filter, out total, isPage);
                return list;
            }
        }

        public bool Audit(HouseReportModel model)
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
                string auditTxt = model.ReportStatus == ReportStatus.Passed ? "审核通过" : "被驳回";
                new Data.SystemData.SysMessageData().SendMessage(dp, new System_Message
                {
                    CreateTime = DateTime.Now,
                    CreateUser = Guid.Empty,
                    IsDel = false,
                    MsgTitle = $"房产报告{auditTxt}",
                    MsgType = SysMessageType.Audit.ToString(),
                    MsgContent = $@"您的报告：{entity.ReportName}，{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}{auditTxt}",
                    Url = "/HouseReport/ReportCreate"
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

        public byte[] DownloadReportFile(Guid reportId,string rootPath,out string fileName)
        {
            HouseReportModel hr;
            using (DataProvider dp = new DataProvider())
            {
                hr = dp.RM_HouseReport.Where(m => m.Id == reportId).Select(m => new HouseReportModel()
                {
                    ReportName=m.ReportName,
                    EnclosureName = m.EnclosureName,
                    EnclusurePath = m.EnclusurePath,
                    AuditFileName=m.AuditFileName,
                    AuditFilePath=m.AuditFilePath
                }).FirstOrDefault();
            }
            if (hr == null)
            {
                fileName = "";
                return null;
            }
            fileName = hr.ReportName+".zip";
            using (ZipFile zips = new ZipFile(Encoding.UTF8))
            {
                if (File.Exists(rootPath.TrimEnd('\\') + "\\" + hr.EnclusurePath?.TrimStart('\\')))
                { 
                    ZipEntry entry = zips.AddFile(rootPath.TrimEnd('\\') + "\\" + hr.EnclusurePath?.TrimStart('\\'));
                    entry.FileName = $"（工作文件）{hr.EnclosureName}";
                }
                if (File.Exists(rootPath.TrimEnd('\\') + "\\" + hr.AuditFilePath?.TrimStart('\\')))
                {
                    ZipEntry entry = zips.AddFile(rootPath.TrimEnd('\\') + "\\" + hr.AuditFilePath?.TrimStart('\\'));
                    entry.FileName = $"（审核文件）{hr.AuditFileName}";
                }
                using (MemoryStream stream = new MemoryStream())
                {

                    zips.Save(stream);
                    byte[] vs = new byte[stream.Length];
                    stream.Seek(0, SeekOrigin.Begin);
                    stream.Read(vs, 0, vs.Length);
                    return vs;
                }
            }
        }
    }
}
