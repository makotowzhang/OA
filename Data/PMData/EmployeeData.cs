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
        public PM_Employee GetEmpById(DataProvider dp, Guid? empId)
        {
            return dp.PM_Employee.FirstOrDefault(m => m.Id == empId);
        }

        public List<EmployeeModel> GetEmpList(DataProvider dp, EmployeeFilter filter, out int total, bool IsPage = true)
        {
            var list = from a in dp.PM_Employee.Where(m => !m.IsDel)
                       join b in dp.System_User.Where(m => m.IsDel == false) on a.RelateUserId equals b.Id
                       join dep in dp.PM_Department.Where(m=>!m.IsDel) on a.DepartmentId equals dep.Id
                       select new EmployeeModel
                       {
                           Id = a.Id,
                           CreateTime = a.CreateTime,
                           CreateUser = a.CreateUser,
                           EmpCode = a.EmpCode,
                           EmpName = a.EmpName,
                           PhoneNumber = a.PhoneNumber,
                           RelateUserId = a.RelateUserId,
                           RelateUserName = b.UserName,
                           DepName=dep.DepName,
                           DepartmentId=dep.Id
                       };
            if (!string.IsNullOrWhiteSpace(filter.EmpCode))
            {
                list = list.Where(m => m.EmpCode.Contains(filter.EmpCode));
            }
            if (!string.IsNullOrWhiteSpace(filter.EmpName))
            {
                list = list.Where(m => m.EmpName.Contains(filter.EmpName));
            }
            if (filter.DepList!=null &&filter.DepList.Count>0)
            {
                list = list.Where(m =>filter.DepList.Contains(m.DepartmentId));
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

        public List<EmployeeModel> GetUserEmpList(DataProvider dp)
        {
            var list = from a in dp.PM_Employee.Where(m => !m.IsDel)
                       join b in dp.System_User.Where(m => m.IsDel == false) on a.RelateUserId equals b.Id
                       select new EmployeeModel
                       {
                           RelateUserId = a.RelateUserId,
                           EmpName = a.EmpName
                       };
            return list.ToList();
        }
    }
}
