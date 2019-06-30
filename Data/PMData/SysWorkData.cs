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

        public List<PM_SysWork> GetSysWorkList(DataProvider dp, SysWorkFilter filter, out int total, bool IsPage = true)
        {
            var list = dp.PM_SysWork.Where(m => !m.IsDel);
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
