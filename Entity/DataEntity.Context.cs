﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class DataProvider : DbContext
    {
        public DataProvider()
            : base("name=DataProvider")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<System_User> System_User { get; set; }
        public virtual DbSet<System_Role> System_Role { get; set; }
        public virtual DbSet<System_UserRole> System_UserRole { get; set; }
        public virtual DbSet<System_Menu> System_Menu { get; set; }
        public virtual DbSet<System_Authorize> System_Authorize { get; set; }
        public virtual DbSet<System_Log> System_Log { get; set; }
        public virtual DbSet<System_Message> System_Message { get; set; }
        public virtual DbSet<PM_DataLibrary> PM_DataLibrary { get; set; }
        public virtual DbSet<System_DicItem> System_DicItem { get; set; }
        public virtual DbSet<System_DicGroup> System_DicGroup { get; set; }
    }
}
