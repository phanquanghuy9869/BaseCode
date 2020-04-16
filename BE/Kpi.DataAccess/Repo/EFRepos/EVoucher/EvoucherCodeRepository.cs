using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using Kpi.Core.DTO;
using Kpi.DataAccess.DataContext;
using Kpi.DataAccess.Repo.Base;
using Kpi.Core.Helper;
using System.Linq.Expressions;
using AutoMapper;

namespace Kpi.DataAccess.Repo.EFRepos.EVoucher
{
    public interface IEvoucherCodeRepository : IGridRepository<EVoucherCodeDTO, EvoucherCodePagingFilterModelDTO>
    {
    }

    public class EvoucherCodeRepository : BaseDataRepository<E_VoucherCode>, IEvoucherCodeRepository
    {
        private readonly IGenericRepository<E_VoucherCode> _vRepo;
        private readonly IGenericRepository<E_VoucherType> _eVType;
        private readonly IGenericRepository<Org_OrganizationType> _orgTypeRepo;

        public EvoucherCodeRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _vRepo = _unitOfWork.GetDataRepository<E_VoucherCode>();
            _eVType = _unitOfWork.GetDataRepository<E_VoucherType>();
            _orgTypeRepo = _unitOfWork.GetDataRepository<Org_OrganizationType>();
        }
        
        public IEnumerable<EVoucherCodeDTO> GetPaging(EvoucherCodePagingFilterModelDTO pagingModel)
        {
            var query = this.PagingFilter(_vRepo.SelectAll(), pagingModel);
            return query.OrderBy(x => x.Id).Skip(pagingModel.Start - 1).Take(pagingModel.Length).ProjectTo<EVoucherCodeDTO>().ToList();
        }

        protected IQueryable<E_VoucherCode> PagingFilter(IQueryable<E_VoucherCode> query, EvoucherCodePagingFilterModelDTO pagingModel)
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

        public EVoucherCodeDTO Add(EVoucherCodeDTO entityDTo)
        {
            var entity = AutoMapperHelper.Map<EVoucherCodeDTO, E_VoucherCode>(entityDTo);
            var result = _vRepo.Add(entity);
            _unitOfWork.SaveChanges();
            return AutoMapperHelper.Map< E_VoucherCode, EVoucherCodeDTO>(result);
        }

        public IEnumerable<EVoucherCodeDTO> AddRange(IEnumerable<EVoucherCodeDTO> entityDTOs)
        {
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<EVoucherCodeDTO, E_VoucherCode>();
            });
            IMapper mapper = config.CreateMapper();
            IEnumerable<E_VoucherCode> entities = mapper.Map<List<EVoucherCodeDTO>, List<E_VoucherCode>>(entityDTOs as List<EVoucherCodeDTO>);
            entities = _vRepo.AddRange(entities);
            _unitOfWork.SaveChanges();

            config = new MapperConfiguration(cfg => {
                cfg.CreateMap<E_VoucherCode, EVoucherCodeDTO>();
            });
            mapper = config.CreateMapper();
            var result = mapper.Map<List<E_VoucherCode>, List<EVoucherCodeDTO>>(entities as List<E_VoucherCode>);
            return result;
        }

        public virtual int Count(EvoucherCodePagingFilterModelDTO pagingModel)
        {
            var query = this.PagingFilter(_vRepo.SelectAll(), pagingModel);
            return query.Count();
        }

        public virtual void Update(EVoucherCodeDTO entityDto)
        {
            var entity = AutoMapperHelper.Map<EVoucherCodeDTO, E_VoucherCode>(entityDto);
            _vRepo.Update(entity);
            _unitOfWork.SaveChanges();
        }

        public virtual void Delete(EVoucherCodeDTO entityDto)
        {
            var entity = AutoMapperHelper.Map<EVoucherCodeDTO, E_VoucherCode>(entityDto);
            _vRepo.Delete(entity);
            _unitOfWork.SaveChanges();
        }

        public virtual EVoucherCodeDTO Get(int id)
        {
            var result = _vRepo.Get(x => x.Id == id);
            return AutoMapperHelper.Map<E_VoucherCode, EVoucherCodeDTO>(result);
        }

        public virtual IEnumerable<EVoucherCodeDTO> GetAll()
        {
            return _vRepo.SelectAll().ProjectTo<EVoucherCodeDTO>().ToList();
        }
    }
}
