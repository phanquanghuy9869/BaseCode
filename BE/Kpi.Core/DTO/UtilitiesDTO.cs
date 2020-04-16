using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kpi.Core.DTO
{
    public interface IGridFilterModelDTO
    {
        int Start { get; set; }
        int Length { get; set; }
    }

    public class RespondData
    {
        public bool IsSuccess { get; set; }
        public string Status { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
    }

    public class BaseGridFilterModelDTO : IGridFilterModelDTO
    {
        public int Start { get; set; }
        public int Length { get; set; }
    }

    public class KpiPeriodDTO
    {
        public List<int> CriterionType { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }

    public class MenuFilterModelDTO : BaseGridFilterModelDTO
    { }

    public class KpiEvaluationFilterModelDTO : BaseGridFilterModelDTO
    { }

    public class KpiFilterModelDTO : BaseGridFilterModelDTO
    {
        public string UserName { get; set; }
        public int YearMonth { get; set; }
        public int? orgId { get; set; }
        public List<int> StatusIds { get; set; }

        // huypq modified 11/12/19, với HR thì cần list cả các org con
        public List<int> OrgIds { get; set; }
        public string EmployeeName { get; set; }
        public string Level1ManagerName { get; set; }
        public string Level2ManagerUserName { get; set; }
        public string Level1ManagerUserName { get; set; }
        public string EmpUserName { get; set; }
        public string Note { get; set; }
    }

    public class EventDiaryFilterModelDTO : BaseGridFilterModelDTO
    {
        public int OrgId { get; set; }
        public int YearMonth { get; set; }
        public string Level1ManagerUsername { get; set; }
    }

    public class ManagerKpiFilterModelDTO : BaseGridFilterModelDTO
    {
        public int YearMonth { get; set; }
        public string FullName { get; set; }
        public string Level1ManagerUserName { get; set; }
        public int OrgId { get; set; }
        public string empName { get; set; }
        public string Level2ManagerUserName { get; set; }
        public List<int> StatusIds { get; set; }
        public List<int> OrgIds { get; set; }
        public bool IsVip { get; set; }
    }
    public class DivisionManagerKpiFilter : ManagerKpiFilterModelDTO
    {
        public string UserName { get; set; }
        public bool IsVip { get; set; }
    }

    public class UncompeletedKpiFilterModelDTO
    {
        public string Username { get; set; }
        public int YearMonth { get; set; }
        public List<int> OrgIds { get; set; }
        //public string OrgPath { get; set; }
        public int ActionId { get; set; }
    }

    public class OrgInfoModelFilterModelDTO : UserOrgFilterModelDTO
    {
        public string PhoneNumber { get; set; }
    }

    public class ViewUserEventDiaryFilterModelDTO : BaseGridFilterModelDTO
    { }
    public class KpiPeriodConfigFilterModelDTO : BaseGridFilterModelDTO
    {
        public string PeriodConfig { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }

    public class ViewEventDiaryConfigFilterModelDTO : BaseGridFilterModelDTO
    {
        public int? OrgId { get; set; }
        public int? Level1ManagerId { get; set; }
        public int? Level2ManagerId { get; set; }
        public string Username { get; set; }
    }

    public class DiaryCriterionDetailFilterModelDTO : BaseGridFilterModelDTO
    { }

    public class KpiPointEvaluationFilterModelDTO : BaseGridFilterModelDTO
    { }

    public class UserOrgFilterModelDTO : BaseGridFilterModelDTO
    {
        public string UserName { get; set; }
        public string Name { get; set; }
        public int? orgId { get; set; }
        public string JobStatus { get; set; }
    }

    public class OrgPagingFilterModelDTO : BaseGridFilterModelDTO
    {
        public string Code { get; set; }
        public string Name { get; set; }
    }

    public class ProcessKpiDTO
    {
        public int KpiId { get; set; }
        public int ActionId { get; set; }
    }

    public class View_KpiEvaluation_OrganizationFilterModelDTO : BaseGridFilterModelDTO
    {
        public int? orgId { get; set; }
        public int? yearMonth { get; set; }
        public int? status { get; set; }
        public string newLevel1MngUserName { get; set; }
        public string newLevel2MngUserName { get; set; }
        public string directoryPath { get; set; }
        public string submitNote { get; set; }
    }

    public class View_Statistics_ReportsFilterModelDTO : BaseGridFilterModelDTO
    {
        public int? orgId { get; set; }
        public int? yearMonth { get; set; }
        public string directoryPath { get; set; }
        public Boolean checkOrg { get; set; }
    }

    public class UnLockDiaryCriterionFilterModelDTO : BaseGridFilterModelDTO
    {
        public int? orgId { get; set; }
        public int? yearMonth { get; set; }
        public string directoryPath { get; set; }
    }

    public class UpdateKPIByYearMonthEventFilter
    {
        public int? EventDiaryConfigId { get; set; }
        public int? YearMonth { get; set; }
    }

    public class EventDiaryPagingModelDTO
    {
        public List<EventDiaryDTO> Paging { get; set; }
        public int Count { get; set; }
    }

    public class EmpTransferFilterModelDTO : BaseGridFilterModelDTO
    {
        public string EmpName { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }

    public class EmploymentHistoryFilterModelDTO : BaseGridFilterModelDTO
    {
        public int? UserId { get; set; }
    }

    public class KpiCriterionTypePaginFilterModelDTO : BaseGridFilterModelDTO
    {
        public string Code { get; set; }
        public string Name { get; set; }
    }

    public class KpiCriterionCatalogPaginFilterModelDTO : BaseGridFilterModelDTO
    {
        public string Code { get; set; }
        public string CriterionTitle { get; set; }
    }

    public class NotificationPaginFilterModelDTO : BaseGridFilterModelDTO
    {
        public int? Id { get; set; }
        public int? Type { get; set; }
        public int? Status { get; set; }
        public string UserName { get; set; }
    }

    public class TaskFilterRespondDTO
    {
        public IEnumerable<KpiDTO> NoTaskKpiList { get; set; }
        public IEnumerable<KpiDTO> TaskKpiList { get; set; }
    }

    public class FileDTO
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
        public int? FileSize { get; set; }
        public string FilePath { get; set; }
        public bool IsRecentlyCreated { get; set; }
        public bool IsRecentlyDeleted { get; set; }
    }

    public class FileDataDTO
    {
        public FileStream FileStream { get; set; }
        public string FileName { get; set; }
        public string Extension { get; set; }
        public string MimeType { get; set; }
    }

    public class EvoucherTypePagingFilterModelDTO : BaseGridFilterModelDTO
    {

        public Nullable<int> CompanyId { get; set; }
        public int? Id { get; set; }
        public string IsValidate { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
    }

    public class EvoucherBudgetDetailPagingFilterModelDTO : BaseGridFilterModelDTO
    {
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public int? CompanyId { get; set; }
        public long? EvoucherBudgetId { get; set; }
        public long? EvoucherBudgetDetailId { get; set; }
        public int? Status { get; set; }
    }

    public class EvoucherCodePagingFilterModelDTO : BaseGridFilterModelDTO
    {
        public string IsValidate { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
    }

    public class EvoucherCodeLinePagingFilterModelDTO : BaseGridFilterModelDTO
    {
        public string IsValidate { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public long EvoucheBudgetDetailId { get; set; }
        public long VoucherCodeId { get; set; }
        public int Status { get; set; }
    }

    public class View_EVoucherUserPagingFilterModelDTO : BaseGridFilterModelDTO
    {
        public string IsValidate { get; set; }
        public string CodeUser { get; set; }
        public string VoucherCode { get; set; }
        public string UserName { get; set; }
        public long EvoucheBudgetDetailId { get; set; }
        public long VoucherCodeId { get; set; }
        public int Status { get; set; }
        public int IsExpiryDate { get; set; }
    }
}
