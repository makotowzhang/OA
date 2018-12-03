using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.RMModel;
using Entity;
using Common;

namespace Data.RMData
{
    public class HouseReportData
    {
        public RM_HouseReport GetEntity(DataProvider dp ,Guid? id)
        {
            if (!id.HasValue)
            {
                return null;
            }
            else
            {
                return dp.RM_HouseReport.FirstOrDefault(m => m.Id == id);
            }
        }


        public List<HouseReportModel> GetReportList(DataProvider dp, HouseReportFilter filter, out int total,bool IsPage = true)
        {
            string waitsubmit = Model.EnumModel.ReportStatus.WaitSubmit.ToString();
            var list = from hr in dp.RM_HouseReport.Where(m => !m.IsDel&&m.ReportStatus!= waitsubmit)
                       join su1 in dp.System_User.Where(m => m.IsDel == false) on hr.CreateUser equals su1.Id
                       join su2 in dp.System_User.Where(m => m.IsDel == false) on hr.AuditUser equals su2.Id into su2c from su2ci in su2c.DefaultIfEmpty()
                       join emp in dp.PM_Employee on hr.CreateUser equals emp.RelateUserId into empc from empci in empc.DefaultIfEmpty()  
                       join dep in dp.PM_Department on empci.DepartmentId equals dep.Id into depc from depci in depc.DefaultIfEmpty()
                       select new HouseReportModel()
                       {
                           Id=hr.Id,
                           ReportCode=hr.ReportCode,
                           ReportName=hr.ReportName,
                           ReportTypeString=hr.ReportType,
                           ReportStatusString=hr.ReportStatus,
                           ChargeAmount =hr.ChargeAmount,
                           ChargeStatus=hr.ChargeStatus,
                           SubmitTime=hr.SubmitTime,
                           CreateTime=hr.CreateTime,
                           CreateUser=hr.CreateUser,
                           CreateUserName=su1.TrueName,
                           AuditTime=hr.AuditTime,
                           AuditUser=hr.AuditUser,
                           AuditUserName=su2ci.TrueName,
                           AuditReason=hr.AuditReason,
                           CreateDepName=depci.DepName,
                           ReportEntruster=hr.ReportEntruster,
                           ReportUser=hr.ReportUser,
                           Salesman=hr.Salesman
                       };
            if (filter.ReportType.IsNotNullAndCountGtZero())
            {
                list = list.Where(m => filter.ReportType.Contains(m.ReportTypeString));
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
            if (filter.AuditStatus.IsNotNullAndCountGtZero())
            {
                list = list.Where(m => filter.AuditStatus.Contains(m.ReportStatusString));
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
                list = list.Where(m => m.CreateTime >= filter.CreateBeginTime.Value);
            }
            if (filter.CreateEndTime.HasValue)
            {
                list = list.Where(m => m.CreateTime <= filter.CreateEndTime.Value);
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
            List<HouseReportModel> temp;
            if (IsPage)
            {
                temp= list.Skip(filter.Skip).Take(filter.PageSize).ToList();
            }
            else
            {
                temp= list.ToList();
            }
            if (temp.IsNotNullAndCountGtZero())
            {
                var reportIdList = temp.Select(m => m.Id);
                var aaa = (from a in dp.RM_ReportDicItem.Where(m => reportIdList.Contains(m.ReportId) && m.DicGroupCode == "SignAppraiser")
                           join b in dp.System_User on a.DicItemId equals b.Id
                           select new
                           {
                               a.ReportId,
                               SignAppraiserName = b.TrueName
                           });
                var signAppraiserCollect = (from a in dp.RM_ReportDicItem.Where(m => reportIdList.Contains(m.ReportId) && m.DicGroupCode == "SignAppraiser")
                                           join b in dp.System_User on a.DicItemId equals b.Id
                                           select new
                                           {
                                               a.ReportId,
                                               SignAppraiserName = b.TrueName
                                           }).ToList();

                foreach (var hr in temp)
                {
                    hr.SignAppraiserName = signAppraiserCollect.Where(m => m.ReportId == hr.Id.Value).Select(m=>m.SignAppraiserName).ToList();
                }
            }
            return temp;
        }
    }
}
