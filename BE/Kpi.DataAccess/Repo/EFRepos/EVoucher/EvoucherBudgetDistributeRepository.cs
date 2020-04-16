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
    public interface IEvoucherBudgetDistributeRepository
    {
        PagingData<E_VoucherBudgetDTO> FindBudgetDistributes(EvoucherBudgetDetailPagingFilterModelDTO pagingModel);
        E_VoucherBudgetDTO GetBudgetDistributesById(EvoucherBudgetDetailPagingFilterModelDTO filter);
        string SaveCompanyBudgetDistributes(E_VoucherBudgetDTO filter);
        string SaveEmployeeBudgetDistributes(E_VoucherBudgetDetailDTO filter, int? companyId);
        E_VoucherBudgetDetailDTO GetEmployeeBudgetDist(EvoucherBudgetDetailPagingFilterModelDTO filter);
        PagingData<E_VoucherBudgetDetailDTO> GetDetails(EvoucherBudgetDetailPagingFilterModelDTO pagingModel);
        IEnumerable<EvoucherTypeDTO> GetDenominations(int? companyId);
        string CompleteDistribute(E_VoucherBudgetDTO model);
        string ReturnDistribute(E_VoucherBudgetDTO model);
        string ApproveDistribute(E_VoucherBudgetDTO model);
    }

    public class EvoucherBudgetDistributeRepository : BaseDataRepository<E_VoucherBudget>, IEvoucherBudgetDistributeRepository
    {
        private readonly IGenericRepository<E_VoucherBudget> _repo;
        private readonly IGenericRepository<E_VoucherBudgetDetail> _detailRepo;
        private readonly IGenericRepository<E_VoucherBudgetDetailLine> _detailLineRepo;
        private readonly IGenericRepository<E_VoucherBudgetDenominations> _denomiRepo;
        private readonly IGenericRepository<Org_Organization> _orgRepo;
        private readonly IGenericRepository<Org_UserOrg> _userOrgRepo;
        private readonly IGenericRepository<E_VoucherType> _vcTypeRepo;

        public EvoucherBudgetDistributeRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _repo = _unitOfWork.GetDataRepository<E_VoucherBudget>();
            _detailRepo = _unitOfWork.GetDataRepository<E_VoucherBudgetDetail>();
            _orgRepo = _unitOfWork.GetDataRepository<Org_Organization>();
            _userOrgRepo = _unitOfWork.GetDataRepository<Org_UserOrg>();
            _denomiRepo = _unitOfWork.GetDataRepository<E_VoucherBudgetDenominations>();
            _vcTypeRepo = _unitOfWork.GetDataRepository<E_VoucherType>();
            _detailLineRepo = _unitOfWork.GetDataRepository<E_VoucherBudgetDetailLine>();
        }

        public PagingData<E_VoucherBudgetDTO> FindBudgetDistributes(EvoucherBudgetDetailPagingFilterModelDTO filter)
        {
            var query = _repo.SelectWhere(x =>
               ((x.BudgetDate.HasValue && filter.DateFrom.HasValue && DbFunctions.TruncateTime(x.BudgetDate.Value) >= DbFunctions.TruncateTime(filter.DateFrom.Value))
                   || !filter.DateFrom.HasValue
               )
               && ((x.BudgetDate.HasValue && filter.DateFrom.HasValue && DbFunctions.TruncateTime(x.BudgetDate.Value) <= DbFunctions.TruncateTime(filter.DateTo.Value))
                   || !filter.DateTo.HasValue
               )
               && x.CompanyId == filter.CompanyId
               // trang thai: chi load trang thai da nhap hoac da phan bo hoac null: tat ca;
               && (((filter.Status != null && x.Status == filter.Status) || filter.Status == null) && x.Status >= (int?)BudgetStatus.CreatedCompleted)
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

        public E_VoucherBudgetDTO GetBudgetDistributesById(EvoucherBudgetDetailPagingFilterModelDTO filter)
        {
            E_VoucherBudgetDTO res = null;
            var budget = _repo.SelectWhere(x => x.Id == filter.EvoucherBudgetId && x.CompanyId == filter.CompanyId).FirstOrDefault();
            if (budget != null)
            {
                res = new E_VoucherBudgetDTO()
                {
                    Budget = budget.Budget,
                    BudgetDate = budget.BudgetDate,
                    CompanyId = budget.CompanyId,
                    StaffConfirm = budget.StaffConfirm,
                    TotalStaff = budget.TotalStaff,
                    Status = budget.Status,
                    StatusName = budget.StatusName,
                    Id = budget.Id,
                    TotalValues = budget.TotalValues,
                    CompanyName = budget.CompanyName,
                    CreateDate = budget.CreateDate,
                    CreateUser = budget.CreateUser
                };
                // tong so voucher da phan bo
                var totalDistriVouchers = _denomiRepo.SelectWhere(x => x.VoucherBudgetId == budget.Id).ToList();
                res.TotalDistributedCount = totalDistriVouchers != null && totalDistriVouchers.Count > 0 ? totalDistriVouchers.Sum(x => x.CountNumber) : 0;

                // cac menh gia
                var denomis = _denomiRepo.SelectWhere(x => x.VoucherBudgetId == filter.EvoucherBudgetId).Select(
                    x => new E_VoucherBudgetDenominationsDTO
                    {
                        VoucherBudgetId = x.VoucherBudgetId,
                        CountNumber = x.CountNumber,
                        Denominations = x.Denominations,
                        Id = x.Id,
                        TotalValues = x.TotalValues,
                        VoucherTypeCode = x.VoucherTypeCode,
                        VoucherTypeName = x.VoucherTypeName
                    }).ToList();
                res.BudgetDenominations = denomis;

                // tong so menh gia cac loai
                if (denomis != null && denomis.Count > 0)
                {
                    res.DenomiTotalCount = denomis.Sum(x => x.CountNumber);
                    res.DenomiTotalValues = denomis.Sum(x => x.TotalValues);
                }
            }
            return res;
        }

        public E_VoucherBudgetDetailDTO GetEmployeeBudgetDist(EvoucherBudgetDetailPagingFilterModelDTO filter)
        {
            throw new NotImplementedException();
        }

        public string SaveCompanyBudgetDistributes(E_VoucherBudgetDTO model)
        {
            using (var ts = this._unitOfWork.BeginTransaction())
            {
                try
                {
                    var budget = this._repo.SelectWhere(x => x.Id == model.Id && x.CompanyId == model.CompanyId).FirstOrDefault();
                    if (budget != null)
                    {
                        // da hoan thanh phan bo
                        if (budget.Status >= (int?)BudgetStatus.DistributedCompleted)
                        {
                            return "Ngân sách của công ty ngày này đã hoàn thành phân bổ";
                        }

                        // update trang thai thanh da phan bo
                        var properties = new List<Expression<Func<E_VoucherBudget, object>>>
                        {
                            (x => x.Status),
                            (x => x.StatusName),
                        };
                        budget.Status = (int?)BudgetStatus.Distributed;
                        budget.StatusName = "Đã phân bổ";
                        this._repo.UpdateByProperties(budget, properties);
                        _unitOfWork.SaveChanges();

                        #region Them dong menh gia cua cac nhan vien
                        foreach (var empData in model.BudgetDetails)
                        {
                            var budDetail = _detailRepo.SelectWhere(x => x.Id == empData.Id && x.OrgCompanyId == model.CompanyId).FirstOrDefault();
                            if (budDetail != null)
                            {
                                if (empData.DistributeLines != null && empData.DistributeLines.Count > 0)
                                {
                                    // update trang thai thanh da phan bo va so luong
                                    var detailProps = new List<Expression<Func<E_VoucherBudgetDetail, object>>>
                                    {
                                        (x => x.Status),
                                        (x => x.StatusName),
                                    };
                                    budDetail.Status = (int?)BudgetStatus.Distributed;
                                    budDetail.StatusName = "Đã phân bổ";
                                    this._detailRepo.UpdateByProperties(budDetail, detailProps);
                                    _unitOfWork.SaveChanges();

                                    if (empData.DistributeLines.Sum(x => x.TotalValues) > empData.Budget)
                                    {
                                        return "Dữ liệu không khớp: [" + empData.CodeUser + "] Tổng tiền voucher lớn hơn ngân sách";
                                    }

                                    foreach (var demino in empData.DistributeLines)
                                    {
                                        // tim dong co cung menh gia cua nhan vien nay
                                        var demi = _detailLineRepo.SelectWhere(x => x.VoucherTypeCode == demino.VoucherTypeCode && x.VoucherBudgetDetailId == empData.Id).FirstOrDefault();
                                        if (demi != null) // update
                                        {
                                            demi.CountNumberPage = demino.CountNumberPage;
                                            demi.Denominations = demino.Denominations;
                                            demi.TotalValues = demino.TotalValues;

                                            var detailLineProps = new List<Expression<Func<E_VoucherBudgetDetailLine, object>>>
                                            {
                                                (x => x.CountNumberPage),
                                                (x => x.Denominations),
                                                (x => x.TotalValues),
                                            };
                                            this._detailLineRepo.UpdateByProperties(demi, detailLineProps);
                                            _unitOfWork.SaveChanges();
                                        }
                                        else // add
                                        {
                                            var demi1 = new E_VoucherBudgetDetailLine()
                                            {
                                                CountNumberPage = demino.CountNumberPage,
                                                Denominations = demino.Denominations,
                                                TotalValues = demino.TotalValues,
                                                VoucherBudgetDetailId = (int?)empData.Id,
                                                VoucherTypeCode = demino.VoucherTypeCode,
                                                VoucherTypeName = demino.VoucherTypeName
                                            };

                                            _detailLineRepo.Add(demi1);
                                            _unitOfWork.SaveChanges();
                                        }
                                    }
                                }
                            }
                        }
                        #endregion


                        #region Them dong menh gia cua ngay
                        foreach (var budDeno in model.BudgetDenominations)
                        {
                            // tim dong co cung menh gia cua nhan vien nay
                            var tBudDenomi = _denomiRepo.SelectWhere(x => x.VoucherTypeCode == budDeno.VoucherTypeCode && x.VoucherBudgetId == model.Id).FirstOrDefault();
                            if (tBudDenomi != null) // update
                            {
                                tBudDenomi.CountNumber = budDeno.CountNumber;
                                tBudDenomi.Denominations = budDeno.Denominations;
                                tBudDenomi.TotalValues = budDeno.TotalValues;

                                var budDenoProps = new List<Expression<Func<E_VoucherBudgetDenominations, object>>>
                                            {
                                                (x => x.CountNumber),
                                                (x => x.Denominations),
                                                (x => x.TotalValues),
                                            };
                                this._denomiRepo.UpdateByProperties(tBudDenomi, budDenoProps);
                                _unitOfWork.SaveChanges();
                            }
                            else // add
                            {
                                var demi1 = new E_VoucherBudgetDenominations()
                                {
                                    CountNumber = budDeno.CountNumber,
                                    Denominations = budDeno.Denominations,
                                    TotalValues = budDeno.TotalValues,
                                    VoucherBudgetId = model.Id,
                                    VoucherTypeCode = budDeno.VoucherTypeCode,
                                    VoucherTypeName = budDeno.VoucherTypeName,
                                };

                                _denomiRepo.Add(demi1);
                                _unitOfWork.SaveChanges();
                            }
                        }
                        #endregion

                        ts.Commit();
                    }
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }
            return "";
        }

        public string SaveEmployeeBudgetDistributes(E_VoucherBudgetDetailDTO filter, int? companyId)
        {
            using (var ts = this._unitOfWork.BeginTransaction())
            {
                try
                {
                    var budget = this._repo.SelectWhere(x => x.Id == filter.VoucherBudgetId && x.CompanyId == companyId).FirstOrDefault();
                    if (budget != null)
                    {
                        // da hoan thanh phan bo
                        if (budget.Status >= (int?)BudgetStatus.DistributedCompleted)
                        {
                            return "Ngân sách của công ty ngày này đã hoàn thành phân bổ";
                        }

                        var budDetail = _detailRepo.SelectWhere(x => x.Id == filter.Id).FirstOrDefault();
                        if (budDetail != null)
                        {
                            #region xoa line cu
                            var oldLines = _detailLineRepo.SelectWhere(x => x.VoucherBudgetDetailId == budDetail.Id).ToList();
                            var deleteLines = new List<E_VoucherBudgetDetailLine>();
                            // danh sach detail line se xoa
                            if (filter.DistributeLines != null && filter.DistributeLines.Count > 0)
                            {
                                foreach (var item in oldLines)
                                {
                                    if (!filter.DistributeLines.Exists(x => x.VoucherTypeCode == item.VoucherTypeCode))
                                    {
                                        deleteLines.Add(item);
                                    }
                                }
                            }
                            else
                            {
                                deleteLines = oldLines;
                            }
                            foreach (var item in deleteLines)
                            {
                                _detailLineRepo.Delete(item);
                                _unitOfWork.SaveChanges();

                            }
                            #endregion

                            #region them moi hoac update
                            foreach (var demino in filter.DistributeLines)
                            {
                                // tim dong co cung menh gia cua nhan vien nay
                                var demi = _detailLineRepo.SelectWhere(x => x.VoucherTypeCode == demino.VoucherTypeCode && x.VoucherBudgetDetailId == filter.Id).FirstOrDefault();
                                if (demi != null) // update
                                {
                                    demi.CountNumberPage = demino.CountNumberPage;
                                    demi.Denominations = demino.Denominations;
                                    demi.TotalValues = demino.TotalValues;

                                    var detailLineProps = new List<Expression<Func<E_VoucherBudgetDetailLine, object>>>
                                            {
                                                (x => x.CountNumberPage),
                                                (x => x.Denominations),
                                                (x => x.TotalValues),
                                            };
                                    this._detailLineRepo.UpdateByProperties(demi, detailLineProps);
                                    _unitOfWork.SaveChanges();
                                }
                                else // add
                                {
                                    var demi1 = new E_VoucherBudgetDetailLine()
                                    {
                                        CountNumberPage = demino.CountNumberPage,
                                        Denominations = demino.Denominations,
                                        TotalValues = demino.TotalValues,
                                        VoucherBudgetDetailId = (int?)filter.Id,
                                        VoucherTypeCode = demino.VoucherTypeCode,
                                        VoucherTypeName = demino.VoucherTypeName
                                    };

                                    _detailLineRepo.Add(demi1);
                                    _unitOfWork.SaveChanges();
                                }
                            }
                            #endregion

                            #region update bang thong ke menh gia cua ngay
                            // danh sach nhan vien
                            var details = _detailRepo.SelectWhere(x => x.VoucherBudgetId == filter.VoucherBudgetId).ToList();
                            // danh sach menh gia
                            var oldBudgetDenomis = _denomiRepo.SelectWhere(x => x.VoucherBudgetId == filter.VoucherBudgetId).ToList();

                            // danh sach thong ke menh gia cua tung nhan vien
                            var currBudgetDenomis = new List<E_VoucherBudgetDenominationsDTO>();

                            if (details != null && details.Count > 0)
                            {
                                foreach (var item in details)
                                {
                                    var detailLines = _detailLineRepo.SelectWhere(x => x.VoucherBudgetDetailId == item.Id).ToList();
                                    if (detailLines != null && detailLines.Count > 0)
                                    {
                                        foreach (var line in detailLines)
                                        {
                                            var budDeno = currBudgetDenomis.FirstOrDefault(x => x.VoucherTypeCode == line.VoucherTypeCode);
                                            // neu co trong danh sach 
                                            if (budDeno != null)
                                            {
                                                budDeno.CountNumber += line.CountNumberPage;
                                                budDeno.TotalValues = budDeno.CountNumber * budDeno.Denominations;
                                            }
                                            else // chua co
                                            {
                                                currBudgetDenomis.Add(new E_VoucherBudgetDenominationsDTO()
                                                {
                                                    CountNumber = line.CountNumberPage,
                                                    Denominations = line.Denominations,
                                                    TotalValues = line.TotalValues,
                                                    VoucherTypeCode = line.VoucherTypeCode,
                                                    VoucherTypeName = line.VoucherTypeName
                                                });
                                            }
                                        }

                                    }
                                }

                                // cap nhat danh sach
                                #region danh sach menh gia bang 0
                                var setToZeroDenomis = new List<E_VoucherBudgetDenominations>();
                                if (currBudgetDenomis != null && currBudgetDenomis.Count > 0)
                                {
                                    foreach (var item in oldBudgetDenomis)
                                    {
                                        if (!currBudgetDenomis.Exists(x => x.VoucherTypeCode == item.VoucherTypeCode))
                                        {
                                            setToZeroDenomis.Add(item);
                                        }
                                    }
                                }
                                else
                                {
                                    setToZeroDenomis = oldBudgetDenomis;
                                }
                                foreach (var item in setToZeroDenomis)
                                {
                                    var setToZeroDenomisProps = new List<Expression<Func<E_VoucherBudgetDenominations, object>>>
                                            {
                                                (x => x.CountNumber),
                                                (x => x.Denominations),
                                                (x => x.TotalValues),
                                            };
                                    this._denomiRepo.UpdateByProperties(item, setToZeroDenomisProps);
                                    _unitOfWork.SaveChanges();
                                }
                                #endregion

                                #region cap nhat danh sach
                                foreach (var budDeno in currBudgetDenomis)
                                {
                                    // tim dong co cung menh gia cua nhan vien nay
                                    var tBudDenomi = _denomiRepo.SelectWhere(x => x.VoucherTypeCode == budDeno.VoucherTypeCode && x.VoucherBudgetId == filter.VoucherBudgetId).FirstOrDefault();
                                    if (tBudDenomi != null) // update
                                    {
                                        tBudDenomi.CountNumber = budDeno.CountNumber;
                                        tBudDenomi.Denominations = budDeno.Denominations;
                                        tBudDenomi.TotalValues = budDeno.TotalValues;

                                        var budDenoProps = new List<Expression<Func<E_VoucherBudgetDenominations, object>>>
                                            {
                                                (x => x.CountNumber),
                                                (x => x.Denominations),
                                                (x => x.TotalValues),
                                            };
                                        this._denomiRepo.UpdateByProperties(tBudDenomi, budDenoProps);
                                        _unitOfWork.SaveChanges();
                                    }
                                    else // add
                                    {
                                        var demi1 = new E_VoucherBudgetDenominations()
                                        {
                                            CountNumber = budDeno.CountNumber,
                                            Denominations = budDeno.Denominations,
                                            TotalValues = budDeno.TotalValues,
                                            VoucherBudgetId = (long)filter.VoucherBudgetId,
                                            VoucherTypeCode = budDeno.VoucherTypeCode,
                                            VoucherTypeName = budDeno.VoucherTypeName,
                                        };

                                        _denomiRepo.Add(demi1);
                                        _unitOfWork.SaveChanges();
                                    }
                                }
                                #endregion
                            }
                            #endregion
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

        public PagingData<E_VoucherBudgetDetailDTO> GetDetails(EvoucherBudgetDetailPagingFilterModelDTO filter)
        {
            var query = _detailRepo.SelectWhere(x => x.VoucherBudgetId == filter.EvoucherBudgetId && x.OrgCompanyId == filter.CompanyId);

            var res = query.OrderBy(x => x.Id)
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
                 CreatedUser = x.CreatedUser,
                 VoucherBudgetId = x.VoucherBudgetId
             }).ToList();

            // lay danh sach chi tiet voucher tung nhan vien
            if (res != null && res.Count > 0)
            {
                foreach (var budDetail in res)
                {
                    budDetail.DistributeLines = DistributeLines(budDetail.Id);
                }
            }

            return new PagingData<E_VoucherBudgetDetailDTO>() { PageData = res, TotalRowCount = (long)query.Count() };
        }

        private List<E_VoucherBudgetDetailLineDTO> DistributeLines(long id)
        {
            return _detailLineRepo.SelectWhere(x => x.VoucherBudgetDetailId == id).Select(x => new E_VoucherBudgetDetailLineDTO()
            {
                CountNumberPage = x.CountNumberPage,
                Denominations = x.Denominations,
                Id = x.Id,
                TotalValues = x.TotalValues,
                VoucherBudgetDetailId = x.VoucherBudgetDetailId,
                VoucherTypeCode = x.VoucherTypeCode,
                VoucherTypeName = x.VoucherTypeName
            }).ToList();
        }

        public IEnumerable<EvoucherTypeDTO> GetDenominations(int? companyId)
        {
            return _vcTypeRepo.SelectWhere(x => x.IsValidate.HasValue && x.IsValidate.Value && x.CompanyId == companyId).OrderBy(x => x.OderNumber).Select(x => new EvoucherTypeDTO()
            {
                Code = x.Code,
                OderNumber = x.OderNumber,
                Name = x.Name,
                Denominations = x.Denominations,
                Id = x.Id,
                IsValidate = x.IsValidate,
            }).ToList();
        }

        public string CompleteDistribute(E_VoucherBudgetDTO model)
        {
            return new EvoucherBudgetRepository(this._unitOfWork).UpdateBudgetStatus(model, BudgetStatus.DistributedCompleted, "Hoàn thành phân bổ");
        }

        public string ReturnDistribute(E_VoucherBudgetDTO model)
        {
            return new EvoucherBudgetRepository(this._unitOfWork).UpdateBudgetStatus(model, BudgetStatus.Distributed, "Đã phân bổ");
        }

        public string ApproveDistribute(E_VoucherBudgetDTO model)
        {
            return new EvoucherBudgetRepository(this._unitOfWork).UpdateBudgetStatus(model, BudgetStatus.Approved, "Đã phê duyệt");
        }
    }
}
