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
    
    public partial class PM_SysWork
    {
        public int Id { get; set; }
        public string WorkNo { get; set; }
        public string WorkName { get; set; }
        public string Principal { get; set; }
        public string WorkType { get; set; }
        public string ProjectLeader { get; set; }
        public string SpecialConsultant { get; set; }
        public System.DateTime CreateTime { get; set; }
        public System.Guid CreateUser { get; set; }
        public Nullable<System.DateTime> UpdateTime { get; set; }
        public Nullable<System.Guid> UpdateUser { get; set; }
        public bool IsDel { get; set; }
    }
}
