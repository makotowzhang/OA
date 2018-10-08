using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.SystemModel;
using Data.SystemData;
using Entity;
using AutoMapper;

namespace Business.SystemBusiness
{
    public class LogBusiness
    {
        LogData data = new LogData();
        public bool AddLog(LogModel log)
        {
            log.DoTime = DateTime.Now;
            using (DataProvider dp = new DataProvider())
            {
                data.AddLog(dp, Mapper.Map<System_Log>(log));
                dp.SaveChanges();
                return true;
            }
        }

        public TableDataModel GetLogList(LogFilter filter)
        {
            using (DataProvider dp = new DataProvider())
            {
                var tableData = data.GetLogList(dp, filter, out int total);
                return new TableDataModel(total, tableData);
            }
        }
    }
}
