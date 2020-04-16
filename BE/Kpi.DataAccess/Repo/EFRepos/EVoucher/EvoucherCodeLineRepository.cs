using System.Collections.Generic;
using System.Linq;
using AutoMapper.QueryableExtensions;
using Kpi.Core.DTO;
using Kpi.DataAccess.DataContext;
using Kpi.DataAccess.Repo.Base;
using Kpi.Core.Helper;
using AutoMapper;
using System;

namespace Kpi.DataAccess.Repo.EFRepos.EVoucher
{
    public interface IEvoucherCodeLineRepository : IGridRepository<E_VoucherCodeLineDTO, EvoucherCodeLinePagingFilterModelDTO>
    {
        IEnumerable<E_VoucherCodeLineDTO> GetUserVoucher(EvoucherCodeLinePagingFilterModelDTO pagingModel);
        List<string> CheckExistedVoucherCode(List<string> voucherCodes);
        void ImportVoucherModel(EVoucherImportModelDTO importModel);
    }

    public class EvoucherCodeLineRepository : BaseDataRepository<E_VoucherCodeLine>, IEvoucherCodeLineRepository
    {
        private readonly IGenericRepository<E_VoucherCodeLine> _vRepo;
        private readonly IGenericRepository<E_VoucherCodeDenominations> _voucherCodeDenomGRepo;
        private readonly IGenericRepository<E_VoucherCode> _voucherPeriodRepo;
        private readonly IGenericRepository<E_VoucherType> _eVType;
        private readonly IGenericRepository<Org_OrganizationType> _orgTypeRepo;

        public EvoucherCodeLineRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _vRepo = _unitOfWork.GetDataRepository<E_VoucherCodeLine>();
            _eVType = _unitOfWork.GetDataRepository<E_VoucherType>();
            _orgTypeRepo = _unitOfWork.GetDataRepository<Org_OrganizationType>();
            _voucherPeriodRepo = _unitOfWork.GetDataRepository<E_VoucherCode>();
            _voucherCodeDenomGRepo = _unitOfWork.GetDataRepository<E_VoucherCodeDenominations>();
        }

        public IEnumerable<E_VoucherCodeLineDTO> GetPaging(EvoucherCodeLinePagingFilterModelDTO pagingModel)
        {
            var query = this.PagingFilter(_vRepo.SelectAll(), pagingModel);
            return query.OrderBy(x => x.Id).Skip(pagingModel.Start - 1).Take(pagingModel.Length).ProjectTo<E_VoucherCodeLineDTO>().ToList();
        }


        public IEnumerable<E_VoucherCodeLineDTO> GetUserVoucher(EvoucherCodeLinePagingFilterModelDTO pagingModel)
        {
            var query = this.PagingFilter(_vRepo.SelectWhere(x => x.EvoucheBudgetDetailId == pagingModel.EvoucheBudgetDetailId 
                                                            && x.Status == pagingModel.Status)
                                                            , pagingModel);
            return query.OrderBy(x => x.Id).Skip(pagingModel.Start - 1).Take(pagingModel.Length).ProjectTo<E_VoucherCodeLineDTO>().ToList();
        }


        protected IQueryable<E_VoucherCodeLine> PagingFilter(IQueryable<E_VoucherCodeLine> query, EvoucherCodeLinePagingFilterModelDTO pagingModel)
        {
            bool? isValid = null;
            switch (pagingModel.IsValidate)
            {
                case "T":
                    isValid = true;
                    break;
                case "F":
                    isValid = false;
                    break;
                default:
                    break;
            }
            query = this._vRepo.SelectWhere(x => (isValid.HasValue));
            return query;
        }

        public E_VoucherCodeLineDTO Add(E_VoucherCodeLineDTO entityDTo)
        {
            var entity = AutoMapperHelper.Map<E_VoucherCodeLineDTO, E_VoucherCodeLine>(entityDTo);
            var result = _vRepo.Add(entity);
            _unitOfWork.SaveChanges();
            return AutoMapperHelper.Map<E_VoucherCodeLine, E_VoucherCodeLineDTO>(result);
        }

        public IEnumerable<E_VoucherCodeLineDTO> AddRange(IEnumerable<E_VoucherCodeLineDTO> entityDTOs)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<E_VoucherCodeLineDTO, E_VoucherCode>();
            });
            IMapper mapper = config.CreateMapper();
            IEnumerable<E_VoucherCodeLine> entities = mapper.Map<List<E_VoucherCodeLineDTO>, List<E_VoucherCodeLine>>(entityDTOs as List<E_VoucherCodeLineDTO>);
            entities = _vRepo.AddRange(entities);
            _unitOfWork.SaveChanges();

            config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<E_VoucherCode, E_VoucherCodeLineDTO>();
            });
            mapper = config.CreateMapper();
            var result = mapper.Map<List<E_VoucherCode>, List<E_VoucherCodeLineDTO>>(entities as List<E_VoucherCode>);
            return result;
        }

        public virtual int Count(EvoucherCodeLinePagingFilterModelDTO pagingModel)
        {
            var query = this.PagingFilter(_vRepo.SelectAll(), pagingModel);
            return query.Count();
        }

        public virtual void Update(E_VoucherCodeLineDTO entityDto)
        {
            var entity = AutoMapperHelper.Map<E_VoucherCodeLineDTO, E_VoucherCodeLine>(entityDto);
            _vRepo.Update(entity);
            _unitOfWork.SaveChanges();
        }

        public virtual void Delete(E_VoucherCodeLineDTO entityDto)
        {
            var entity = AutoMapperHelper.Map<E_VoucherCodeLineDTO, E_VoucherCodeLine>(entityDto);
            _vRepo.Delete(entity);
            _unitOfWork.SaveChanges();
        }

        public virtual E_VoucherCodeLineDTO Get(int id)
        {
            var result = _vRepo.Get(x => x.Id == id);
            return AutoMapperHelper.Map<E_VoucherCodeLine, E_VoucherCodeLineDTO>(result);
        }

        public virtual IEnumerable<E_VoucherCodeLineDTO> GetAll()
        {
            return _vRepo.SelectAll().ProjectTo<E_VoucherCodeLineDTO>().ToList();
        }

        public List<string> CheckExistedVoucherCode(List<string> voucherCodes)
        {
            return _vRepo.SelectWhere(x => voucherCodes.Contains(x.Code)).Select(x => x.Code).Distinct().ToList();
        }

        public void ImportVoucherModel(EVoucherImportModelDTO importModel)
        {
            using (var tsc = this._unitOfWork.BeginTransaction())
            {
                var voucherPeriod = this.AddOrEditVoucherPeriod(importModel);
                var voucherDenom = this.AddVoucherDenomination(importModel, voucherPeriod.Id);
                this.AddVoucherLine(importModel, voucherPeriod.Id);
                tsc.Commit();
            }
        }

        // add bảng E_VoucherCode
        private E_VoucherCode AddOrEditVoucherPeriod(EVoucherImportModelDTO importModel)
        {
            var voucherPeriod = this._voucherPeriodRepo.SelectWhere(x => x.CompanyId == importModel.OrgId && x.VoucheCodeDate == importModel.Time).FirstOrDefault();
            if (voucherPeriod != null)
            {
                return voucherPeriod;
            }

            var budget = importModel.ImportData.Sum(x => x.Denominations);
            voucherPeriod = new E_VoucherCode
            {
                CompanyId = importModel.OrgId,
                CompanyName = importModel.OrgName,
                Budget = budget,
                TotalValues = 0,
                VoucheCodeDate = importModel.Time,
                VoucheCodeMonth = importModel.Time.Month,
                VoucheCodeYear = importModel.Time.Year,
                CreateDate = DateTime.Now,
                CreateUser = importModel.CurrentUserName,
            };
            voucherPeriod = this._voucherPeriodRepo.Add(voucherPeriod);
            this._unitOfWork.SaveChanges();
            return voucherPeriod;
        }

        private E_VoucherCodeDenominations AddVoucherDenomination(EVoucherImportModelDTO importModel, long voucherPeriodId)
        {
            var voucherDenomination = new E_VoucherCodeDenominations
            {
                VoucherCodeId = voucherPeriodId,
                VoucherTypeCode = importModel.VoucherTypeCode,
                VoucherTypeName = importModel.VoucherTypeName,
                Denominations = importModel.Denominations,
                CountNumber = importModel.ImportData.Count,
                TotalValues = 0,
            };
            voucherDenomination = this._voucherCodeDenomGRepo.Add(voucherDenomination);
            this._unitOfWork.SaveChanges();
            return voucherDenomination;
        }

        private List<E_VoucherCodeLine> AddVoucherLine(EVoucherImportModelDTO importModel, long voucherPeriodId)
        {
            var codeLines = importModel.ImportData.Select(x => new E_VoucherCodeLine
            {
                VoucherCodeId = voucherPeriodId,
                VoucherTypeCode = importModel.VoucherTypeCode,
                VoucheTypeName = importModel.VoucherTypeName,
                Denominations = x.Denominations,
                Code = x.Code,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                CreateDate = x.CreateDate,
                CreateUser = importModel.CurrentUserName,
                Status = 1,
                StatusName = "",
                UseDate = x.UseDate
            });
            codeLines = this._vRepo.AddRange(codeLines);
            this._unitOfWork.SaveChanges();
            return codeLines.ToList();
        }
    }
}
