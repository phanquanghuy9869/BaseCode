//------------------------------------------------------------------------------
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
    using System.Collections.Generic;
    
    public partial class View_KpiPointEvaluation
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public Nullable<int> EventDiaryId { get; set; }
        public string EmployeeName { get; set; }
        public int EmployeeJobId { get; set; }
        public string EmployeeJobTitle { get; set; }
        public string Level1ManagerUserId { get; set; }
        public int Level1ManagerOrgId { get; set; }
        public string Level1ManagerOrgName { get; set; }
        public Nullable<int> Level1ManagerJobTitleId { get; set; }
        public int StatusId { get; set; }
        public string StatusName { get; set; }
        public System.DateTime KpiMonth { get; set; }
        public int KpiMonthNumber { get; set; }
        public Nullable<int> KpiPoint { get; set; }
        public string KpiClassification { get; set; }
        public Nullable<int> FinalKpiPoint { get; set; }
        public string FinalKpiClassification { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string CreatedByUser { get; set; }
        public Nullable<System.DateTime> ApproveByLevel1Manager { get; set; }
        public string ApproveByLevel1ManagerId { get; set; }
        public Nullable<System.DateTime> ApproveByLevel2Manager { get; set; }
        public string ApproveByLevel2ManagerId { get; set; }
        public bool IsActived { get; set; }
        public bool IsDeleted { get; set; }
        public int dairyPoint { get; set; }
        public int CriterionCatalogId { get; set; }
        public string Comment { get; set; }
        public int KpiEvaluateId { get; set; }
    }
}
