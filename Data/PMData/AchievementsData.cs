using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.PMModel;
using Entity;

namespace Data.PMData
{
    public class AchievementsData
    {

        public List<View_Achievements> GetAchievementsList(DataProvider dp, AchievementsFilter filter, out int total, bool IsPage = true)
        {
            var list = dp.View_Achievements.Where(m=>true);
            if (filter.TimeBegin.HasValue)
            {
                list = list.Where(m => m.AuditTime>= filter.TimeBegin.Value);
            }
            if (filter.TimeEnd.HasValue)
            {
                list = list.Where(m => m.AuditTime <= filter.TimeEnd.Value);
            }
            if (filter.CreateUser.HasValue)
            {
                list = list.Where(m => m.CreateUser == filter.CreateUser.Value);
            }
            list = list.OrderByDescending(m => m.SubmitTime);
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

        public List<AchievementsModel> GetChartData(DataProvider dp, AchievementsFilter filter)
        {
            var list = dp.View_Achievements.Where(m => true);
            if (filter.TimeBegin.HasValue)
            {
                list = list.Where(m => m.AuditTime >= filter.TimeBegin.Value);
            }
            if (filter.TimeEnd.HasValue)
            {
                list = list.Where(m => m.AuditTime <= filter.TimeEnd.Value);
            }
            if (filter.CreateUser.HasValue)
            {
                list = list.Where(m => m.CreateUser == filter.CreateUser.Value);
            }

            return list.Select(m => new AchievementsModel()
            {
                ChargeAmount = m.ChargeAmount,
                ReportFlag = m.ReportFlag,
                CreateUser = m.CreateUser,
                AuditTime = m.AuditTime
            }).ToList();

        }
    }
}
