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
    public class MaterielBusiness
    {
        private MaterielData data = new MaterielData();

        public MaterielModel GetModel(int id)
        {
            using (DataProvider dp = new DataProvider())
            {
                return Mapper.Map<MaterielModel>(data.GetMatById(dp, id));
            }
        }

        public bool Save(MaterielModel model)
        {
            using (DataProvider dp = new DataProvider())
            {
                var entity = data.GetMatById(dp, model.Id);
                if (entity == null)
                {
                    model.IsDel = false;
                    model.CreateTime = DateTime.Now;
                    dp.PM_Materiel.Add(Mapper.Map<PM_Materiel>(model));
                }
                else
                {
                    entity.MaterielCode = model.MaterielCode;
                    entity.MaterielName = model.MaterielName;
                    entity.MaterielType = model.MaterielType;
                    entity.ResidualQuantity = model.ResidualQuantity;
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

        public List<MaterielModel> GetMatList(MaterielFilter filter, out int total,bool isPage=true)
        {
            using (DataProvider dp = new DataProvider())
            {
                var list = data.GetMatList(dp, filter, out total, isPage);
                return Mapper.Map<List<MaterielModel>>(list);
            }
        }

        public List<MaterielModel> GetAllMat()
        {
            using (DataProvider dp = new DataProvider())
            {
                return Mapper.Map<List<MaterielModel>>(dp.PM_Materiel.Where(m => !m.IsDel&&m.IsEnabled).ToList());
            }
        }

        public bool Delete(List<MaterielModel> list)
        {
            if (list == null || list.Count == 0)
            {
                return false;
            }
            using (DataProvider dp = new DataProvider())
            {
                foreach (var mat in list)
                {
                    if (mat.Id!=0)
                    {
                        continue;
                    }
                    var entity = data.GetMatById(dp, mat.Id);
                    if (entity == null)
                    {
                        continue;
                    }
                    entity.IsDel = true;
                    entity.UpdateUser = mat.UpdateUser;
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

        public bool CheckMatCodeExsit(string matCode, int? id)
        {
            using (DataProvider dp = new DataProvider())
            {
                return dp.PM_Materiel.Count(m => m.MaterielCode == matCode && m.Id != id) > 0;
            }
        }
    }
}
