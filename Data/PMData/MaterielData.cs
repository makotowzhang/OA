using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.PMModel;
using Entity;

namespace Data.PMData
{
    public class MaterielData
    {
        public PM_Materiel GetMatById(DataProvider dp, int matId)
        {
            return dp.PM_Materiel.FirstOrDefault(m => m.Id == matId);
        }

        public List<MaterielModel> GetMatList(DataProvider dp, MaterielFilter filter, out int total, bool IsPage = true)
        {
            var list = from mat in dp.PM_Materiel.Where(m => !m.IsDel)
                       join dic in dp.System_DicItem.Where(m => !m.IsDel) on mat.MaterielType equals dic.Id
                       select new MaterielModel()
                       {
                           Id=mat.Id,
                           IsEnabled=mat.IsEnabled,
                           MaterielCode=mat.MaterielCode,
                           MaterielName=mat.MaterielName,
                           MaterielType=mat.MaterielType,
                           MaterielTypeDesc=dic.ItemDesc,
                           ResidualQuantity=mat.ResidualQuantity,
                           Sort=mat.Sort,
                           CreateTime=mat.CreateTime
                       };
            if (!string.IsNullOrWhiteSpace(filter.MaterielCode))
            {
                list = list.Where(m => m.MaterielCode.Contains(filter.MaterielCode));
            }
            if (!string.IsNullOrWhiteSpace(filter.MaterielName))
            {
                list = list.Where(m => m.MaterielName.Contains(filter.MaterielName));
            }
            if (filter.MaterielType!=null &&filter.MaterielType.Count>0)
            {
                list = list.Where(m => filter.MaterielType.Contains(m.MaterielType));
            }
            list = list.OrderBy(m => m.Sort).ThenByDescending(m=>m.CreateTime);
            total = list.Count();
            if (IsPage)
            {
                return list.Skip(filter.Skip).Take(filter.PageSize).ToList();
            }
            else
            {
                return list.ToList();
            }
        }
    }
}
