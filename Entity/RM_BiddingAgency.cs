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
    
    public partial class RM_BiddingAgency
    {
        public System.Guid Id { get; set; }
        public string TenderType { get; set; }
        public string TenderWay { get; set; }
        public string AgentMan { get; set; }
        public string Salesman { get; set; }
        public string ProjectName { get; set; }
        public string EntrustingParty { get; set; }
        public string EPMan { get; set; }
        public string EPTel { get; set; }
        public Nullable<System.DateTime> OpenTime { get; set; }
        public string SignUpCount { get; set; }
        public string WinningUnit { get; set; }
        public string AuthorizedMan { get; set; }
        public string AuthorizedTel { get; set; }
        public string WinningAmount { get; set; }
        public string WinningM { get; set; }
        public Nullable<System.DateTime> WorkTime { get; set; }
        public string WorkDesc { get; set; }
        public string EnclosureName { get; set; }
        public string EnclusurePath { get; set; }
        public string RegFee { get; set; }
        public string RegFeeWay { get; set; }
        public string AgencyFee { get; set; }
        public string AgencyUnit { get; set; }
        public string AppraisalFee { get; set; }
        public string ChargeStatus { get; set; }
        public string InvoiceStatus { get; set; }
        public Nullable<System.Guid> CreateUser { get; set; }
        public Nullable<System.DateTime> CreateTime { get; set; }
        public Nullable<System.Guid> UpdateUser { get; set; }
        public Nullable<System.DateTime> UpdateTime { get; set; }
    }
}
