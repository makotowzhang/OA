using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.PMModel;
using Entity;
using Common;

namespace Data.PMData
{
    public class LeaseData
    {
        public PM_Lease GetLeaById(DataProvider dp, int? leaId)
        {
            return dp.PM_Lease.FirstOrDefault(m => m.Id == leaId);
        }

        public List<LeaseModel> GetLeaList(DataProvider dp, LeaseFilter filter, out int total, bool IsPage = true)
        {
            var list = from lea in dp.PM_Lease.Where(m => !m.IsDel)
                       join dic in dp.System_DicItem.Where(m => !m.IsDel) on lea.LeaseType equals dic.Id
                       select new LeaseModel()
                       {
                           Id=lea.Id,
                           IsEnabled=lea.IsEnabled,
                           LeaseName=lea.LeaseName,
                           LeaseType=lea.LeaseType,
                           LeaseTypeDesc=dic.ItemDesc,
                           Sort=lea.Sort,
                           CreateTime=lea.CreateTime
                       };
            if (filter.LeaseName.IsNotNullOrWhiteSpace())
            {
                list = list.Where(m => m.LeaseName.Contains(filter.LeaseName));
            }
           
            if (filter.LeaseType.IsNotNullAndCountGtZero())
            {
                list = list.Where(m => filter.LeaseType.Contains(m.LeaseType));
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
