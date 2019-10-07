using Model.EnumModel;
using Model.SystemModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.CCModel
{
    public class EstimateModel
    {
        public Guid? Id { get; set; }
        public string ReportCode { get; set; }
        public string ReportName { get; set; }
        public string EntrustingParty { get; set; }
        public string EstimateAmount { get; set; }
        public string CostConsultantor { get; set; }
        public string CostChecker { get; set; }
        public DateTime? BZDate { get; set; }
        public string ReportDesc { get; set; }
        public string Operator { get; set; }
        public string Salesman { get; set; }
        public string Contacts { get; set; }
        public string ContactsPhone { get; set; }
        public decimal? ChargeAmount { get; set; }
        public bool ChargeStatus { get; set; }
        public bool IsInvoice { get; set; }
        public string ReportType { get; set; }
        public string ReportStatus { get; set; }
        public Guid? AuditUser { get; set; }
        public string AuditReason { get; set; }
        public string AuditFileName { get; set; }
        public string AuditFilePath { get; set; }
        public DateTime? AuditTime { get; set; }
        public bool IsDel { get; set; }
        public Guid CreateUser { get; set; }
        public System.DateTime CreateTime { get; set; }
        public DateTime? SubmitTime { get; set; }
        public Guid? UpdateUser { get; set; }
        public DateTime? UpdateTime { get; set; }

        public List<Guid> AuditDep { get; set; } = new List<Guid>();
        public List<Guid> AuditUserIds { get; set; } = new List<Guid>();

        public string CreateUserName { get; set; }

        public string AuditUserName { get; set; }

        public string CreateDepName { get; set; }
    }

    public class EstimateFilter : PageModel
    {
        public string ReportCode { get; set; }
        public string ReportName { get; set; }

        public List<bool> ChargeStatus { get; set; }
        public List<string> AuditStatus { get; set; }

        public string CreateUserName { get; set; }

        public string AuditUserName { get; set; }

        public Guid? CreateUserId { get; set; }

        public Guid? AuditUserId { get; set; }

        public DateTime? CreateBeginTime { get; set; }

        public DateTime? CreateEndTime { get; set; }

        public DateTime? AuditBeginTime { get; set; }

        public DateTime? AuditEndTime { get; set; }

        public ListType ListType { get; set; }
    }
}
