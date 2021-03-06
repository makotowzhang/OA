﻿using System;
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

        public List<AchievementsModel> GetAchievementsList(DataProvider dp, AchievementsFilter filter, out int total, bool IsPage = true)
        {
            var list = from a in dp.View_Achievements
                       join b in dp.System_User on a.CreateUser equals b.Id
                       select new AchievementsModel()
                       {
                           Id = a.Id,
                           ReportCode = a.ReportCode,
                           ReportName = a.ReportName,
                           ChargeAmount = a.ChargeAmount,
                           ReportType = a.ReportType,
                           ReportStatus = a.ReportStatus,
                           CreateUser = a.CreateUser,
                           SubmitTime = a.SubmitTime,
                           AuditTime = a.AuditTime,
                           ChargeStatus = a.ChargeStatus,
                           ReportFlag = a.ReportFlag,
                           CreateUserName = b.TrueName
                       };
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
            var list = from a in dp.View_Achievements
                       join b in dp.System_User on a.CreateUser equals b.Id
                       select new AchievementsModel()
                       {
                           Id = a.Id,
                           ReportCode = a.ReportCode,
                           ReportName = a.ReportName,
                           ChargeAmount = a.ChargeAmount,
                           ReportType = a.ReportType,
                           ReportStatus = a.ReportStatus,
                           CreateUser = a.CreateUser,
                           SubmitTime = a.SubmitTime,
                           AuditTime = a.AuditTime,
                           ChargeStatus = a.ChargeStatus,
                           ReportFlag = a.ReportFlag,
                           CreateUserName = b.TrueName
                       };
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

            return list.ToList();

        }
    }
}
