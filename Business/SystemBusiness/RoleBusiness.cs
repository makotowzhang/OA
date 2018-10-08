using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.SystemModel;
using Data.SystemData;
using Entity;
using AutoMapper;

namespace Business.SystemBusiness
{
    public class RoleBusiness
    {
        RoleData data = new RoleData();
        public bool AddRole(RoleModel model)
        {
            System_Role entity = Mapper.Map<System_Role>(model);
            entity.Id = Guid.NewGuid();
            entity.IsDel = false;
            entity.CreateTime = DateTime.Now;
            using (DataProvider dp = new DataProvider())
            {
                data.AddRole(dp, entity);
                return dp.SaveChanges()==1;
            }
        }

        public bool EditRole(RoleModel model)
        {
            using (DataProvider dp = new DataProvider())
            {
                System_Role entity = data.GetModel(dp, model.Id);
                if (entity == null)
                {
                    return false;
                }
                entity.RoleName = model.RoleName;
                entity.RoleDesc = model.RoleDesc;
                entity.Sort = model.Sort;
                entity.IsEnabled = model.IsEnabled;
                entity.UpdateUser = model.UpdateUser;
                entity.UpdateTime = DateTime.Now;
                return dp.SaveChanges() == 1;
            }
        }

        public RoleModel GetRole(Guid roleId)
        {
            using (DataProvider dp = new DataProvider())
            {
                return Mapper.Map<RoleModel>(data.GetModel(dp, roleId));
            }
        }

        public List<RoleModel> GetRoleList(RoleFilter filter,out int total)
        {
            using (DataProvider dp = new DataProvider())
            {
                List<RoleModel> list = Mapper.Map<List<RoleModel>>(data.GetRoleList(dp, filter, out total));
                return list;
            }
        }

        public List<RoleModel> GetAllRole(RoleFilter filter)
        {
            using (DataProvider dp = new DataProvider())
            {
                int total;
                List<RoleModel> list = Mapper.Map<List<RoleModel>>(data.GetRoleList(dp, filter, out total,false));
                return list;
            }
        }

        public bool DeleteRole(List<RoleModel> model)
        {
            if (model == null || model.Count == 0)
            {
                return false;
            }
            using (DataProvider dp = new DataProvider())
            {
                foreach (RoleModel m in model)
                {
                    System_Role entity = data.GetModel(dp, m.Id);
                    entity.IsDel = true;
                    entity.UpdateUser = m.UpdateUser;
                    entity.UpdateTime = DateTime.Now;
                }
                dp.SaveChanges();
                return true;
            }
        }
    }
}
