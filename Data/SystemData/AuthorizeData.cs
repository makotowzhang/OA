using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;

namespace Data.SystemData
{
    public class AuthorizeData
    {
        public void AddAuthorize(DataProvider dp, System_Authorize entity)
        {
            dp.System_Authorize.Add(entity);
        }

        public void DeleteAuthorize(DataProvider dp, System_Authorize entity)
        {
            dp.System_Authorize.Remove(entity);
        }

        public void DeleteAuthorize(DataProvider dp, List<System_Authorize> entity)
        {
            dp.System_Authorize.RemoveRange(entity);
        }

        public List<System_Authorize> GetAuthorizeListByRoleId(DataProvider dp, Guid roleId)
        {
            return dp.System_Authorize.Where(m => m.RoleId == roleId).ToList();
        }


        public List<System_Menu> GetUserAuthorMenu(DataProvider dp, Guid userId)
        {
            var roleList = dp.System_UserRole.Where(m => m.UserId == userId).Select(m=>m.RoleId);

            var menuList = dp.System_Authorize.Where(m => roleList.Contains(m.RoleId)).Select(m => m.MenuId);

            return dp.System_Menu.Where(m => menuList.Contains(m.Id)).ToList();
        }

        public List<System_Menu> GetAuthorizeAction(DataProvider dp, Guid menuId,List<Guid> roleList)
        {
            var childMenuList = dp.System_Menu.Where(m => m.ParentId == menuId).Select(m=>m.Id);
            var authorize = dp.System_Authorize.Where(m => childMenuList.Contains((Guid)m.MenuId) && roleList.Contains((Guid)m.RoleId)).Select(m=>m.MenuId).Distinct();
            return dp.System_Menu.Where(m => authorize.Contains(m.Id)).ToList();
        }

        public bool CheckUserViewAuthorize(DataProvider dp, Guid menuId, Guid userId)
        {
            Guid viewId = dp.System_Menu.Where(m => m.ParentId == menuId && m.MenuType == "Action" && m.ActionCode=="View" &&m.IsDel==false&&m.IsEnabled==true).Select(m => m.Id).FirstOrDefault();
            if (viewId == Guid.Empty)
            {
                return false;
            }
            var roleList = dp.System_UserRole.Where(m => m.UserId == userId).Select(m => m.RoleId);
            return dp.System_Authorize.Count(m => m.MenuId == viewId && roleList.Contains(m.RoleId)) > 0;
        }
    }
}
