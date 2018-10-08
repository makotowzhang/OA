using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;
using Model.SystemModel;

namespace Data.SystemData
{
    public class MenuData
    {
        public List<System_Menu> GetMenuByParentId(DataProvider dp, Guid parentId)
        {
            return dp.System_Menu.Where(m => m.ParentId == parentId && m.IsDel == false).OrderBy(m=>m.Sort).ToList();
        }

        public List<System_Menu> GetMenuByParentIdWithoutAction(DataProvider dp, Guid parentId)
        {
            return dp.System_Menu.Where(m => m.ParentId == parentId && m.IsDel == false&&m.MenuType!="Action").OrderBy(m => m.Sort).ToList();
        }

        public System_Menu GetMenuById(DataProvider dp, Guid id)
        {
            return dp.System_Menu.FirstOrDefault(m => m.Id == id && m.IsDel == false);
        }


        public void AddMenu(DataProvider dp, System_Menu entity)
        {
             dp.System_Menu.Add(entity);
        }

        public List<Guid?> GetRoleAuthorMenuId(DataProvider dp, List<Guid> roleList)
        {
            var actionCode = Model.EnumModel.MenuType.Action.ToString();
            var viewMenu = dp.System_Menu.Where(m => m.MenuType == actionCode&&m.ActionCode=="View" && m.IsDel == false && m.IsEnabled == true).Select(m=>m.Id);
            return dp.System_Authorize.Where(m => roleList.Contains((Guid)m.RoleId)&&viewMenu.Contains((Guid)m.MenuId)).Select(m => m.MenuId).ToList();
        }

        public List<System_Menu> GetParentMenu(DataProvider dp, List<Guid> menuList)
        {
            var parentMenuId = dp.System_Menu.Where(m => menuList.Contains(m.Id)).Select(m => m.ParentId).Distinct();
            if (parentMenuId.Count() == 0)
            {
                return new List<System_Menu>();
            }
            else
            {
                List<Guid> guids = parentMenuId.Select(m => m.Value).ToList();
                var list = dp.System_Menu.Where(m => guids.Contains(m.Id) && m.IsEnabled == true && m.IsDel == false).ToList();
                foreach (var m in GetParentMenu(dp, guids))
                {
                    if (!list.Select(x => x.Id).Contains(m.Id))
                    {
                        list.Add(m);
                    }
                }
                return list;
            }

        }

       
    }
}
