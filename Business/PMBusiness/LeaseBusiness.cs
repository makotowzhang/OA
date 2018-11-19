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
    public class LeaseBusiness
    {
        private LeaseData data = new LeaseData();

        public LeaseModel GetModel(int? id)
        {
            using (DataProvider dp = new DataProvider())
            {
                return Mapper.Map<LeaseModel>(data.GetLeaById(dp, id));
            }
        }

        public bool Save(LeaseModel model)
        {
            using (DataProvider dp = new DataProvider())
            {
                var entity = data.GetLeaById(dp, model.Id);
                if (entity == null)
                {
                    model.IsDel = false;
                    model.CreateTime = DateTime.Now;
                    dp.PM_Lease.Add(Mapper.Map<PM_Lease>(model));
                }
                else
                {
                    entity.LeaseName = model.LeaseName;
                    entity.LeaseType = model.LeaseType;
                    entity.Sort = model.Sort;
                    entity.IsEnabled = model.IsEnabled;
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

        public List<LeaseModel> GetLeaList(LeaseFilter filter, out int total,bool isPage=true)
        {
            using (DataProvider dp = new DataProvider())
            {
                var list = data.GetLeaList(dp, filter, out total, isPage);
                return Mapper.Map<List<LeaseModel>>(list);
            }
        }

        public List<LeaseModel> GetAllLease()
        {
            using (DataProvider dp = new DataProvider())
            {
                return Mapper.Map<List<LeaseModel>>(dp.PM_Lease.Where(m => !m.IsDel&&m.IsEnabled).ToList());
            }
        }

        public bool Delete(List<LeaseModel> list)
        {
            if (list == null || list.Count == 0)
            {
                return false;
            }
            using (DataProvider dp = new DataProvider())
            {
                foreach (var lea in list)
                {
                    if (lea.Id!=0)
                    {
                        continue;
                    }
                    var entity = data.GetLeaById(dp, lea.Id);
                    if (entity == null)
                    {
                        continue;
                    }
                    entity.IsDel = true;
                    entity.UpdateUser = lea.UpdateUser;
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
