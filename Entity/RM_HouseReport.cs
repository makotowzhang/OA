//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Entity
{
    using System;
    using System.Collections.Generic;
    
    public partial class RM_HouseReport
    {
        public System.Guid Id { get; set; }
        public string ReportCode { get; set; }
        public string ReportName { get; set; }
        public string ReportEntruster { get; set; }
        public string ReportUser { get; set; }
        public string Obligee { get; set; }
        public string LocationAddress { get; set; }
        public string HouseNumber { get; set; }
        public Nullable<decimal> BuildArea { get; set; }
        public string LandNumber { get; set; }
        public Nullable<decimal> LandArea { get; set; }
        public Nullable<decimal> ValuationPrice { get; set; }
        public Nullable<decimal> ValuationValue { get; set; }
        public string ValuationPurpose { get; set; }
        public string ValuationStruct { get; set; }
        public Nullable<System.DateTime> ValueTime { get; set; }
        public Nullable<System.DateTime> ReportTime { get; set; }
        public string ReportDesc { get; set; }
        public string WorkName { get; set; }
        public string WorkDesc { get; set; }
        public string EnclosureName { get; set; }
        public string EnclusurePath { get; set; }
        public string Operator { get; set; }
        public string Salesman { get; set; }
        public string Contacts { get; set; }
        public string ContactsPhone { get; set; }
        public Nullable<decimal> ChargeAmount { get; set; }
        public bool ChargeStatus { get; set; }
        public bool IsInvoice { get; set; }
        public string ReportType { get; set; }
        public string ReportStatus { get; set; }
        public Nullable<System.Guid> AuditUser { get; set; }
        public string AuditReason { get; set; }
        public Nullable<System.DateTime> AuditTime { get; set; }
        public bool IsDel { get; set; }
        public System.Guid CreateUser { get; set; }
        public System.DateTime CreateTime { get; set; }
        public Nullable<System.DateTime> SubmitTime { get; set; }
        public Nullable<System.Guid> UpdateUser { get; set; }
        public Nullable<System.DateTime> UpdateTime { get; set; }
    }
}
