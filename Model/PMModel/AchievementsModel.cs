using Model.EnumModel;
using Model.SystemModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.PMModel
{
    public class AchievementsModel
    {
        public Guid Id { get; set; }
        public string ReportCode { get; set; }
        public string ReportName { get; set; }
        public decimal? ChargeAmount { get; set; }
        public string ReportType { get; set; }
        public string ReportStatus { get; set; }
        public Guid CreateUser { get; set; }
        public DateTime? SubmitTime { get; set; }
        public DateTime? AuditTime { get; set; }
        public bool ChargeStatus { get; set; }
        public string ReportFlag { get; set; }
    }

    public class AchievementsFilter : PageModel
    {
        public DateTime? TimeBegin { get; set; }

        public DateTime? TimeEnd { get; set; }

        public TimeCycle? TimeCycle { get; set; }

        public Guid? CreateUser { get; set; }
    }

    public class PieChart
    {
        public string name { get; set; }

        public decimal value { get; set; }
    }

    public class LineChart
    {
        public List<string> xAxis { get; set; } = new List<string>();

        public List<decimal> yAxis { get; set; } = new List<decimal>();
    }

    public class AchievementsChartData
    {
        public List<PieChart> PieChartData { get; set; } = new List<PieChart>();

        public LineChart LineChartData { get; set; } = new LineChart();
    }


}
