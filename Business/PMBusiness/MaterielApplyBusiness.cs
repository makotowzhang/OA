using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.PMData;
using Model.PMModel;
using Model.EnumModel;
using AutoMapper;
using Entity;


namespace Business.PMBusiness
{
    public class MaterielApplyBusiness
    {
        private MaterielApplyData data = new MaterielApplyData();

        public MaterielApplyModel GetModel(Guid id)
        {
            using (DataProvider dp = new DataProvider())
            {
                MaterielApplyModel model = Mapper.Map<MaterielApplyModel>(data.GetPMAModel(dp, id));
                if (model != null)
                {
                    model.AuditDep = dp.PM_Department.Where(m => !m.IsDel &&
                                                            dp.PM_AuditUser.Where(x => x.AuditId == model.Id).Select(x => x.AuditId).Contains(m.Id))
                                                          .Select(m => m.Id).ToList();
                    model.AuditUserIds = dp.System_User.Where(m => m.IsDel==false &&
                                                             dp.PM_AuditUser.Where(x => x.AuditId == model.Id).Select(x => x.AuditId).Contains(m.Id))
                                                          .Select(m => m.Id).ToList();
                }
                return model;
            }
        }

        public bool Save(MaterielApplyModel model)
        {
            using (DataProvider dp = new DataProvider())
            {
                var entity = data.GetPMAModel(dp, model.Id);
                if (entity == null)
                {
                    
                    model.Id = Guid.NewGuid();
                    model.AuditStatus = AuditStatus.WaitAudit.ToString();
                    model.CreateTime = DateTime.Now;
                    dp.PM_Employee.Add(Mapper.Map<PM_Employee>(model));
                }
                else
                {
                    entity.MaterielId = model.MaterielId;
                    entity.Quantity = model.Quantity;
                    entity.ApplyReason = model.ApplyReason;
                    entity.UpdateUser = model.UpdateUser;
                    entity.UpdateTime = DateTime.Now;
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
    }
}
