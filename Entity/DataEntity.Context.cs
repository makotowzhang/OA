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
        public virtual DbSet<System_DicGroup> System_DicGroup { get; set; }
        public virtual DbSet<System_DicItem> System_DicItem { get; set; }
        public virtual DbSet<PM_Employee> PM_Employee { get; set; }
        public virtual DbSet<PM_Department> PM_Department { get; set; }
        public virtual DbSet<PM_DataLibrary> PM_DataLibrary { get; set; }
        public virtual DbSet<PM_Materiel> PM_Materiel { get; set; }
        public virtual DbSet<PM_AuditUser> PM_AuditUser { get; set; }
        public virtual DbSet<PM_MaterielApply> PM_MaterielApply { get; set; }
        public virtual DbSet<PM_MoneyApply> PM_MoneyApply { get; set; }
        public virtual DbSet<PM_Notice> PM_Notice { get; set; }
        public virtual DbSet<PM_Lease> PM_Lease { get; set; }
        public virtual DbSet<PM_LeaseApply> PM_LeaseApply { get; set; }
        public virtual DbSet<RM_ReportAudit> RM_ReportAudit { get; set; }
        public virtual DbSet<RM_ReportDicItem> RM_ReportDicItem { get; set; }
        public virtual DbSet<RM_HouseReport> RM_HouseReport { get; set; }
        public virtual DbSet<RM_AreaReport> RM_AreaReport { get; set; }
        public virtual DbSet<RM_AssetsReport> RM_AssetsReport { get; set; }
        public virtual DbSet<PM_ThingsTodo> PM_ThingsTodo { get; set; }
        public virtual DbSet<View_Achievements> View_Achievements { get; set; }
        public virtual DbSet<System_Message> System_Message { get; set; }
        public virtual DbSet<System_MessageReceiver> System_MessageReceiver { get; set; }
        public virtual DbSet<PM_SysWork> PM_SysWork { get; set; }
        public virtual DbSet<CC_Budgeting> CC_Budgeting { get; set; }
        public virtual DbSet<CC_Estimate> CC_Estimate { get; set; }
        public virtual DbSet<CC_Settlement> CC_Settlement { get; set; }
        public virtual DbSet<CC_Investment> CC_Investment { get; set; }
        public virtual DbSet<CC_Appraisal> CC_Appraisal { get; set; }
        public virtual DbSet<RM_BiddingAgency> RM_BiddingAgency { get; set; }
    }
}
