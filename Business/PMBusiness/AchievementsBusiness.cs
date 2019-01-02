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
    public class AchievementsBusiness
    {
        private AchievementsData data = new AchievementsData();

        public List<AchievementsModel> GetAchievementsList(AchievementsFilter filter, out int total)
        {
            using (DataProvider dp = new DataProvider())
            {
                var list = data.GetAchievementsList(dp, filter, out total);
                return Mapper.Map<List<AchievementsModel>>(list);
            }
        }

        public AchievementsChartData GetChartData(AchievementsFilter filter)
        {
            using (DataProvider dp = new DataProvider())
            {
               
                var list = data.GetChartData(dp, filter);
                if (list.Count() == 0)
                {
                    return new AchievementsChartData();
                }
                if (!filter.TimeBegin.HasValue)
                {
                    filter.TimeBegin = list.Min(m => m.AuditTime);
                }
                if (!filter.TimeEnd.HasValue)
                {
                    filter.TimeEnd= list.Max(m => m.AuditTime);
                }
                if (filter.TimeEnd < filter.TimeBegin)
                {
                    return new AchievementsChartData();
                }
                AchievementsChartData model = new AchievementsChartData();
                model.PieChartData.Add(new PieChart()
                {
                    name = "房产",
                    value = list.Where(m => m.ReportFlag == "HouseReport").Sum(m => m.ChargeAmount ?? 0)
                });
                model.PieChartData.Add(new PieChart()
                {
                    name = "土地",
                    value = list.Where(m => m.ReportFlag == "AreaReport").Sum(m => m.ChargeAmount ?? 0)
                });
                model.PieChartData.Add(new PieChart()
                {
                    name = "资产",
                    value = list.Where(m => m.ReportFlag == "AssetsReport").Sum(m => m.ChargeAmount ?? 0)
                });
                if (!filter.TimeCycle.HasValue)
                {
                    filter.TimeCycle = Model.EnumModel.TimeCycle.Week;
                    TimeSpan ts = filter.TimeEnd.Value - filter.TimeBegin.Value;
                    if (ts.Days >= 90)
                    {
                        filter.TimeCycle = Model.EnumModel.TimeCycle.Month;
                    }
                    if (ts.Days >= 500)
                    {
                        filter.TimeCycle = Model.EnumModel.TimeCycle.Annual;
                    }
                }
                if (filter.TimeCycle == Model.EnumModel.TimeCycle.Week)
                {
                    DateTime tempTime = filter.TimeBegin.Value.Date;
                    while (tempTime < filter.TimeEnd.Value.Date)
                    {
                        DateTime endTime = tempTime.AddDays(6).AddHours(23).AddMinutes(59).AddSeconds(59);
                        model.LineChartData.xAxis.Add(tempTime.ToString("yyyy.MM.dd") + "-" + endTime.ToString("yyyy.MM.dd"));
                        model.LineChartData.yAxis.Add(list.Where(m => m.AuditTime >= tempTime && m.AuditTime <= endTime).Sum(m=>m.ChargeAmount??0));
                        tempTime =tempTime.AddDays(7);
                    }
                }
                if (filter.TimeCycle == Model.EnumModel.TimeCycle.Month)
                {
                    DateTime tempTime = filter.TimeBegin.Value.Date.AddDays(0-filter.TimeBegin.Value.Day);
                    while (tempTime < filter.TimeEnd.Value.Date)
                    {
                        DateTime endTime = tempTime.AddMonths(1).AddDays(-1).AddHours(23).AddMinutes(59).AddSeconds(59);
                        model.LineChartData.xAxis.Add(tempTime.ToString("yyyy.MM") + "-" + endTime.ToString("yyyy.MM"));
                        model.LineChartData.yAxis.Add(list.Where(m => m.AuditTime >= tempTime && m.AuditTime <= endTime).Sum(m => m.ChargeAmount ?? 0));
                        tempTime = tempTime.AddMonths(1);
                    }
                }
                if (filter.TimeCycle == Model.EnumModel.TimeCycle.Annual)
                {
                    DateTime tempTime = new DateTime(filter.TimeBegin.Value.Year,1,1);
                    while (tempTime < filter.TimeEnd.Value.Date)
                    {
                        DateTime endTime = tempTime.AddYears(1).AddDays(-1).AddHours(23).AddMinutes(59).AddSeconds(59);
                        model.LineChartData.xAxis.Add(tempTime.ToString("yyyy"));
                        model.LineChartData.yAxis.Add(list.Where(m => m.AuditTime >= tempTime && m.AuditTime <= endTime).Sum(m => m.ChargeAmount ?? 0));
                        tempTime = tempTime.AddYears(1);
                    }
                }
                return model;
            }
        }


    }
}
