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
    
    public partial class Sys_KpiPeriodConfig
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public Nullable<int> MonthNumber { get; set; }
        public System.DateTime FromDate { get; set; }
        public System.DateTime ToDate { get; set; }
        public Nullable<int> YearMonth { get; set; }
        public string Note { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string CreatedByUser { get; set; }
        public Nullable<bool> IsActived { get; set; }
        public Nullable<bool> IsCreateEventDiary { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public Nullable<int> DayStart { get; set; }
        public Nullable<int> DayEnd { get; set; }
        public Nullable<int> DaySendEvalation { get; set; }
        public string PeriodConfig { get; set; }
    }
}
