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
    
    public partial class PM_DataLibrary
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string FileExtension { get; set; }
        public string FileType { get; set; }
        public string FilePath { get; set; }
        public bool CanPreview { get; set; }
        public string PreviewPath { get; set; }
        public bool IsDel { get; set; }
        public System.Guid CreateUser { get; set; }
        public System.DateTime CreateTime { get; set; }
        public Nullable<System.Guid> UpdateUser { get; set; }
        public Nullable<System.DateTime> UpdateTime { get; set; }
        public Nullable<System.Guid> FileClassify { get; set; }
    }
}
