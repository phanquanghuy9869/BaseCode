using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kpi.Core.DTO
{
    public interface IEntityDTO
    {
        int Id { get; set; }

    }

    public class MenuDTO
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string Text { get; set; }
        public int DisplayOrder { get; set; }
        public int? ParrentID { get; set; }
        public string DirectoryPath { get; set; }
        public bool IsDeleted { get; set; }

        // external prop
        public bool HasPermission { get; set; }
        public string NameEnglish { get; set; }
    }

    public class CriterionCatalogDTO
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string CriterionTitle { get; set; }
        public int MinimumPoint { get; set; }
        public int MaximumPoint { get; set; }
        public Nullable<int> ParentId { get; set; }
        public bool IsFolder { get; set; }
        public Nullable<int> CategoryId { get; set; }
        public string CategoryName { get; set; }
        public Nullable<int> CriterionLevel { get; set; }
        public Nullable<bool> IsMinus { get; set; }
        public string ParentName { get; set; }
        public string CriterionTitleEn { get; set; }
    }

    public class TaskDTO
    {
        public int Id { get; set; }
        public int TaskIndex { get; set; }
        public string Task { get; set; }
        public string Expectation { get; set; }
        public Nullable<System.DateTime> AssignedDate { get; set; }
        public Nullable<System.DateTime> Deadline { get; set; }
        public string Result { get; set; }
        public bool IsFinish { get; set; }
        public int KpiEvaluationId { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string CreatedByUser { get; set; }
        public string DeadlineStr { get; set; }

        ///
        public bool IsUIDeleted { get; set; }
        public List<FileDTO> Files { get; set; }
    }

    public class KpiCriterionDetailDTO
    {
        public int Id { get; set; }
        public int CriterionIndex { get; set; }
        public string CriterionTitle { get; set; }
        public int MinimumPoint { get; set; }
        public int MaximumPoint { get; set; }
        public int EmployeeEvaluatePoint { get; set; }
        public string EmployeeEvaluateComment { get; set; }
        public Nullable<System.DateTime> EmployeeLastUpdatedDate { get; set; }
        public int ManagerEvaluatePoint { get; set; }
        public string ManagerEvaluateComment { get; set; }
        public Nullable<System.DateTime> ManagerLastUpdatedDate { get; set; }
        public Nullable<int> Month { get; set; }
        public Nullable<int> Year { get; set; }
        public string UserId { get; set; }
        public Nullable<int> OrgId { get; set; }
        public int KpiEvaluateId { get; set; }
        public int KpiCatalogId { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string CreatedByUser { get; set; }
        public string CriterionTitleEn { get; set; }
    }

    public class KpiEvaluationDTO
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public Nullable<int> EventDiaryId { get; set; }
        public string EmployeeName { get; set; }
        public int EmployeeJobId { get; set; }
        public string EmployeeJobTitle { get; set; }
        public string Level1ManagerUserId { get; set; }
        public int Level1ManagerOrgId { get; set; }
        public string Level1ManagerOrgName { get; set; }
        public Nullable<int> Level1ManagerJobTitleId { get; set; }
        public string Level1ManagerJobTitle { get; set; }
        public int StatusId { get; set; }
        public string StatusName { get; set; }
        public System.DateTime KpiMonth { get; set; }
        public int KpiMonthNumber { get; set; }
        public Nullable<int> YearMonth { get; set; }
        public Nullable<int> EmpKpiPoint { get; set; }
        public string EmpKpiClassification { get; set; }
        public Nullable<int> Level1ManagerKpiPoint { get; set; }
        public string Level1ManagerKpiClassification { get; set; }
        public Nullable<int> KpiPeriodConfigId { get; set; }
        public int CriterionType { get; set; }
        public string CriterionTypeName { get; set; }
        public Nullable<int> FinalKpiPoint { get; set; }
        public string FinalKpiClassification { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string CreatedByUser { get; set; }
        public Nullable<System.DateTime> ApproveByLevel1Manager { get; set; }
        public string ApproveByLevel1ManagerId { get; set; }
        public Nullable<System.DateTime> ApproveByLevel2Manager { get; set; }
        public string ApproveByLevel2ManagerId { get; set; }
        public string Organization { get; set; }

        public string Level2ManagerOrgName { get; set; }
        public string Level2ManagerJobTitle { get; set; }
        public Nullable<System.DateTime> SentHRDate { get; set; }
        public string SubmitNote { get; set; }

        public int OrganizationId { get; set; }

        public string ActionName { get; set; }

        public int ActionId { get; set; }
        public bool IsDeleted { get; set; }
        public string Level1ManagerUserName { get; set; }
        public string Level1ManagerFullName { get; set; }
        public string Level2ManagerUserName { get; set; }
        public string Level2ManagerFullName { get; set; }

        public Nullable<int> HrKpiPoint { get; set; }
        public string HrKpiPointClassification { get; set; }
        public string HrKpiPointComment { get; set; }
        public string FinalKpiComment { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public string UpdatedByUser { get; set; }
        public string StatusNameEn { get; set; }
        public string CriterionTypeNameEn { get; set; }
        public string OrganizationEn { get; set; }
        public string Level1ManagerJobTitleEn { get; set; }
        public string Level1ManagerOrgNameEn { get; set; }
        public string Level2ManagerJobTitleEn { get; set; }
        public string Level2ManagerOrgNameEn { get; set; }
        public string EmployeeJobTitleEn { get; set; }
    }

    public class KpiDTO : KpiEvaluationDTO
    {
        public List<TaskDTO> TaskList { get; set; }

        public List<KpiCriterionDetailDTO> KpiCriterionDetailList { get; set; }

        public List<CriterionCatalogDTO> CriterionCatalogList { get; set; }
    }

    public class KpiPointEvaluationDTO
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
        public int Expr3 { get; set; }
        public string Comment { get; set; }
    }

    public class DiaryCriterionDetailDTO
    {
        public int Id { get; set; }
        public int CriterionCatalogId { get; set; }
        public string CriterionCatalogCode { get; set; }
        public string CriterionCatalogName { get; set; }
        public Nullable<int> CriterionCatalogFolderId { get; set; }
        public string UserName { get; set; }
        public string UserFullName { get; set; }
        public Nullable<System.DateTime> CriterionDate { get; set; }
        public int CriterionDayOfMonth { get; set; }
        public int KpiPoint { get; set; }
        public Nullable<int> EventDiaryId { get; set; }
        public int KpiMonthNumber { get; set; }
        public string Comment { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string CreatedByUser { get; set; }
        public string CreatedByUserFullName { get; set; }
        public string CreatedByUserTitle { get; set; }
        public bool? IsDeleted { get; set; }
        public string CriterionCatalogNameEn { get; set; }
        public string CreatedByUserTitleEn { get; set; }
    }
    public class UnLockDiaryCriterionDTO
    {
        public long STT { get; set; }
        public string Organization { get; set; }
        public int OrganizationId { get; set; }
        public Nullable<int> YearMonth { get; set; }
        public string DirectoryPath { get; set; }
        public string StatusName { get; set; }
        public string Level1ManagerFullName { get; set; }
        public string Level2ManagerFullName { get; set; }
    }
    public class KpiPeriodConfigDTO
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public int MonthNumber { get; set; }
        public int YearMonth { get; set; }
        public System.DateTime FromDate { get; set; }
        public System.DateTime ToDate { get; set; }
        public string Note { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string CreatedByUser { get; set; }
        public bool IsActived { get; set; }
        public bool IsCreateEventDiary { get; set; }
        public List<int> CriterionTypes { get; set; }
        public Nullable<int> DayStart { get; set; }
        public Nullable<int> DayEnd { get; set; }
        public Nullable<int> DaySendEvalation { get; set; }
        public string PeriodConfig { get; set; }
    }

    public class UserDTO
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public Nullable<System.DateTime> LockoutEndDateUtc { get; set; }
        public bool LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class UserOrgDTO
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string UserFullName { get; set; }
        public string UserEmail { get; set; }
        public string PhoneNumber { get; set; }
        public int OrgId { get; set; }
        public string OrgName { get; set; }
        public string JobTitle { get; set; }
        public int? JobTitleId { get; set; }
        public string Level1ManagerUserName { get; set; }
        public string Level1ManagerFullName { get; set; }
        public string Level2ManagerUserName { get; set; }
        public string Level2ManagerFullName { get; set; }
        public Nullable<System.DateTime> FirstKpiDate { get; set; }
        public string Status { get; set; }
        public Nullable<bool> IsOrgManager { get; set; }
        public int? EventDiaryConfigId { get; set; }
        public int? Level1ManagerId { get; set; }
        public int? Level2ManagerId { get; set; }
        public string KpiType { get; set; }
        public DateTime? DOB { get; set; }
        public string Code { get; set; }
        public DateTime? StartWorkDate { get; set; }

        public int? CurrentKpi { get; set; }
        public int? CurrentKpiStatusId { get; set; }
        public DateTime? EDCUpdateDate { get; set; }
        public string IdCardNumber { get; set; }
        public Nullable<System.DateTime> IdCardDate { get; set; }
        public string IdCardLocation { get; set; }
        public int? CurrentEventDiaryId { get; set; }
        public int? FirstKpiDateYM { get; set; }
        public bool IsActived { get; set; }
    }

    public class Org_JobTitleDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string CreatedByUser { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public string UpdatedByUser { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<System.DateTime> DeletedDate { get; set; }
        public string DeletedByUser { get; set; }
        public string TitleEn { get; set; }
    }

    public class UserInfoModel : UserOrgDTO
    {
        public string UserId { get; set; }
        public int? Level1ManagerOrgId { get; set; }
        public string Level1ManagerJobTitle { get; set; }
        public string Level1ManagerOrgName { get; set; }
        public string Level2ManagerJobTitle { get; set; }
        public string Level2ManagerOrgName { get; set; }
        public int? Level1ManagerJobTileId { get; set; }
        public bool IsEmployee { get; set; }
        public bool IsEVoucherManager { get; set; }
        public bool IsBudgetDistributor { get; set; }
        public bool IsEmpManager { get; set; }
        public bool IsEVoucherDistributor { get; set; }
        public bool IsHasLogin { get; set; }
        public bool IsDistributorApprover { get; set; }
        public bool IsLevel2Manager { get; set; }
        public bool DivisionManager { get; set; }
        public int CriterionType { get; set; }
        public string CriterionTypeName { get; set; }
        public string CriterionTypeNameEn { get; set; }
    }

    public class EmploymentHistoryDTO
    {
        public DateTime? fromDate { get; set; }
        public DateTime? toDate { get; set; }
        public string orgName { get; set; }
        public string jobTitle { get; set; }

    }

    public class OrganizationDTO
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Nullable<int> OrganizationTypeID { get; set; }
        public string NodeID { get; set; }
        public string DirectoryPath { get; set; }
        public int? ParentId { get; set; }
        public string ParentName { get; set; }
        public string NameEn { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<int> NumberOrder { get; set; }
    }

    public class OrgInfoModelDTO : OrganizationDTO
    {
        public string ManagerFullName { get; set; }
        public string ManagerUserName { get; set; }
    }

    public class ViewEventDiaryConfigDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Nullable<int> OrgId { get; set; }
        public string OrgName { get; set; }
        public string Level1ManagerUserName { get; set; }
        public string Level1ManagerFullName { get; set; }
        public string Level1ManagerJobTitle { get; set; }
        public string Level1ManagerOrgName { get; set; }
        public string Level2ManagerUserName { get; set; }
        public string Level2ManagerFullName { get; set; }
        public string Level2ManagerJobTitle { get; set; }
        public string Level2ManagerOrgName { get; set; }
        public Nullable<System.DateTime> FromDate { get; set; }
        public Nullable<System.DateTime> ToDate { get; set; }
        public Nullable<int> YearMonth { get; set; }
    }

    public class ViewUserEventDiaryDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Nullable<int> OrgId { get; set; }
        public string OrgName { get; set; }
        public string Level1ManagerUserName { get; set; }
        public string Level1ManagerFullName { get; set; }
        public string Level1ManagerJobTitle { get; set; }
        public string Level1ManagerOrgName { get; set; }
        public string Level2ManagerUserName { get; set; }
        public string Level2ManagerFullName { get; set; }
        public string Level2ManagerJobTitle { get; set; }
        public string Level2ManagerOrgName { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string CreatedByUser { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public string UpdatedByUser { get; set; }
        public bool IsDeleted { get; set; }
        public string UserName { get; set; }
        public string UserFullName { get; set; }
        public string UserEmail { get; set; }
        public string JobTitle { get; set; }
        public Nullable<int> JobTitleId { get; set; }
        public string Status { get; set; }
    }

    public class EventDiaryDTO
    {
        public int Id { get; set; }
        public int OrgId { get; set; }
        public string OrgName { get; set; }
        public string Level1ManagerUserName { get; set; }
        public string Level1ManagerFullName { get; set; }
        //public string Level1ManagerUserId { get; set; }
        //public string Level1ManagerOrgName { get; set; }
        public string Level2ManagerUserName { get; set; }
        public string Level2ManagerFullName { get; set; }
        //public string Level2ManagerUserId { get; set; }
        //public string Level2ManagerOrgName { get; set; }
        public Nullable<System.DateTime> FromDate { get; set; }
        public Nullable<System.DateTime> ToDate { get; set; }
        public Nullable<int> YearMonth { get; set; }
        public int EventDiaryConfigId { get; set; }

        public List<DiaryCriterionDetailDTO> Details { get; set; }

        // #272
        public List<int> IdList { get; set; }
    }

    public class EventDiarySyncModelDTO : EventDiaryDTO
    {
        public string Code { get; set; }
    }

    public class PagingModelDTO
    {
        public IEnumerable<Object> Paging { get; set; }
        public int Count { get; set; }
    }

    public class EventDiaryConfigDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Nullable<int> OrgId { get; set; }
        public string OrgName { get; set; }
        public string Level1ManagerUserName { get; set; }
        public string Level1ManagerFullName { get; set; }
        public string Level1ManagerJobTitle { get; set; }
        public string Level1ManagerOrgName { get; set; }
        public string Level2ManagerUserName { get; set; }
        public string Level2ManagerFullName { get; set; }
        public string Level2ManagerJobTitle { get; set; }
        public string Level2ManagerOrgName { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string CreatedByUser { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public string UpdatedByUser { get; set; }
        public bool IsDeleted { get; set; }
        public string Code { get; set; }
        public Nullable<int> Level1ManagerUserId { get; set; }
        public Nullable<int> Level2ManagerUserId { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public List<UserOrgDTO> UserList { get; set; }
        public string OrgDirPath { get; set; }

        public int ApplyDate { get; set; }
        public string OrgNameEn { get; set; }
    }

    public class ProcessStatusDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int BussinessProcessID { get; set; }
        public Nullable<bool> IsFirstProccess { get; set; }
        public Nullable<bool> IsLastProccess { get; set; }
        public string TitleEn { get; set; }
    }

    public class BussinessApplicationDTO
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
    public class View_Statistics_ReportsDTO
    {
        public long STT { get; set; }
        public Nullable<int> TongSoNhanVien { get; set; }
        public Nullable<int> Tong { get; set; }
        public Nullable<int> AP { get; set; }
        public Nullable<int> A { get; set; }
        public Nullable<int> AM { get; set; }
        public Nullable<int> BP { get; set; }
        public Nullable<int> B { get; set; }
        public Nullable<int> BM { get; set; }
        public Nullable<int> C { get; set; }
        public int OrganizationId { get; set; }
        public string Organization { get; set; }
        public Nullable<int> YearMonth { get; set; }
        public string DirectoryPath { get; set; }
        public float APPercent { get; set; }
        public float APercent { get; set; }
        public float AMPercent { get; set; }
        public float BPPercent { get; set; }
        public float BPercent { get; set; }
        public float BMPercent { get; set; }
        public float CPercent { get; set; }
        public Nullable<int> ParentId { get; set; }
    }
    public class View_KpiEvaluation_OrganizationDTO
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public Nullable<int> EventDiaryId { get; set; }
        public string EmployeeName { get; set; }
        public int EmployeeJobId { get; set; }
        public string EmployeeJobTitle { get; set; }
        public string Level1ManagerUserId { get; set; }
        public int Level1ManagerOrgId { get; set; }
        public string Level1ManagerOrgName { get; set; }
        public Nullable<int> Level1ManagerJobTitleId { get; set; }
        public string Level1ManagerJobTitle { get; set; }
        public int StatusId { get; set; }
        public string StatusName { get; set; }
        public System.DateTime KpiMonth { get; set; }
        public int KpiMonthNumber { get; set; }
        public Nullable<int> YearMonth { get; set; }
        public Nullable<int> EmpKpiPoint { get; set; }
        public string EmpKpiClassification { get; set; }
        public Nullable<int> Level1ManagerKpiPoint { get; set; }
        public string Level1ManagerKpiClassification { get; set; }
        public Nullable<int> KpiPeriodConfigId { get; set; }
        public int CriterionType { get; set; }
        public string CriterionTypeName { get; set; }
        public Nullable<int> FinalKpiPoint { get; set; }
        public string FinalKpiClassification { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string CreatedByUser { get; set; }
        public Nullable<System.DateTime> ApproveByLevel1Manager { get; set; }
        public string ApproveByLevel1ManagerId { get; set; }
        public Nullable<System.DateTime> ApproveByLevel2Manager { get; set; }
        public string ApproveByLevel2ManagerId { get; set; }
        public string Organization { get; set; }
        public Nullable<int> OrganizationId { get; set; }
        public bool IsActived { get; set; }
        public bool IsDeleted { get; set; }
        public string Level1ManagerUserName { get; set; }
        public string Level1ManagerFullName { get; set; }
        public string Level2ManagerUserName { get; set; }
        public string Level2ManagerFullName { get; set; }
        public string Level2ManagerOrgName { get; set; }
        public string Level2ManagerJobTitle { get; set; }
        public Nullable<System.DateTime> SentHRDate { get; set; }
        public Nullable<int> EventDiaryConfigId { get; set; }
        public string UserId { get; set; }
        public Nullable<int> HrKpiPoint { get; set; }
        public string HrKpiPointClassification { get; set; }
        public string HrKpiPointComment { get; set; }
        public string FinalKpiComment { get; set; }
        public string SubmitNote { get; set; }
        public Nullable<int> ParentId { get; set; }
        public string DirectoryPath { get; set; }
        public int? ColSpan { get; set; }
        public int? No { get; set; }
        public string Code { get; set; }
        public string reportClassification { get; set; }
        public Nullable<int> reportPoint { get; set; }
        public Nullable<int> NumberOrder { get; set; }
        public Nullable<bool> IsActive { get; set; }
    }

    public static class KpiUserRole
    {
        public static readonly string Employee = "Employee";
        public static readonly string EVoucherManager = "EVoucherManager";
        public static readonly string Level2Manager = "Level2Manager";
        public static readonly string BudgetDistributor = "BudgetDistributor";
        public static readonly string EVoucherDistributor = "EVoucherDistributor";
        public static readonly string EmpManager = "EmpManager";
        public static readonly string DistributorApprover = "DistributorApprover";
        public static readonly string DivisionManager = "DivisionManager";
    }

    public class EmpTransferDTO
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public System.DateTime TransferDate { get; set; }
        public Nullable<int> UserId { get; set; }
        public Nullable<int> OldJobTitleId { get; set; }
        public Nullable<int> NewJobTitleId { get; set; }
        public Nullable<int> OldOrgId { get; set; }
        public Nullable<int> NewOrgId { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public Nullable<int> OldLevel1MngId { get; set; }
        public Nullable<int> NewLevel1MngId { get; set; }
        public Nullable<int> OldLevel2MngId { get; set; }
        public Nullable<int> NewLevel2MngId { get; set; }
        public string Note { get; set; }
        public Nullable<System.DateTime> FromDate { get; set; }
        public Nullable<System.DateTime> ToDate { get; set; }

        public string EmpFullName { get; set; }
        public string OldJobTitleName { get; set; }
        public string NewJobTitleName { get; set; }
        public string OldOrgName { get; set; }
        public string NewOrgName { get; set; }
        public string OldLv1MngName { get; set; }
        public string NewLv1MngName { get; set; }
        public string OldLv2MngName { get; set; }
        public string NewLv2MngName { get; set; }
    }

    public class DivMngPerUser
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string UserFullName { get; set; }
        public string OrgName { get; set; }
        public string JobTitle { get; set; }
        public List<DivMngPerDTO> Details { get; set; }
    }

    public class DivMngPerDTO
    {
        public int UserId { get; set; }
        public int OrgId { get; set; }
        public string OrgName { get; set; }
    }

    public class KpiCriterionTypeDTO
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public List<Kpi_CriterionTypeCatalogDTO> Catalogs { get; set; }
        public string NameEn { get; set; }
    }

    public class Kpi_CriterionTypeCatalogDTO
    {
        public int Id { get; set; }
        public Nullable<int> CriterionTypeId { get; set; }
        public Nullable<int> CriterionCatalogId { get; set; }
        public Nullable<int> StartPoint { get; set; }
        public string CatalogName { get; set; }
    }

    public class NotificationDTO
    {
        public int Id { get; set; }
        public Nullable<int> Status { get; set; }
        public string Notes { get; set; }
        public string Action { get; set; }
        public string FromUserName { get; set; }
        public string ToUserName { get; set; }
        public Nullable<System.DateTime> UserDate { get; set; }
        public Nullable<int> Type { get; set; }
        public string NameType { get; set; }
        public string Modules { get; set; }
        public string NotesEn { get; set; }
    }
    public class Notification_TypeDTO
    {
        public int Id { get; set; }
        public Nullable<int> Type { get; set; }
        public string TypeName { get; set; }
        public string ActionName { get; set; }
        public string ObjectName { get; set; }
    }

    public class OrganizationTypeDTO
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsCompany { get; set; }
    }

    public class EVoucherCodeDTO
    {
        public long Id { get; set; }
        public Nullable<int> CompanyId { get; set; }
        public string CompanyName { get; set; }
        public Nullable<int> Status { get; set; }
        public string StatusName { get; set; }
        public Nullable<decimal> Budget { get; set; }
        public Nullable<decimal> TotalValues { get; set; }
        public Nullable<System.DateTime> VoucheCodeDate { get; set; }
        public Nullable<int> VoucheCodeMonth { get; set; }
        public Nullable<int> VoucheCodeYear { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public string CreateUser { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public string UpdateUser { get; set; }
    }
}
