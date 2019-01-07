using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.PMData;
using Model.PMModel;
using AutoMapper;
using Entity;

namespace Business.PMBusiness
{
    public class EmployeeBusiness
    {
        private EmployeeData data = new EmployeeData();

        public EmployeeModel GetModel(Guid id)
        {
            using (DataProvider dp = new DataProvider())
            {
                return Mapper.Map<EmployeeModel>(data.GetEmpById(dp, id));
            }
        }

        public bool Save(EmployeeModel model)
        {
            using (DataProvider dp = new DataProvider())
            {
                var entity = data.GetEmpById(dp, model.Id);
                if (entity == null)
                {
                    if (model.IsRelateUser)
                    {
                        System_User user = new System_User()
                        {
                            Id=Guid.NewGuid(),
                            CreateTime=DateTime.Now,
                            CreateUser=model.CreateUser,
                            IsDel=false,
                            IsEnabled=true,
                            Password=Common.MD5Encrypt.MD5Encrypt64("123456"),
                            TrueName=model.EmpName,
                            UserName=model.RelateUserName
                        };
                        model.RelateUserId = user.Id;
                        dp.System_User.Add(user);
                        if (model.RelateRoleIds != null && model.RelateRoleIds.Count > 0)
                        {
                            model.RelateRoleIds.ForEach(m=>
                            {
                                System_UserRole relate = new System_UserRole()
                                {
                                    UserId=user.Id,
                                    RoleId=m,
                                    CreateTime=DateTime.Now,
                                    CreateUser=model.CreateUser
                                };
                                dp.System_UserRole.Add(relate);
                            });
                        }
                    }
                    model.Id = Guid.NewGuid();
                    model.IsDel = false;
                    model.CreateTime = DateTime.Now;
                    dp.PM_Employee.Add(Mapper.Map<PM_Employee>(model));
                }
                else
                {
                    entity.EmpName = model.EmpName;
                    entity.PhoneNumber = model.PhoneNumber;
                    entity.DepartmentId = model.DepartmentId;
                    entity.UpdateUser = model.UpdateUser;
                    entity.UpdateTime = DateTime.Now;
                    var user = dp.System_User.FirstOrDefault(m => m.Id == entity.RelateUserId);
                    if (user != null)
                    {
                        user.TrueName = entity.EmpName;
                    }
                }
                try
                {
                    dp.SaveChanges();
                    return true;
                }
                catch
                {
                    throw;
                }
            }
        }

        public List<EmployeeModel> GetEmpList(EmployeeFilter filter, out int total)
        {
            using (DataProvider dp = new DataProvider())
            {
                var list = data.GetEmpList(dp, filter, out total);
                return Mapper.Map<List<EmployeeModel>>(list);
            }
        }

        public bool Delete(List<EmployeeModel> list)
        {
            if (list == null || list.Count == 0)
            {
                return false;
            }
            using (DataProvider dp = new DataProvider())
            {
                foreach (var dep in list)
                {
                    if (!dep.Id.HasValue)
                    {
                        continue;
                    }
                    var entity = data.GetEmpById(dp, dep.Id.Value);
                    if (entity == null)
                    {
                        continue;
                    }
                    entity.IsDel = true;
                    entity.UpdateUser = dep.UpdateUser;
                    entity.UpdateTime = DateTime.Now;
                    var user = dp.System_User.FirstOrDefault(m => m.Id == entity.RelateUserId);
                    if (user != null)
                    {
                        user.IsDel = true;
                        user.UpdateUser = dep.UpdateUser;
                        user.UpdateTime = DateTime.Now;
                    }
                }
                try
                {
                    dp.SaveChanges();
                    return true;
                }
                catch
                {
                    throw;
                }
            }
        }

        public bool CheckEmpCode(string empCode)
        {
            using (DataProvider dp = new DataProvider())
            {
                return dp.PM_Employee.Count(m => m.EmpCode == empCode && !m.IsDel) > 0;
            }
        }

        public List<EmployeeModel> GetUserEmpList()
        {
            using (DataProvider dp = new DataProvider())
            {
                return data.GetUserEmpList(dp);
            }
        }
    }
}
