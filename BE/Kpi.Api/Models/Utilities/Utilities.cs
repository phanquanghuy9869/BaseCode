using System;
using System.Collections.Generic;

namespace Kpi.Api.Models.Utilities
{
    public interface IGridFilterModel
    {
        int Start { get; set; }
        int Length { get; set; }
    }

    public class BaseGridFilterModel : IGridFilterModel
    {
        public int Start { get; set; }
        public int Length { get; set; }
    }

    public class KpiGridFilterModel : BaseGridFilterModel
    {
        public string UserName { get; set; }
        public int YearMonth { get; set; }
        public int? orgId { get; set; }
        public List<int> OrgIds { get; set; }
        public List<int> StatusIds { get; set; }
        public string empName { get; set; }
        public string Level2ManagerUserName { get; set; }
        public string EmployeeName { get; set; }
        public string Level1ManagerName { get; set; }
        public string Level1ManagerUserName { get; set; }
        public string EmpUserName { get; set; }
        public string Note { get; set; }
    }

    public class EventDiaryFilterModel : BaseGridFilterModel
    {

    }

    public class KpiPeriodConfigGridFilterModel : BaseGridFilterModel
    {
        public string UserName { get; set; }
        public int YearMonth { get; set; }
        public int? orgId { get; set; }
        public string PeriodConfig { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

    }

    public class ViewEventDiaryConfigFilterModel : BaseGridFilterModel
    {
        public int? OrgId { get; set; }
        public int? Level1ManagerId { get; set; }
        public int? Level2ManagerId { get; set; }
        public string Username { get; set; }
    }

    public class UserOrgFilterModel : BaseGridFilterModel
    {
        public string UserName { get; set; }
        public string Name { get; set; }
        public int? orgId { get; set; }
        public string JobStatus { get; set; }
    }

    public class EmpTransferFilterModel : BaseGridFilterModel
    {
        public string EmpName { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }

    public class EmploymentHistoryFilterModel : BaseGridFilterModel
    {
        public int? UserId { get; set; }
    }

    public class OrgPagingFilterModel : BaseGridFilterModel
    {
        public string Code { get; set; }
        public string Name { get; set; }
    }
    
    public class KpiCriterionTypePaginFilterModel : BaseGridFilterModel
    {
        public string Code { get; set; }
        public string Name { get; set; }
    }

    public class KpiCriterionCatalogPaginFilterModel : BaseGridFilterModel
    {
        public string Code { get; set; }
        public string CriterionTitle { get; set; }
    }

    public class NotificationPaginFilterModel : BaseGridFilterModel
    {
        public int? Type { get; set; }
        public int? Status { get; set; }
        public string UserName { get; set; }
    }

    public class EvoucherTypePagingFilterModel : BaseGridFilterModel
    {
        public Nullable<int> CompanyId { get; set; }
        public int? Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string IsValidate { get; set; }
    }
}