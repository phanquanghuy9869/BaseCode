﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Kpi.DataAccess.DataContext
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class BRG_KpiEntities : DbContext
    {
        public BRG_KpiEntities()
            : base("name=BRG_KpiEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<AspNetRole> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        public virtual DbSet<DivMngPer> DivMngPers { get; set; }
        public virtual DbSet<E_VoucherBudget> E_VoucherBudget { get; set; }
        public virtual DbSet<E_VoucherBudgetDenominations> E_VoucherBudgetDenominations { get; set; }
        public virtual DbSet<E_VoucherBudgetDetail> E_VoucherBudgetDetail { get; set; }
        public virtual DbSet<E_VoucherBudgetDetailLine> E_VoucherBudgetDetailLine { get; set; }
        public virtual DbSet<E_VoucherCode> E_VoucherCode { get; set; }
        public virtual DbSet<E_VoucherCodeDenominations> E_VoucherCodeDenominations { get; set; }
        public virtual DbSet<E_VoucherCodeLine> E_VoucherCodeLine { get; set; }
        public virtual DbSet<E_VoucherError> E_VoucherError { get; set; }
        public virtual DbSet<E_VoucherType> E_VoucherType { get; set; }
        public virtual DbSet<EmpTransfer> EmpTransfers { get; set; }
        public virtual DbSet<Kpi_CriterionCatalog> Kpi_CriterionCatalog { get; set; }
        public virtual DbSet<Kpi_CriterionType> Kpi_CriterionType { get; set; }
        public virtual DbSet<Kpi_CriterionTypeCatalog> Kpi_CriterionTypeCatalog { get; set; }
        public virtual DbSet<Kpi_DiaryCriterionDetail> Kpi_DiaryCriterionDetail { get; set; }
        public virtual DbSet<Kpi_EventDiary> Kpi_EventDiary { get; set; }
        public virtual DbSet<Kpi_File> Kpi_File { get; set; }
        public virtual DbSet<Kpi_JobTitleCriterionType> Kpi_JobTitleCriterionType { get; set; }
        public virtual DbSet<Kpi_KpiCriterionDetail> Kpi_KpiCriterionDetail { get; set; }
        public virtual DbSet<Kpi_KpiEvaluation> Kpi_KpiEvaluation { get; set; }
        public virtual DbSet<Kpi_Task> Kpi_Task { get; set; }
        public virtual DbSet<Kpi_Task_File> Kpi_Task_File { get; set; }
        public virtual DbSet<Notification_Queue> Notification_Queue { get; set; }
        public virtual DbSet<Notification_Type> Notification_Type { get; set; }
        public virtual DbSet<Org_EmpTracking> Org_EmpTracking { get; set; }
        public virtual DbSet<Org_JobTitle> Org_JobTitle { get; set; }
        public virtual DbSet<Org_Organization> Org_Organization { get; set; }
        public virtual DbSet<Org_OrganizationType> Org_OrganizationType { get; set; }
        public virtual DbSet<Org_UserOrg> Org_UserOrg { get; set; }
        public virtual DbSet<Process_BussinessApplication> Process_BussinessApplication { get; set; }
        public virtual DbSet<Process_BussinessProcess> Process_BussinessProcess { get; set; }
        public virtual DbSet<Process_ProcessStatus> Process_ProcessStatus { get; set; }
        public virtual DbSet<Process_ProcessTransition> Process_ProcessTransition { get; set; }
        public virtual DbSet<RoleMenuPermission> RoleMenuPermissions { get; set; }
        public virtual DbSet<Sys_EventDiaryConfig> Sys_EventDiaryConfig { get; set; }
        public virtual DbSet<Sys_KpiPeriodConfig> Sys_KpiPeriodConfig { get; set; }
        public virtual DbSet<Sys_Menu> Sys_Menu { get; set; }
        public virtual DbSet<Sys_PeriodConfigCriterionType> Sys_PeriodConfigCriterionType { get; set; }
        public virtual DbSet<Sys_Sequence> Sys_Sequence { get; set; }
        public virtual DbSet<Process_Action> Process_Action { get; set; }
        public virtual DbSet<View_AspNetUserRoles> View_AspNetUserRoles { get; set; }
        public virtual DbSet<View_EventDiaryConfig> View_EventDiaryConfig { get; set; }
        public virtual DbSet<View_EventDiaryCriterion> View_EventDiaryCriterion { get; set; }
        public virtual DbSet<View_EVoucherUser> View_EVoucherUser { get; set; }
        public virtual DbSet<View_KpiEvaluation_Organization> View_KpiEvaluation_Organization { get; set; }
        public virtual DbSet<View_KpiPointEvaluation> View_KpiPointEvaluation { get; set; }
        public virtual DbSet<View_Statistics_Reports> View_Statistics_Reports { get; set; }
        public virtual DbSet<View_UnLockDiaryCriterion> View_UnLockDiaryCriterion { get; set; }
        public virtual DbSet<View_User_EventDiary> View_User_EventDiary { get; set; }
        public virtual DbSet<View_UserOrg_GetPaging> View_UserOrg_GetPaging { get; set; }
    }
}