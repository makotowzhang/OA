using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;
using Model.SystemModel;

namespace Data.SystemData
{
    public class UserData
    {
        public System_User GetUserById(DataProvider dp, Guid userid)
        {
            return dp.System_User.Where(m => m.Id == userid).FirstOrDefault();
        }

        public int GetUserNameCount(DataProvider dp, UserModel model, bool IsUpdate = false)
        {
            var temp = dp.System_User.Where(m => m.UserName == model.UserName);
            if (IsUpdate)
            {
                temp = temp.Where(m => m.Id != model.Id);
            }
            return temp.Count();
        }

        public void AddUser(DataProvider dp, System_User entity)
        {
            dp.System_User.Add(entity);
        }

        public void AddUserRole(DataProvider dp, System_UserRole entity)
        {
            dp.System_UserRole.Add(entity);
        }

        public void AddUserRole(DataProvider dp, List<System_UserRole> entity)
        {
            dp.System_UserRole.AddRange(entity);
        }

        public void DeleteUserRole(DataProvider dp,Guid userId)
        {
            dp.System_UserRole.RemoveRange(dp.System_UserRole.Where(m=>m.UserId==userId));
        }

        public List<System_User> GetUserList(DataProvider dp, UserFilter filter,out int total,bool IsPage=true)
        {
            var temp = dp.System_User.Where(m => m.IsDel==false);
            if (!string.IsNullOrWhiteSpace(filter.UserName))
            {
                temp = temp.Where(m => m.UserName.Contains(filter.UserName));
            }
            temp = temp.OrderBy(m => m.CreateTime);
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
