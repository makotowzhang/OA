using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;
using Model.SystemModel;

namespace Data.SystemData
{
    public class LogData
    {
        public void AddLog(DataProvider dp, System_Log log)
        {
            dp.System_Log.Add(log);
        }

        public List<LogModel> GetLogList(DataProvider dp, LogFilter filter,out int total)
        {
            var logView = from log in dp.System_Log
                          join user in dp.System_User on log.DoUser equals user.Id
                          select new LogModel
                          {
                              ActionName = log.ActionName,
                              Description = log.Description,
                              ActionType = log.ActionType,
                              DoTime = log.DoTime,
                              DoUser = log.DoUser,
                              DoUserName = user.TrueName,
                              IpAddress = log.IpAddress,
                              Id = log.Id,
                              MenuName = log.MenuName
                          };
            if (!string.IsNullOrWhiteSpace(filter.ActionName))
            {
                logView = logView.Where(m => m.ActionName.Contains(filter.ActionName));
            }
            if (!string.IsNullOrWhiteSpace(filter.MenuName))
            {
                logView = logView.Where(m => m.MenuName.Contains(filter.MenuName));
            }
            if (!string.IsNullOrWhiteSpace(filter.ActionType))
            {
                logView = logView.Where(m => m.ActionType==filter.ActionType);
            }
            if (!string.IsNullOrWhiteSpace(filter.DoUserName))
            {
                logView = logView.Where(m => m.DoUserName.Contains(filter.DoUserName));
            }
            if (filter.BeginTime!=null)
            {
                logView = logView.Where(m => m.DoTime >= filter.BeginTime);
            }
            if (filter.EndTime != null)
            {
                logView = logView.Where(m => m.DoTime <= filter.EndTime);
            }
            total = logView.Count();
            return logView.OrderByDescending(m=>m.DoTime).Skip(filter.Skip).Take(filter.PageSize).ToList();
        }
    }
}
