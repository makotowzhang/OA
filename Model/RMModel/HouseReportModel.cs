using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.EnumModel;
using Model.SystemModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

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
        public decimal? BuildArea { get; set; }
        public string LandNumber { get; set; }
        public decimal? LandArea { get; set; }
        public decimal? ValuationPrice { get; set; }
        public decimal? ValuationValue { get; set; }
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
        public decimal? ChargeAmount { get; set; }
        public bool IsInvoice { get; set; } = true;
        [JsonConverter(typeof(StringEnumConverter))]
        public ReportType ReportType { get; set; } = ReportType.Formal;

        public string ReportTypeString { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public ReportStatus ReportStatus { get; set; } = ReportStatus.WaitSubmit;

        public string ReportStatusString { get; set; }
        public Guid? AuditUser { get; set; }
        public string AuditReason { get; set; }
        public DateTime? AuditTime { get; set; }
        public bool IsDel { get; set; }
        public DateTime? SubmitTime { get; set; }

        public List<Guid> ValuationObjective { get; set; } = new List<Guid>();

        public List<Guid> ValuationMethods { get; set; } = new List<Guid>();

        public List<Guid> SignAppraiser { get; set; } = new List<Guid>();

        public List<string> SignAppraiserName { get; set; } = new List<string>();

        public List<Guid> AuditDep { get; set; } = new List<Guid>();
        public List<Guid> AuditUserIds { get; set; } = new List<Guid>();

        public string CreateUserName { get; set; }

        public string AuditUserName { get; set; }

        public string CreateDepName { get; set; }
    }

    public class HouseReportFilter : PageModel
    {
        public List<string> ReportType { get; set; }

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
