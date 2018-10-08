using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.SystemModel;
using Entity;
using Data.SystemData;
using AutoMapper;

namespace Business.SystemBusiness
{
    public class AuthorizeBusiness
    {
        AuthorizeData data = new AuthorizeData();
        public bool SetAuthorize(AuthorizeModel model)
        {
            if (model.RoleId == Guid.Empty || model.MenuIdList == null)
            {
                return false;
            }
            using (DataProvider dp = new DataProvider())
            {
                data.DeleteAuthorize(dp,data.GetAuthorizeListByRoleId(dp, model.RoleId));
                foreach (Guid menuId in model.MenuIdList)
                {
                    data.AddAuthorize(dp, new System_Authorize()
                    {
                        MenuId=menuId,
                        RoleId=model.RoleId,
                        CreateUser=model.CreateUser,
                        CreateTime=DateTime.Now
                    });
                }
                dp.SaveChanges();
                return true;
            }
        }


        public List<Guid?> GetRoleAuthorize(Guid roleId)
        {
            using (DataProvider dp = new DataProvider())
            {
                return data.GetAuthorizeListByRoleId(dp, roleId).Select(m => m.MenuId).ToList();
            }
        }

        public List<MenuModel> GetAuthorizeAction(Guid menuId, List<RoleModel> roleList)
        {
            using (DataProvider dp = new DataProvider())
            {
                return Mapper.Map<List<MenuModel>>(data.GetAuthorizeAction(dp, menuId, roleList.Select(m=>m.Id).ToList()));
            }
        }

        public bool CheckUserViewAuthorize(Guid menuId, Guid userId)
        {
            using (DataProvider dp = new DataProvider())
            {
                return data.CheckUserViewAuthorize(dp, menuId, userId);
            }
        }
    }
}
