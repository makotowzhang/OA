using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;
using Model.SystemModel;
using AutoMapper;
using Data.SystemData;

namespace Business.SystemBusiness
{
    public class UserBusiness
    {
        UserData data = new UserData();

        public UserModel GetUserModel(Guid userId)
        {
            using (DataProvider dp = new DataProvider())
            {
                System_User entity = data.GetUserById(dp, userId);
                if (entity == null)
                {
                    return null;
                }
                UserModel model = Mapper.Map<UserModel>(entity);
                List<System_Role> roleList = new RoleData().GetUserRole(dp, userId);
                model.RoleId = new List<Guid>();
                model.UserRole = new List<RoleModel>();
                if (roleList != null && roleList.Count > 0)
                {
                    foreach (var m in roleList)
                    {
                        model.RoleId.Add(m.Id);
                        model.UserRole.Add(Mapper.Map<RoleModel>(m));
                    }
                }
                return model;
            }
        }

        public List<UserModel> GetUserList(UserFilter filter,out int total, bool IsPage = true)
        {
            using (DataProvider dp = new DataProvider())
            {
                List<UserModel> list = new List<UserModel>();
                RoleData roleData = new RoleData();
                foreach (var m in data.GetUserList(dp, filter,out total, IsPage))
                {
                    UserModel model = Mapper.Map<UserModel>(m);
                    model.UserRole = new List<RoleModel>();
                    List<System_Role> roleList = roleData.GetUserRole(dp, m.Id);
                    if (roleList != null && roleList.Count > 0)
                    {
                        model.UserRole = Mapper.Map<List<RoleModel>>(roleList);
                    }
                    list.Add(model);
                }
                return list;
            }
        }

        public bool AddUser(UserModel model)
        {
            
            System_User entity = Mapper.Map<System_User>(model);
            entity.Id = Guid.NewGuid();
            entity.Password = Common.MD5Encrypt.MD5Encrypt64("123456");
            entity.IsDel = false;
            entity.CreateTime = DateTime.Now;
            List<System_UserRole> userRoleList = new List<System_UserRole>();
            if (model.RoleId != null && model.RoleId.Count > 0)
            {
                foreach (var m in model.RoleId)
                {
                    userRoleList.Add(new System_UserRole()
                    {
                        UserId = entity.Id,
                        RoleId = m,
                        CreateUser = entity.CreateUser,
                        CreateTime = DateTime.Now
                    });
                }
            }
            using (DataProvider dp = new DataProvider())
            {
                if (data.GetUserNameCount(dp, model, false) > 0)
                {
                    return false;
                }
                data.AddUser(dp,entity);
                if (userRoleList.Count > 0)
                {
                    data.AddUserRole(dp, userRoleList);
                }
                try
                {
                    dp.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        public bool EditUser(UserModel model)
        {
            List<System_UserRole> userRoleList = new List<System_UserRole>();
            if (model.RoleId != null && model.RoleId.Count > 0)
            {
                foreach (var m in model.RoleId)
                {
                    userRoleList.Add(new System_UserRole()
                    {
                        UserId = model.Id,
                        RoleId = m,
                        CreateUser = model.UpdateUser,
                        CreateTime = DateTime.Now
                    });
                }
            }
            using (DataProvider dp = new DataProvider())
            {
                if (data.GetUserNameCount(dp, model, true) > 0)
                {
                    return false;
                }
                System_User entity = data.GetUserById(dp, model.Id);
                entity.UserName = model.UserName;
                entity.TrueName = model.TrueName;
                entity.IsEnabled = model.IsEnabled;
                entity.UpdateUser = model.Id;
                entity.UpdateTime = DateTime.Now;
                data.DeleteUserRole(dp, model.Id);
                if (userRoleList.Count > 0)
                {
                    data.AddUserRole(dp, userRoleList);
                }
                try
                {
                    dp.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        public bool CheckUserNameExsit(UserModel model, bool isUpdate=false)
        {
            using (DataProvider dp = new DataProvider())
            {
                return data.GetUserNameCount(dp, model, isUpdate) > 0;
            }
        }

        public bool DeleteUser(List<UserModel> model)
        {
            if (model == null || model.Count == 0)
            {
                return false;
            }
            using (DataProvider dp = new DataProvider())
            {
                foreach (UserModel m in model)
                {
                    System_User entity = data.GetUserById(dp, m.Id);
                    entity.IsDel = true;
                    entity.UpdateUser = m.UpdateUser;
                    entity.UpdateTime = DateTime.Now;
                }
                dp.SaveChanges();
                return true;
            }
        }

        public bool ChangePwd(Guid userId, string newPwd)
        {
            using (DataProvider dp = new DataProvider())
            {
                System_User user = data.GetUserById(dp, userId);
                user.Password = newPwd;
                dp.SaveChanges();
                return true;
            }
        }
    }
}
