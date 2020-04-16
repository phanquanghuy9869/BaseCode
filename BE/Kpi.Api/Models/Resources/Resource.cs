using Kpi.Core.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Kpi.Api.Models.Resources
{
    public interface IResourceModel
    {
        int Id { get; set; }
    }

    public class CriterionCatalogRS
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
    }

    public class TaskRS
    {
        public int Id { get; set; }
        public int TaskIndex { get; set; }
        public string Task { get; set; }
        public string Expectation { get; set; }
        public Nullable<System.DateTime> AssignedDate { get; set; }
        public Nullable<System.DateTime> Deadline { get; set; }
        public string DeadlineStr { get; set; }
        public string Result { get; set; }
        public bool IsFinish { get; set; }
        public int KpiEvaluationId { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string CreatedByUser { get; set; }
        public bool IsUIDeleted { get; set; }

        public List<FileDTO> Files { get; set; }
    }

    public class KpiCriterionDetailRS
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

    public class KpiEvaluationRS : IResourceModel
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
        public int StatusId { get; set; }
        public string StatusName { get; set; }
        public System.DateTime KpiMonth { get; set; }
        public int KpiMonthNumber { get; set; }
        public Nullable<int> YearMonth { get; set; }
        public Nullable<int> EmpKpiPoint { get; set; }
        public string EmpKpiClassification { get; set; }
        public Nullable<int> Level1ManagerKpiPoint { get; set; }
        public string Level1ManagerKpiClassification { get; set; }
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
        public Nullable<int> HrKpiPoint { get; set; }
        public string HrKpiPointClassification { get; set; }
        public string HrKpiPointComment { get; set; }
        public string FinalKpiComment { get; set; }
        public string Organization { get; set; }
        public string OrganizationId { get; set; }
    }

    public class KpiRS : KpiEvaluationRS
    {
        public List<TaskRS> TaskList { get; set; }

        public List<KpiCriterionDetailRS> KpiCriterionDetailList { get; set; }

        public List<CriterionCatalogRS> CriterionCatalogList { get; set; }
    }

    public class KpiPeriodConfigRS : IResourceModel
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
    public class KpiPointEvaluationRS : IResourceModel
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

    public class DiaryCriterionDetailRS : IResourceModel
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
    }

    public class ViewEventDiaryConfigRS : IResourceModel
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
        public string Code { get; set; }
        public string IsDeleted { get; set; }
        public Nullable<int> Level1ManagerUserId { get; set; }
        public Nullable<int> Level2ManagerUserId { get; set; }
        public List<UserOrgDTO> UserList { get; set; }
        public Nullable<bool> IsActive { get; set; }

        public int ApplyDate { get; set; }
    }

    public class ViewUserEventDiaryRS : IResourceModel
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

    public class UserOrgRS : UserInfoModel, IResourceModel
    {
        public bool IsCreateUser { get; set; }
        public string Password { get; set; }
        //public string PhoneNumber { get; set; }
        public string SecretWord { get; set; }
        //public string Code { get; set; }
        public int? YearMonth { get; set; }
        public bool IsActived { get; set; }
    }

    public class EmploymentHistoryRS : IResourceModel
    {
        public int Id { get; set; }
    }


    public class EmpTransferRS : IResourceModel
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

    public class View_KpiEvaluation_OrganizationRS : IResourceModel
    {
        public int Id { get; set; }
    }

    public class PasswordResourceModel
    {
        [Required]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(255, ErrorMessage = "Mật khẩu phải có ít nhất 6 kí tự. ", MinimumLength = 6)]
        [RegularExpression(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{6,}$", ErrorMessage = "Mật khẩu phải có ít nhất 1 ký tự đặc biệt, 1 ký tự là chữ số và 1 ký tự viết hoa. ")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Xác nhận mật khẩu không trùng khớp. ")]
        public string ConfirmPassword { get; set; }
    }

    public class OrgViewModel : OrganizationDTO, IResourceModel
    {
    }
    
    public class KpiCriterionTypeViewModel : KpiCriterionTypeDTO, IResourceModel
    {
    }

    public class KpiCriterionCatalogViewModel : CriterionCatalogDTO, IResourceModel
    {
    }
    public class NotificationViewModel : NotificationDTO, IResourceModel
    {
    }

    public class EvoucherTypeViewModel : EvoucherTypeDTO, IResourceModel
    {
    }

    
    public class UserImportModel {
        public int OrgId { get; set; }
        public string OrgName { get; set; }
        public List<UserData> ImportData { get; set; }
    }

    public class UserData {
        public string UserFullName { get; set; }
        public string Code { get; set; }
        public DateTime? Dob { get; set; }
        public string Identity { get; set; }
        public string UserEmail { get; set; }
        public string PhoneNumber { get; set; }
    }
}