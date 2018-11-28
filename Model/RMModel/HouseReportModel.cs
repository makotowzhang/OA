using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.EnumModel;
using Model.SystemModel;

namespace Model.RMModel
{
    public class HouseReportModel:CommonModel
    {
        public Guid? Id { get; set; }
        public string ReportCode { get; set; }
        public string ReportName { get; set; }
        public string ReportEntruster { get; set; }
        public string ReportUser { get; set; }
        public string Obligee { get; set; }
        public string LocationAddress { get; set; }
        public string HouseNumber { get; set; }
        public decimal BuildArea { get; set; }
        public string LandNumber { get; set; }
        public decimal LandArea { get; set; }
        public decimal ValuationPrice { get; set; }
        public decimal ValuationValue { get; set; }
        public string ValuationPurpose { get; set; }
        public string ValuationStruct { get; set; }
        public DateTime? ValueTime { get; set; }
        public DateTime? ReportTime { get; set; }
        public string ReportDesc { get; set; }
        public string WorkName { get; set; }
        public string WorkDesc { get; set; }
        public string EnclosureName { get; set; }
        public string EnclusurePath { get; set; }
        public string Operator { get; set; }
        public string Salesman { get; set; }
        public string Contacts { get; set; }
        public string ContactsPhone { get; set; }
        public bool ChargeStatus { get; set; } = true;
        public decimal ChargeAmount { get; set; }
        public bool IsInvoice { get; set; } = true;
        public ReportType ReportType { get; set; } = ReportType.Formal;
        public ReportStatus ReportStatus { get; set; } = ReportStatus.WaitSubmit;
        public Guid? AuditUser { get; set; }
        public string AuditReason { get; set; }
        public DateTime? AuditTime { get; set; }
        public bool IsDel { get; set; }
        public DateTime? SubmitTime { get; set; }

        public List<Guid> ValuationObjective { get; set; }

        public List<Guid> ValuationMethods { get; set; }

        public List<Guid> SignAppraiser { get; set; }

        public List<Guid> AuditDep { get; set; }
        public List<Guid> AuditUserIds { get; set; }
    }

    public class HouseReportFilter : PageModel
    {

    }
}
