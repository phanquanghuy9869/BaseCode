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
    
    public partial class Process_BussinessApplication
    {
        public int Id { get; set; }
        public int HandledByOrganization { get; set; }
        public string OrganizationName { get; set; }
        public Nullable<int> ObjectID { get; set; }
        public string ObjectTypeCode { get; set; }
        public string ObjectName { get; set; }
        public string HandledByUser { get; set; }
        public int HandledByUserTitle { get; set; }
        public string HandleByUserTitleName { get; set; }
        public string RegencyTitle { get; set; }
        public System.DateTime HandledDate { get; set; }
        public string ActionID { get; set; }
        public int BusinessTransitionID { get; set; }
        public int BeforeStatus { get; set; }
        public int AfterStatus { get; set; }
        public string Note { get; set; }
    }
}
