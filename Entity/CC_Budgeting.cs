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
    
    public partial class CC_Budgeting
    {
        public System.Guid Id { get; set; }
        public string ReportCode { get; set; }
        public string ReportName { get; set; }
        public string EntrustingParty { get; set; }
        public string BudgetAmount { get; set; }
        public string CostConsultantor { get; set; }
        public string CostChecker { get; set; }
        public Nullable<System.DateTime> BZDate { get; set; }
        public string ReportDesc { get; set; }
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
        public string AuditFileName { get; set; }
        public string AuditFilePath { get; set; }
        public Nullable<System.DateTime> AuditTime { get; set; }
        public bool IsDel { get; set; }
        public System.Guid CreateUser { get; set; }
        public System.DateTime CreateTime { get; set; }
        public Nullable<System.DateTime> SubmitTime { get; set; }
        public Nullable<System.Guid> UpdateUser { get; set; }
        public Nullable<System.DateTime> UpdateTime { get; set; }
    }
}
