using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.SystemData;
using Entity;
using Model.SystemModel;
using AutoMapper;

namespace Business.SystemBusiness
{
    public class LoginBusiness
    {
        LoginData data = new LoginData();
        public UserModel CheckUser(LoginModel model )
        {
            using (DataProvider dp = new DataProvider())
            {
                List<System_User> user = data.CheckUser(dp, model);
                if (user == null || user.Count == 0 || user.Count > 1)
                {
                    return null;
                }
                else
                {
                    return Mapper.Map<UserModel>(user[0]);
                }
            }
        }

        public UserModel GetUserInfoById(Guid userId)
        {
            using (DataProvider dp = new DataProvider())
            {
                var user = new UserData().GetUserById(dp, userId);
                if (user == null)
                {
                    return null;
                }
                
                UserModel model = Mapper.Map<UserModel>(user);
                model.UserRole = new List<RoleModel>();
                var roleList = new RoleData().GetUserRole(dp, userId);
                if (roleList != null && roleList.Count > 0)
                {
                    model.UserRole = Mapper.Map<List<RoleModel>>(roleList);
                }
                return model;
            }
           
        }

    }

}
