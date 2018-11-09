using System;
using System.Collections.Generic;
using System.Linq;
using Model.PMModel;
using Entity;
using Common;

namespace Data.PMData
{
    public class MaterielApplyData
    {
        public PM_MaterielApply GetPMAModel(DataProvider dp,Guid? id)
        {
            return dp.PM_MaterielApply.FirstOrDefault(m => m.Id == id);
        }

        public List<MaterielApplyModel> GetPMAList(DataProvider dp, MaterielApplyFilter filter, out int total, bool IsPage = true)
        {
            var list = from pma in dp.PM_MaterielApply
                       join mat in dp.PM_Materiel.Where(m => !m.IsDel) on pma.MaterielId equals mat.Id
                       join su1 in dp.System_User.Where(m => m.IsDel == false) on pma.CreateUser equals su1.Id
                       join su2 in dp.System_User.Where(m => m.IsDel == false) on pma.AuditUser equals su2.Id into su2c
                       from su2ci in su2c.DefaultIfEmpty()
                       select new MaterielApplyModel
                       {
                           Id=pma.Id,
                           ApplyReason=pma.ApplyReason,
                           AuditReason=pma.AuditReason,
                           AuditStatus=pma.AuditStatus,
                           AuditTime=pma.AuditTime,
                           AuditUser=pma.AuditUser,
                           AuditUserName=su2ci.TrueName,
                           CreateTime=pma.CreateTime,
                           CreateUser=pma.CreateUser,
                           CreateUserName=su1.TrueName,
                           MaterielId=pma.MaterielId,
                           MaterielName=mat.MaterielName,
                           Quantity=pma.Quantity,
                           UpdateTime=pma.UpdateTime,
                           UpdateUser=pma.UpdateUser
                       };
            if (filter.AuditStatus.IsNotNullAndCountGtZero())
            {
                list = list.Where(m => filter.AuditStatus.Contains(m.AuditStatus));
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
                list = list.Where(m => m.CreateUser == filter.CreateUserId);
            }
            if (filter.AuditUserId.HasValue)
            {

                list = list.Where(m => dp.PM_AuditUser.Where(x => x.AuditId == m.Id)
                                                      .Select(x => x.AuditId)
                                                      .Contains(filter.AuditUserId)||
                                       dp.PM_Employee.Where(x => (dp.PM_AuditUser.Where(y => y.AuditId == m.Id)
                                                                                 .Select(y => y.AuditId)).Contains(x.DepartmentId))
                                                     .Select(x => x.RelateUserId)
                                                     .Contains(filter.AuditUserId));
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
            if (IsPage)
            {
                return list.Skip(filter.Skip).Take(filter.PageSize).ToList();
            }
            else
            {
                return list.ToList();
            }
        }
    }
}
