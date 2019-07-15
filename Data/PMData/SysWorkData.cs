using Entity;
using Model.PMModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace Data.PMData
{
    public class SysWorkData
    {
        public PM_SysWork GetSysWorkById(DataProvider dp, int id)
        {
            return dp.PM_SysWork.FirstOrDefault(m => m.Id == id);
        }

        public List<SysWorkModel> GetSysWorkList(DataProvider dp, SysWorkFilter filter, out int total, bool IsPage = true)
        {
            var list = from a in  dp.PM_SysWork.Where(m => !m.IsDel) 
                       join b in dp.System_User on a.CreateUser equals b.Id into c from ci in c.DefaultIfEmpty()
                       join d in dp.System_User on a.UpdateUser equals d.Id into e from ei in e.DefaultIfEmpty()
                       select new SysWorkModel
                       {
                           Id=a.Id,
                           WorkNo=a.WorkNo,
                           WorkName=a.WorkNo,
                           Principal=a.Principal,
                           WorkType=a.WorkType,
                           ProjectLeader=a.ProjectLeader,
                           SpecialConsultant=a.SpecialConsultant,
                           FileName=a.FileName,
                           FilePath=a.FilePath,
                           IsDel=a.IsDel,
                           CreateUserName=ci.UserName,
                           UpdateUserName=ei.UserName,
                           CreateTime =a.CreateTime,
                           CreateUser=a.CreateUser,
                           UpdateTime=a.UpdateTime,
                           UpdateUser=a.UpdateUser
                           
                       };
            if (!string.IsNullOrWhiteSpace(filter.WorkNo))
            {
                list = list.Where(m => m.WorkNo.Contains(filter.WorkNo));
            }
            if (!string.IsNullOrWhiteSpace(filter.WorkName))
            {
                list = list.Where(m => m.WorkName.Contains(filter.WorkName));
            }
            if (filter.WorkType.IsNotNullAndCountGtZero())
            {
                list = list.Where(m => filter.WorkType.Contains(m.WorkType));
            }
            if (filter.UserName.ToUpper() != "ADMIN")
            {
                list = list.Where(m => m.CreateUser==filter.UserId);
            }
            list = list.OrderByDescending(m => m.CreateTime);
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
