using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kpi.Core.DTO
{
    public class EvoucherTypeDTO
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public Nullable<decimal> Denominations { get; set; }
        public Nullable<int> OderNumber { get; set; }
        public Nullable<bool> IsValidate { get; set; }
        public string Description { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string CreateUser { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public string UpdateUser { get; set; }
        public Nullable<int> CompanyId { get; set; }
        public string CompanyName { get; set; }
    }

    public class E_VoucherCodeDTO
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
    public class E_VoucherCodeDenominationsDTO
    {
        public long Id { get; set; }
        public Nullable<long> VoucherCodeId { get; set; }
        public string VoucherTypeName { get; set; }
        public string VoucherTypeCode { get; set; }
        public Nullable<decimal> Denominations { get; set; }
        public Nullable<int> CountNumber { get; set; }
        public Nullable<decimal> TotalValues { get; set; }
    }

    public class E_VoucherCodeLineDTO
    {
        public long Id { get; set; }
        public Nullable<long> VoucherCodeId { get; set; }
        public Nullable<long> EvoucheBudgetDetailId { get; set; }
        public string VoucherTypeCode { get; set; }
        public string VoucheTypeName { get; set; }
        public Nullable<decimal> Denominations { get; set; }
        public string Code { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public string CreateUser { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public string UpdateUser { get; set; }
        public Nullable<int> Status { get; set; }
        public string StatusName { get; set; }
        public Nullable<System.DateTime> UseDate { get; set; }
        public string Location { get; set; }
    }

    public class E_VoucherErrorDTO
    {
        public long Id { get; set; }
        public Nullable<long> VoucheBudgetId { get; set; }
        public Nullable<long> VoucheBudgetDetailId { get; set; }
        public string Description { get; set; }
        public Nullable<long> VoucheCodeId { get; set; }
        public Nullable<long> VoucheCodeLineId { get; set; }
    }

    public class E_VoucherBudgetDTO
    {
        public long Id { get; set; }
        public Nullable<int> CompanyId { get; set; }
        public string CompanyName { get; set; }
        public Nullable<int> Status { get; set; }
        public string StatusName { get; set; }
        public Nullable<int> TotalStaff { get; set; }
        public Nullable<int> StaffConfirm { get; set; }
        public Nullable<decimal> Budget { get; set; }
        public Nullable<decimal> TotalValues { get; set; }
        public Nullable<System.DateTime> BudgetDate { get; set; }
        public Nullable<int> BudgetMonth { get; set; }
        public Nullable<int> BudgetYear { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public string CreateUser { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public string UpdateUser { get; set; }
        public List<E_VoucherBudgetDetailDTO> BudgetDetails { get; set; }

        #region phan bo
        public List<E_VoucherBudgetDenominationsDTO> BudgetDenominations { get; set; }
        public decimal? DenomiTotalValues { get; set; }
        public int? DenomiTotalCount { get; set; }
        public int? TotalDistributedCount { get; set; }
        #endregion
    }

    public class E_VoucherBudgetDenominationsDTO
    {
        public long Id { get; set; }
        public long VoucherBudgetId { get; set; }
        public string VoucherTypeName { get; set; }
        public string VoucherTypeCode { get; set; }
        public Nullable<decimal> Denominations { get; set; }
        public Nullable<int> CountNumber { get; set; }
        public Nullable<decimal> TotalValues { get; set; }
    }

    public class E_VoucherBudgetDetailDTO : PagingData<E_VoucherBudgetDetailDTO>
    {
        public long Id { get; set; }
        public Nullable<long> VoucherBudgetId { get; set; }
        public string CodeUser { get; set; }
        public string NameUser { get; set; }
        public string JobTitle { get; set; }
        public string OrgCompanyName { get; set; }
        public Nullable<int> OrgCompanyId { get; set; }
        public Nullable<int> Status { get; set; }
        public string StatusName { get; set; }
        public Nullable<decimal> Budget { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string CreatedUser { get; set; }
        public Nullable<int> IsDeleted { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public string UpdateUser { get; set; }
        public string VoucheTypeName { get; set; }
        public string VoucheTypeCode { get; set; }
        public Nullable<decimal> Denominations { get; set; }
        public Nullable<int> CountNumberPage { get; set; }
        public Nullable<decimal> TotalValues { get; set; }
        public Nullable<int> OrderNo { get; set; }
        #region Distribute
        public List<E_VoucherBudgetDetailLineDTO> DistributeLines { get; set; }
        #endregion
    }

    public class E_VoucherBudgetDetailLineDTO
    {
        public long Id { get; set; }
        public string VoucherTypeName { get; set; }
        public string VoucherTypeCode { get; set; }
        public Nullable<decimal> Denominations { get; set; }
        public Nullable<int> CountNumberPage { get; set; }
        public Nullable<decimal> TotalValues { get; set; }
        public Nullable<int> VoucherBudgetDetailId { get; set; }
    }

    public class View_EVoucherUserDTO
    {
        public long Id { get; set; }
        public Nullable<long> VoucherCodeId { get; set; }
        public Nullable<long> EvoucheBudgetDetailId { get; set; }
        public string VoucherTypeCode { get; set; }
        public string VoucheTypeName { get; set; }
        public Nullable<decimal> Denominations { get; set; }
        public string Code { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public string CreateUser { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public string UpdateUser { get; set; }
        public Nullable<int> Status { get; set; }
        public string StatusName { get; set; }
        public Nullable<System.DateTime> UseDate { get; set; }
        public string Location { get; set; }
        public string CodeUser { get; set; }
        public string NameUser { get; set; }
    }
    public class PagingData<TDataType>
    {
        public long TotalRowCount { get; set; }
        public IEnumerable<TDataType> PageData { get; set; }
    }

    public class ImportDataResult
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public int TotalStaff { get; set; }
        public int StaffConfirm { get; set; }
        public decimal? Budget { get; set; }
        public DateTime? BudgetDate { get; set; }
        public decimal? TotalValue { get; set; }
        public string CompanyName { get; set; }
        public IEnumerable<ImportDataErrorRow> ErrorLines { get; set; }
    }

    public class ImportDataErrorRow
    {
        public string Message { get; set; }
        public E_VoucherBudgetDetailDTO Data { get; set; }
    }

    public class EVoucherImportModelDTO
    {
        public DateTime Time { get; set; }
        public int OrgId { get; set; }
        public string OrgName { get; set; }
        public int VoucherTypeId { get; set; }
        public string VoucherTypeCode { get; set; }
        public string VoucherTypeName { get; set; }
        public int TotalNumber { get; set; }
        public int Denominations { get; set; }
        public List<E_VoucherCodeLineDTO> ImportData { get; set; }
        public string CurrentUserName { get; set; }
    }
}
