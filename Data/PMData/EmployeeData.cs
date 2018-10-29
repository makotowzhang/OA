using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.PMModel;
using Entity;

namespace Data.PMData
{
    public class EmployeeData
    {
        public PM_Employee GetDepById(DataProvider dp, Guid empId)
        {
            return dp.PM_Employee.FirstOrDefault(m => m.Id == empId);
        }

        //public List<PM_Department> GetDepList(DataProvider dp, EmployeeFilter filter, out int total, bool IsPage = true)
        //{
        //    var list = dp.PM_Employee.Where(m => !m.IsDel);
        //    if (!string.IsNullOrWhiteSpace(filter.EmpName))
        //    {
        //        list = list.Where(m => m.EmpName.Contains(filter.EmpName));
        //    }
        //    list = list.OrderByDescending(m => m.CreateTime);
        //    total = list.Count();
        //    if (IsPage)
        //    {
        //        return list.Skip(filter.Skip).Take(filter.PageSize).ToList();
        //    }
        //    else
        //    {
        //        return list.ToList();
        //    }
        //}
    }
}
