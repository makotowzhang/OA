using Model.EnumModel;
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

    public class AchievementsFilter
    {
        public DateTime? TimeBegin { get; set; }

        public DateTime? TimeEnd { get; set; }

        public TimeCycle TimeCycle { get; set; }
    }
}
