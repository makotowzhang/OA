using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;
using Model.SystemModel;

namespace Data.SystemData
{
    public class RoleData
    {
        public System_Role GetModel(DataProvider dp, Guid roleId)
        {
            return dp.System_Role.FirstOrDefault(m => m.Id == roleId);
        }

        public void AddRole(DataProvider dp, System_Role entity)
        {
            dp.System_Role.Add(entity);
        }

        public List<System_Role> GetUserRole(DataProvider dp,Guid userId)
        {
            return dp.System_Role.Where(m => dp.System_UserRole.Where(x => x.UserId == userId).Select(x =>(Guid)x.RoleId).Contains(m.Id)).ToList();
        }

        public List<System_Role> GetRoleList(DataProvider dp, RoleFilter filter, out int total, bool IsPage = true)
        {
            var temp = dp.System_Role.Where(m => m.IsDel==false);
            if (!string.IsNullOrWhiteSpace(filter.RoleName))
            {
                temp = temp.Where(m => m.RoleName.Contains(filter.RoleName));
            }
            temp = temp.OrderBy(m => m.Sort);
            total = temp.Count();
            if (IsPage)
            {
                return temp.Skip(filter.Skip).Take(filter.PageSize).ToList();
            }
            else
            {
                return temp.ToList();
            }
        }
    }
}
