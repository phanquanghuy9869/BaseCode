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
    
    public partial class View_EventDiaryCriterion
    {
        public int Id { get; set; }
        public int OrgId { get; set; }
        public string OrgName { get; set; }
        public string Level1ManagerUserName { get; set; }
        public string Level1ManagerFullName { get; set; }
        public string Level2ManagerUserName { get; set; }
        public string Level2ManagerFullName { get; set; }
        public Nullable<System.DateTime> FromDate { get; set; }
        public Nullable<System.DateTime> ToDate { get; set; }
        public Nullable<int> YearMonth { get; set; }
        public string CriterionCatalogCode { get; set; }
        public string CriterionCatalogName { get; set; }
        public string UserName { get; set; }
        public string UserFullName { get; set; }
        public int CriterionDayOfMonth { get; set; }
        public Nullable<System.DateTime> CriterionDate { get; set; }
    }
}
