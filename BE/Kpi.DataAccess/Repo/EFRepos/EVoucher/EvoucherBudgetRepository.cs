using System;
using System.Collections.Generic;
using System.Linq;
using Kpi.Core.DTO;
using Kpi.DataAccess.DataContext;
using Kpi.DataAccess.Repo.Base;
using System.Data.Entity;
using System.Linq.Expressions;
using Kpi.Core.Helper;

namespace Kpi.DataAccess.Repo.EFRepos.EVoucher
{
    public interface IEvoucherBudgetRepository
    {
        PagingData<E_VoucherBudgetDetailDTO> GetDetailsPaging(EvoucherBudgetDetailPagingFilterModelDTO pagingModel);
        PagingData<E_VoucherBudgetDTO> Find(EvoucherBudgetDetailPagingFilterModelDTO filter);
        IEnumerable<OrganizationDTO> GetCompanies();
        ImportDataResult AddVoucherBudget(E_VoucherBudgetDTO model);
        E_VoucherBudgetDTO FindById(EvoucherBudgetDetailPagingFilterModelDTO filter);
        string FinishBudget(E_VoucherBudgetDTO model);
    }

    public class EvoucherBudgetRepository : BaseDataRepository<E_VoucherBudget>, IEvoucherBudgetRepository
    {
        private readonly IGenericRepository<E_VoucherBudget> _repo;
        private readonly IGenericRepository<E_VoucherBudgetDetail> _detailRepo;
        private readonly IGenericRepository<Org_Organization> _orgRepo;
        private readonly IGenericRepository<Org_UserOrg> _userOrgRepo;

        public EvoucherBudgetRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _repo = _unitOfWork.GetDataRepository<E_VoucherBudget>();
            _detailRepo = _unitOfWork.GetDataRepository<E_VoucherBudgetDetail>();
            _orgRepo = _unitOfWork.GetDataRepository<Org_Organization>();
            _userOrgRepo = _unitOfWork.GetDataRepository<Org_UserOrg>();
        }

        public ImportDataResult AddVoucherBudget(E_VoucherBudgetDTO model)
        {
            using (var ts = this._unitOfWork.BeginTransaction())
            {
                try
                {
                    var totalBudget = model.BudgetDetails.Sum(x => x.Budget);
                    var totalEmp = model.BudgetDetails.Count();
                    // kiem tra cong ty co ton tai va hop le
                    var comp = this._orgRepo.SelectWhere(x => x.Id == model.CompanyId && x.IsActive != null && x.IsActive.Value).FirstOrDefault();
                    if (comp == null)
                    {
                        return new ImportDataResult()
                        {
                            IsSuccess = false,
                            Message = "Công ty không tồn tại hoặc không hợp lệ",
                            Budget = totalBudget,
                            BudgetDate = model.BudgetDate,
                            TotalValue = totalBudget,
                            StaffConfirm = totalEmp,
                            TotalStaff = totalEmp,
                            CompanyName = comp.Name
                        };
                    }

                    // lay danh sach nhan vien loi
                    var errors = GetErrors(model.BudgetDetails);
                    if (errors != null && errors.Count > 0)
                    {
                        return new ImportDataResult()
                        {
                            ErrorLines = errors,
                            IsSuccess = false,
                            Budget = totalBudget,
                            BudgetDate = model.BudgetDate,
                            TotalValue = totalBudget,
                            StaffConfirm = totalEmp,
                            TotalStaff = totalEmp,
                            CompanyName = comp.Name
                        };
                    }

                    // xoa du lieu cu theo cong ty + ngay
                    // lai chi lay theo ngay bo thoi gian
                    model.BudgetDate = model.BudgetDate.Value.Date;
                    var budget = this._repo.SelectWhere(x => x.CompanyId == model.CompanyId && x.BudgetDate == model.BudgetDate).FirstOrDefault();
                    if (budget != null)
                    {
                        // da hoan thanh nhap
                        if (budget.Status != (int?)BudgetStatus.Created)
                        {
                            return new ImportDataResult()
                            {
                                IsSuccess = false,
                                Message = "Ngân sách của công ty ngày này đã hoàn thành nhập",
                                Budget = totalBudget,
                                BudgetDate = model.BudgetDate,
                                TotalValue = totalBudget,
                                StaffConfirm = totalEmp,
                                TotalStaff = totalEmp,
                                CompanyName = comp.Name
                            };
                        }

                        _repo.Delete(budget);
                        var budgetDetails = _detailRepo.SelectWhere(x => x.VoucherBudgetId == budget.Id).ToList();
                        if (budgetDetails != null && budgetDetails.Count > 0)
                        {
                            foreach (var item in budgetDetails)
                            {
                                _detailRepo.Delete(item);
                            }
                        }
                        _unitOfWork.SaveChanges();
                    }

                    // tao budget
                    var entity = new E_VoucherBudget()
                    {
                        Budget = totalBudget,
                        TotalValues = totalBudget,
                        BudgetDate = model.BudgetDate,
                        BudgetYear = model.BudgetDate.Value.Year,
                        BudgetMonth = model.BudgetDate.Value.Month,
                        CompanyId = model.CompanyId,
                        CompanyName = comp.Name,
                        CreateDate = DateTime.Now,
                        StaffConfirm = totalEmp,
                        TotalStaff = totalEmp,
                        Status = (int?)BudgetStatus.Created,
                        StatusName = "Đã nhập"
                    };
                    var result = _repo.Add(entity);
                    _unitOfWork.SaveChanges();

                    // tao budget details
                    if (result != null)
                    {
                        foreach (var item in model.BudgetDetails)
                        {
                            var bDetail = new E_VoucherBudgetDetail()
                            {
                                VoucherBudgetId = result.Id,
                                Budget = item.Budget,
                                CodeUser = item.CodeUser,
                                CreatedUser = model.CreateUser,
                                CreatedDate = DateTime.Now,
                                NameUser = item.NameUser,
                                OrgCompanyId = model.CompanyId,
                                OrgCompanyName = comp.Name,
                                JobTitle = item.JobTitle,
                                Status = (int?)BudgetStatus.Created,
                                StatusName = "Đã nhập",
                                OrderNo = item.OrderNo
                            };
                            _detailRepo.Add(bDetail);
                            _unitOfWork.SaveChanges();
                        }
                    }

                    ts.Commit();

                    return new ImportDataResult()
                    {
                        IsSuccess = true,
                        Budget = totalBudget,
                        BudgetDate = model.BudgetDate,
                        TotalValue = totalBudget,
                        StaffConfirm = totalEmp,
                        TotalStaff = totalEmp,
                        CompanyName = comp.Name
                    };
                }
                catch (Exception ex)
                {
                    return new ImportDataResult()
                    {
                        IsSuccess = false,
                        Message = ex.ToString()
                    };
                }
            }
        }

        public List<ImportDataErrorRow> GetErrors(List<E_VoucherBudgetDetailDTO> detailLines)
        {
            List<ImportDataErrorRow> res = new List<ImportDataErrorRow>();
            foreach (var item in detailLines)
            {
                var user = this._userOrgRepo.SelectWhere(x => x.UserName == item.CodeUser).FirstOrDefault();
                if (user == null)
                {
                    res.Add(new ImportDataErrorRow()
                    {
                        Message = "Tên đăng nhập (SĐT) không tồn tại",
                        Data = item
                    });
                }
                else
                {
                    item.NameUser = user.UserFullName;
                }
            }
            return res;
        }

        public PagingData<E_VoucherBudgetDTO> Find(EvoucherBudgetDetailPagingFilterModelDTO filter)
        {
            var query = _repo.SelectWhere(x =>
            ((x.BudgetDate.HasValue && filter.DateFrom.HasValue && DbFunctions.TruncateTime(x.BudgetDate.Value) >= DbFunctions.TruncateTime(filter.DateFrom.Value))
                || !filter.DateFrom.HasValue
            )
            && ((x.BudgetDate.HasValue && filter.DateFrom.HasValue && DbFunctions.TruncateTime(x.BudgetDate.Value) <= DbFunctions.TruncateTime(filter.DateTo.Value))
                || !filter.DateTo.HasValue
            )
            && x.CompanyId == filter.CompanyId
            && ((filter.Status != null && x.Status == filter.Status) || filter.Status == null)
            );

            var res = query.OrderBy(x => x.Id).Skip(filter.Start - 1).Take(filter.Length)
                .Select(x => new E_VoucherBudgetDTO
                {
                    Budget = x.Budget,
                    BudgetDate = x.BudgetDate,
                    CompanyId = x.CompanyId,
                    StaffConfirm = x.StaffConfirm,
                    TotalStaff = x.TotalStaff,
                    Status = x.Status,
                    StatusName = x.StatusName,
                    Id = x.Id,
                    TotalValues = x.TotalValues,
                    CompanyName = x.CompanyName,
                    CreateDate = x.CreateDate,
                    CreateUser = x.CreateUser
                }).ToList();
            return new PagingData<E_VoucherBudgetDTO>() { PageData = res, TotalRowCount = (long)query.Count() };
        }

        public IEnumerable<OrganizationDTO> GetCompanies()
        {
            var query = _orgRepo.SelectWhere(x => x.IsActive.HasValue && x.IsActive.Value).Select(x => new OrganizationDTO
            {
                Description = x.Description,
                Id = x.Id,
                Name = x.Name,
                OrganizationTypeID = x.OrganizationTypeID,
                Code = x.Code,
            }).ToList();
            return query;
        }

        public PagingData<E_VoucherBudgetDetailDTO> GetDetailsPaging(EvoucherBudgetDetailPagingFilterModelDTO filter)
        {
            var query = _detailRepo.SelectWhere(x => x.VoucherBudgetId == filter.EvoucherBudgetId && x.OrgCompanyId == filter.CompanyId);

            var res = query.OrderBy(x => x.Id).Skip(filter.Start - 1).Take(filter.Length)
             .Select(x => new E_VoucherBudgetDetailDTO
             {
                 Budget = x.Budget,
                 CodeUser = x.CodeUser,
                 NameUser = x.NameUser,
                 OrgCompanyId = x.OrgCompanyId,
                 OrgCompanyName = x.OrgCompanyName,
                 Status = x.Status,
                 StatusName = x.StatusName,
                 Id = x.Id,
                 JobTitle = x.JobTitle,
                 CreatedDate = x.CreatedDate,
                 CreatedUser = x.CreatedUser
             }).ToList();
            return new PagingData<E_VoucherBudgetDetailDTO>() { PageData = res, TotalRowCount = (long)query.Count() };
        }

        public E_VoucherBudgetDTO FindById(EvoucherBudgetDetailPagingFilterModelDTO filter)
        {
            var query = _repo.SelectWhere(x => x.Id == filter.EvoucherBudgetId && x.CompanyId == filter.CompanyId).Select(x => new E_VoucherBudgetDTO
            {
                Budget = x.Budget,
                BudgetDate = x.BudgetDate,
                CompanyId = x.CompanyId,
                StaffConfirm = x.StaffConfirm,
                TotalStaff = x.TotalStaff,
                Status = x.Status,
                StatusName = x.StatusName,
                Id = x.Id,
                TotalValues = x.TotalValues,
                CompanyName = x.CompanyName,
                CreateDate = x.CreateDate,
                CreateUser = x.CreateUser
            }).FirstOrDefault();
            return query;
        }

        public string FinishBudget(E_VoucherBudgetDTO model)
        {
            return UpdateBudgetStatus(model, BudgetStatus.CreatedCompleted, "Đã hoàn thành nhập");
        }

        public string UpdateBudgetStatus(E_VoucherBudgetDTO model, BudgetStatus status, string statusName)
        {
            using (var ts = this._unitOfWork.BeginTransaction())
            {
                try
                {
                    var budget = this._repo.SelectWhere(x => x.Id == model.Id && x.CompanyId == model.CompanyId).FirstOrDefault();
                    if (budget != null)
                    {
                        #region kiem tra cac trang thai
                        if (status == BudgetStatus.CreatedCompleted)// hoan thanh nhap ngan sach
                        {
                            if (budget.Status != (int?)BudgetStatus.Created)
                            {
                                return "Lỗi: Không thể hoàn thành nhập ngân sách có trạng thái không phải 'Đã nhập'.";
                            }
                        }
                        else if (status == BudgetStatus.DistributedCompleted) // hoan thanh phan bo
                        {
                            if (budget.Status != (int?)BudgetStatus.Distributed)
                            {
                                return "Lỗi: Không thể hoàn thành phân bổ ngân sách có trạng thái không phải 'Đã phân bổ'.";
                            }
                        }
                        else if (status == BudgetStatus.Approved) // duyet phan bo
                        {
                            if (budget.Status != (int?)BudgetStatus.DistributedCompleted)
                            {
                                return "Lỗi: Không thể duyệt phân bổ ngân sách có trạng thái không phải 'Hoàn thành phân bổ'.";
                            }
                        }
                        else if (status == BudgetStatus.Distributed) //  tra lai
                        {
                            if (budget.Status != (int?)BudgetStatus.DistributedCompleted)
                            {
                                return "Lỗi: Không thể trả lại phân bổ ngân sách có trạng thái không phải 'Đã hoàn thành phân bổ'.";
                            }
                        }
                        #endregion

                        // update trang thai budget
                        var properties = new List<Expression<Func<E_VoucherBudget, object>>>
                        {
                            (x => x.Status),
                            (x => x.StatusName),
                            (x => x.UpdateUser),
                            (x => x.UpdateDate),
                        };
                        budget.Status = (int?)status;
                        budget.StatusName = statusName;
                        budget.UpdateUser = model.UpdateUser;
                        budget.UpdateDate = DateTime.Now;

                        this._repo.UpdateByProperties(budget, properties);
                        _unitOfWork.SaveChanges();


                        var budgetDetails = _detailRepo.SelectWhere(x => x.VoucherBudgetId == budget.Id).ToList();
                        if (budgetDetails != null && budgetDetails.Count > 0)
                        {
                            foreach (var item in budgetDetails)
                            { // update trang thai budget
                                var properties1 = new List<Expression<Func<E_VoucherBudgetDetail, object>>>
                                {
                                    (x => x.Status),
                                    (x => x.StatusName),
                                    (x => x.UpdateUser),
                                    (x => x.UpdateDate),
                                };
                                item.Status = (int?)status;
                                item.StatusName = statusName;
                                item.UpdateUser = model.UpdateUser;
                                item.UpdateDate = DateTime.Now;

                                this._detailRepo.UpdateByProperties(item, properties1);
                                _unitOfWork.SaveChanges();
                            }
                        }
                        ts.Commit();
                    }
                }
                catch (Exception ex)
                {
                    FileLog.WriteLog(ex.ToString());
                    return ex.Message;
                }
            }
            return "";
        }
    }

    public enum BudgetStatus
    {
        Created = 0,
        CreatedCompleted = 1,
        Distributed = 2,
        DistributedCompleted = 3,
        Approved = 4,
        CodeAssigned = 5
    }
}
