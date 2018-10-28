using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.PMModel;
using Entity;

namespace Data.PMData
{
    public class DepartmentData
    {
        public PM_Department GetDepById(DataProvider dp, Guid depId)
        {
            return dp.PM_Department.FirstOrDefault(m => m.Id == depId);
        }

        public List<PM_Department> GetDepList(DataProvider dp, DepartmentFilter filter, out int total, bool IsPage = true)
        {
            var list = dp.PM_Department.Where(m=>!m.IsDel);
            if (!string.IsNullOrWhiteSpace(filter.DepName))
            {
                list = list.Where(m => m.DepName.Contains(filter.DepName));
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
