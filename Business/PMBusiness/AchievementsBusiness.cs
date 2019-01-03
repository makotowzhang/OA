using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.PMData;
using Model.PMModel;
using AutoMapper;
using Entity;
using Aspose.Cells;
using System.IO;

namespace Business.PMBusiness
{
    public class AchievementsBusiness
    {
        private AchievementsData data = new AchievementsData();

        public List<AchievementsModel> GetAchievementsList(AchievementsFilter filter, out int total,bool isPage=true)
        {
            using (DataProvider dp = new DataProvider())
            {
                var list = data.GetAchievementsList(dp, filter, out total, isPage);
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
                model.PieChartData = model.PieChartData.Where(m => m.value != 0).ToList();
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
                    while (tempTime <= filter.TimeEnd.Value.Date)
                    {
                        DateTime endTime = tempTime.AddDays(6).AddHours(23).AddMinutes(59).AddSeconds(59);
                        model.LineChartData.xAxis.Add(tempTime.ToString("yyyy.MM.dd") + "-" + endTime.ToString("yyyy.MM.dd"));
                        model.LineChartData.yAxis.Add(list.Where(m => m.AuditTime >= tempTime && m.AuditTime <= endTime).Sum(m=>m.ChargeAmount??0));
                        tempTime =tempTime.AddDays(7);
                    }
                }
                if (filter.TimeCycle == Model.EnumModel.TimeCycle.Month)
                {
                    DateTime tempTime = filter.TimeBegin.Value.Date.AddDays(1-filter.TimeBegin.Value.Day);
                    while (tempTime <= filter.TimeEnd.Value.Date)
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
                    while (tempTime <= filter.TimeEnd.Value.Date)
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

        public SummaryAchievementsChartData GetSummaryChartData(AchievementsFilter filter)
        {
            using (DataProvider dp = new DataProvider())
            {

                var list = data.GetChartData(dp, filter);
                if (list.Count() == 0)
                {
                    return new SummaryAchievementsChartData();
                }
                if (!filter.TimeBegin.HasValue)
                {
                    filter.TimeBegin = list.Min(m => m.AuditTime);
                }
                if (!filter.TimeEnd.HasValue)
                {
                    filter.TimeEnd = list.Max(m => m.AuditTime);
                }
                if (filter.TimeEnd < filter.TimeBegin)
                {
                    return new SummaryAchievementsChartData();
                }
                SummaryAchievementsChartData model = new SummaryAchievementsChartData();
                foreach (var userid in list.Select(x => x.CreateUser).Distinct())
                {
                    var userTrueName = list.Where(m => m.CreateUser == userid).Select(m => m.CreateUserName).FirstOrDefault();
                    model.PieChartData.Add(new PieChart()
                    {
                        name= userTrueName,
                        value= list.Where(m => m.CreateUser == userid).Sum(m => m.ChargeAmount ?? 0)
                    });
                    model.PersonalLineChartData.series.Add(new AllLineChartSeries()
                    {
                        name = userTrueName,
                        keyFilter = userid
                    });
                }
                model.PieChartData = model.PieChartData.Where(m => m.value != 0).ToList();
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
                model.AllLineChartData.series.Add(new AllLineChartSeries()
                {
                    name="房产",
                    keyFilter= "HouseReport"
                });
                model.AllLineChartData.series.Add(new AllLineChartSeries()
                {
                    name = "土地",
                    keyFilter = "AreaReport"
                });
                model.AllLineChartData.series.Add(new AllLineChartSeries()
                {
                    name = "资产",
                    keyFilter = "AssetsReport"
                });
                if (filter.TimeCycle == Model.EnumModel.TimeCycle.Week)
                {
                    DateTime tempTime = filter.TimeBegin.Value.Date;
                    while (tempTime <= filter.TimeEnd.Value.Date)
                    {
                        DateTime endTime = tempTime.AddDays(6).AddHours(23).AddMinutes(59).AddSeconds(59);
                        model.AllLineChartData.xAxis.Add(tempTime.ToString("yyyy.MM.dd") + "-" + endTime.ToString("yyyy.MM.dd"));
                        foreach (var ser in model.AllLineChartData.series)
                        {
                            ser.data.Add(list.Where(m => m.AuditTime >= tempTime && m.AuditTime <= endTime && m.ReportFlag == ser.keyFilter).Sum(m => m.ChargeAmount ?? 0));
                        }
                        model.PersonalLineChartData.xAxis.Add(tempTime.ToString("yyyy.MM.dd") + "-" + endTime.ToString("yyyy.MM.dd"));
                        foreach (var ser in model.PersonalLineChartData.series)
                        {
                            ser.data.Add(list.Where(m => m.AuditTime >= tempTime && m.AuditTime <= endTime && m.CreateUser == ser.keyFilter).Sum(m => m.ChargeAmount ?? 0));
                        }
                        tempTime = tempTime.AddDays(7);
                    }
                }
                if (filter.TimeCycle == Model.EnumModel.TimeCycle.Month)
                {
                    DateTime tempTime = filter.TimeBegin.Value.Date.AddDays(1 - filter.TimeBegin.Value.Day);
                    while (tempTime <= filter.TimeEnd.Value.Date)
                    {
                        DateTime endTime = tempTime.AddMonths(1).AddDays(-1).AddHours(23).AddMinutes(59).AddSeconds(59);
                        model.AllLineChartData.xAxis.Add(tempTime.ToString("yyyy.MM.dd") + "-" + endTime.ToString("yyyy.MM.dd"));
                        foreach (var ser in model.AllLineChartData.series)
                        {
                            ser.data.Add(list.Where(m => m.AuditTime >= tempTime && m.AuditTime <= endTime && m.ReportFlag == ser.keyFilter).Sum(m => m.ChargeAmount ?? 0));
                        }
                        model.PersonalLineChartData.xAxis.Add(tempTime.ToString("yyyy.MM.dd") + "-" + endTime.ToString("yyyy.MM.dd"));
                        foreach (var ser in model.PersonalLineChartData.series)
                        {
                            ser.data.Add(list.Where(m => m.AuditTime >= tempTime && m.AuditTime <= endTime && m.CreateUser == ser.keyFilter).Sum(m => m.ChargeAmount ?? 0));
                        }
                        tempTime = tempTime.AddMonths(1);
                    }
                }
                if (filter.TimeCycle == Model.EnumModel.TimeCycle.Annual)
                {
                    DateTime tempTime = new DateTime(filter.TimeBegin.Value.Year, 1, 1);
                    while (tempTime <= filter.TimeEnd.Value.Date)
                    {
                        DateTime endTime = tempTime.AddYears(1).AddDays(-1).AddHours(23).AddMinutes(59).AddSeconds(59);
                        model.AllLineChartData.xAxis.Add(tempTime.ToString("yyyy.MM.dd") + "-" + endTime.ToString("yyyy.MM.dd"));
                        foreach (var ser in model.AllLineChartData.series)
                        {
                            ser.data.Add(list.Where(m => m.AuditTime >= tempTime && m.AuditTime <= endTime && m.ReportFlag == ser.keyFilter).Sum(m => m.ChargeAmount ?? 0));
                        }
                        model.PersonalLineChartData.xAxis.Add(tempTime.ToString("yyyy.MM.dd") + "-" + endTime.ToString("yyyy.MM.dd"));
                        foreach (var ser in model.PersonalLineChartData.series)
                        {
                            ser.data.Add(list.Where(m => m.AuditTime >= tempTime && m.AuditTime <= endTime && m.CreateUser == ser.keyFilter).Sum(m => m.ChargeAmount ?? 0));
                        }
                        tempTime = tempTime.AddYears(1);
                    }
                }
                return model;
            }
        }

        public byte[] ExportPersonalAchievementsList(AchievementsFilter filter,bool IsAll=false)
        {
            using (DataProvider dp = new DataProvider())
            {
                var list = data.GetAchievementsList(dp, filter, out int total, false);
                Dictionary<string, string> dic = new Dictionary<string, string>
                {
                    { "Formal", "正式" },
                    { "PreAssessment", "预评" },
                    { "Consultation", "咨询" },

                    { "HouseReport", "房产报告" },
                    { "AreaReport", "土地报告" },
                    { "AssetsReport", "资产报告" }
                };

                Workbook wk = new Workbook();
                Worksheet ws = wk.Worksheets[0];
                ws.Name = "绩效考核";
                Cells cells = ws.Cells;
                cells[0, 0].PutValue("报告编号");
                cells[0, 1].PutValue("报告名称");
                cells[0, 2].PutValue("报告类型");
                cells[0, 3].PutValue("所属类别");
                cells[0, 4].PutValue("报告时间");
                cells[0, 5].PutValue("审核通过时间");
                cells[0, 6].PutValue("金额");
                if (IsAll)
                {
                    cells[0, 7].PutValue("所属人");
                }
                for (int i = 0; i < list.Count; i++)
                {
                    var model = list[i];
                    cells[i + 1, 0].PutValue(model.ReportCode);
                    cells[i + 1, 1].PutValue(model.ReportName);
                    cells[i + 1, 2].PutValue(dic[model.ReportType]);
                    cells[i + 1, 3].PutValue(dic[model.ReportFlag]);
                    cells[i + 1, 4].PutValue(model.SubmitTime.HasValue?model.SubmitTime.Value.ToString("yyyy-MM-dd HH:mm:ss"):"");
                    cells[i + 1, 5].PutValue(model.AuditTime.HasValue ? model.AuditTime.Value.ToString("yyyy-MM-dd HH:mm:ss") : "");
                    cells[i + 1, 6].PutValue(model.ChargeAmount);
                    if (IsAll)
                    {
                        cells[i + 1, 7].PutValue(model.CreateUserName);
                    }
                }
                using (MemoryStream ms = new MemoryStream())
                {
                    wk.Save(ms, SaveFormat.Xlsx);
                    return ms.ToArray();
                }
            }
        }
    }
}
