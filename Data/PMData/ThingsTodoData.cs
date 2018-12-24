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
    public class ThingsTodoData
    {
        public PM_ThingsTodo GetEntityById(DataProvider dp, int? Id)
        {
            return dp.PM_ThingsTodo.FirstOrDefault(m => m.Id == Id);
        }

        public List<PM_ThingsTodo> GetList(DataProvider dp, ThingsTodoFilter filter, out int total, bool IsPage = true)
        {
            var list =  dp.PM_ThingsTodo.Where(m=>true) ;
            if (filter.BeginTime.HasValue)
            {
                list = list.Where(m => m.TodoDate >= filter.BeginTime.Value);
            }
            if (filter.EndTime.HasValue)
            {
                list = list.Where(m => m.TodoDate <= filter.EndTime.Value);
            }
            if (filter.TodoThings.IsNotNullOrWhiteSpace())
            {
                list = list.Where(m => m.TodoThings.Contains(filter.TodoThings));
            }
            if (filter.CreateUser.HasValue)
            {
                list = list.Where(m => m.CreateUser==filter.CreateUser.Value);
            }
            if (filter.IsAlert.HasValue)
            {
                list = list.Where(m => m.IsAlert == filter.IsAlert.Value);
            }
            list = list.OrderByDescending(m => m.TodoDate);
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
