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
    
    public partial class PM_MaterielApply
    {
        public System.Guid Id { get; set; }
        public int MaterielId { get; set; }
        public int Quantity { get; set; }
        public string AuditStatus { get; set; }
        public string ApplyReason { get; set; }
        public Nullable<System.Guid> AuditUser { get; set; }
        public string AuditReason { get; set; }
        public Nullable<System.DateTime> AuditTime { get; set; }
        public bool IsDel { get; set; }
        public System.Guid CreateUser { get; set; }
        public System.DateTime CreateTime { get; set; }
        public Nullable<System.Guid> UpdateUser { get; set; }
        public Nullable<System.DateTime> UpdateTime { get; set; }
    }
}
