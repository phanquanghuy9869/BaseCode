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
    
    public partial class E_VoucherBudgetDenominations
    {
        public long Id { get; set; }
        public long VoucherBudgetId { get; set; }
        public string VoucherTypeName { get; set; }
        public string VoucherTypeCode { get; set; }
        public Nullable<decimal> Denominations { get; set; }
        public Nullable<int> CountNumber { get; set; }
        public Nullable<decimal> TotalValues { get; set; }
    }
}
