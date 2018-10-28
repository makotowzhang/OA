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
    public class DepartmentBusiness
    {
        private DepartmentData data = new DepartmentData();

        public DepartmentModel GetModel(Guid id)
        {
            using (DataProvider dp = new DataProvider())
            { 
                return Mapper.Map<DepartmentModel>(data.GetDepById(dp,id));
            }
        }

        public bool Save(DepartmentModel model)
        {
            using (DataProvider dp = new DataProvider())
            {
                var entity = data.GetDepById(dp,model.Id);
                if (entity == null)
                {
                    model.Id = Guid.NewGuid();
                    model.IsDel = false;
                    model.CreateTime = DateTime.Now;
                    dp.PM_Department.Add(Mapper.Map<PM_Department>(model));
                }
                else
                {
                    entity.DepName = model.DepName;
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

        public List<DepartmentModel> GetDepList(DepartmentFilter filter,out int total)
        {
            using (DataProvider dp = new DataProvider())
            {
                var list = data.GetDepList(dp, filter,out total);
                return Mapper.Map<List<DepartmentModel>>(list);
            }
        }

        public bool Delete(List<DepartmentModel> list)
        {
            if (list == null || list.Count == 0)
            {
                return false;
            }
            using (DataProvider dp = new DataProvider())
            {
                foreach (var dep in list)
                {
                    var entity = data.GetDepById(dp,dep.Id);
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
    }
}
