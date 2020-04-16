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
    public interface IView_EVoucherUserRepository : IGridRepository<View_EVoucherUserDTO, View_EVoucherUserPagingFilterModelDTO>
    {
        IEnumerable<View_EVoucherUserDTO> GetUserVoucher(View_EVoucherUserPagingFilterModelDTO pagingModel);
        void UpdateEvoucherStatus(View_EVoucherUserDTO evoucherUserDTO);
        
    }

    public class View_EVoucherUserRepository : BaseDataRepository<View_EVoucherUser>, IView_EVoucherUserRepository
    {
        private readonly IGenericRepository<View_EVoucherUser> _vRepo;
        private readonly IGenericRepository<E_VoucherCodeLine> _eVoucherCodeLine;
        private readonly IGenericRepository<Org_OrganizationType> _orgTypeRepo;
        private readonly IGenericRepository<Org_UserOrg> _orgUserOrgRepo;
        


        public View_EVoucherUserRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _vRepo = _unitOfWork.GetDataRepository<View_EVoucherUser>();
            _eVoucherCodeLine = _unitOfWork.GetDataRepository<E_VoucherCodeLine>();
            _orgTypeRepo = _unitOfWork.GetDataRepository<Org_OrganizationType>();
            _orgUserOrgRepo = _unitOfWork.GetDataRepository<Org_UserOrg>();
        }

        public IEnumerable<View_EVoucherUserDTO> GetPaging(View_EVoucherUserPagingFilterModelDTO pagingModel)
        {
            var query = this.PagingFilter(_vRepo.SelectAll(), pagingModel);
            return query.OrderBy(x => x.Id).Skip(pagingModel.Start - 1).Take(pagingModel.Length).ProjectTo<View_EVoucherUserDTO>().ToList();
        }


        /* public IEnumerable<View_EVoucherUserDTO> GetUserVoucher(View_EVoucherUserPagingFilterModelDTO pagingModel)
         {
             var query = this.PagingFilter(_vRepo.SelectWhere(x => x.CodeUser == pagingModel.CodeUser
                                                             && x.Status == pagingModel.Status)
                                                             , pagingModel);
             return query.OrderBy(x => x.Id).Skip(pagingModel.Start - 1).Take(pagingModel.Length).ProjectTo<View_EVoucherUserDTO>().ToList();
         }
         */
        public IEnumerable<View_EVoucherUserDTO> GetUserVoucher(View_EVoucherUserPagingFilterModelDTO pagingModel)
        {
            Org_UserOrg org_UserOrg = _orgUserOrgRepo.Get(x => x.UserName == pagingModel.UserName);
            if (org_UserOrg == null) return null;
            if (pagingModel.IsExpiryDate == 1)
            {
                return _vRepo.SelectWhere(x => x.CodeUser == org_UserOrg.Code
                                                                && x.Status == pagingModel.Status
                                                                && x.EndDate >= System.DateTime.Today  )
                                                                .OrderBy(x => x.EndDate)
                                                                .ProjectTo<View_EVoucherUserDTO>().ToList();
            }
            else if (pagingModel.IsExpiryDate == 2)
            {
                return _vRepo.SelectWhere(x => x.CodeUser == org_UserOrg.Code
                                                                && x.Status == pagingModel.Status
                                                                && x.EndDate < System.DateTime.Today)
                                                                .OrderBy(x => x.EndDate)
                                                                .ProjectTo<View_EVoucherUserDTO>().ToList();
            }
            else
            {
                return _vRepo.SelectWhere(x => x.CodeUser == org_UserOrg.Code
                                                               && x.Status == pagingModel.Status)
                                                               .OrderBy(x => x.EndDate)
                                                               .ProjectTo<View_EVoucherUserDTO>().ToList();
            }
            /*var query = this.PagingFilter(_vRepo.SelectWhere(x => x.CodeUser == pagingModel.CodeUser
                                                            && x.Status == pagingModel.Status)
                                                            , pagingModel);
            return query.OrderBy(x => x.Id).Skip(pagingModel.Start - 1).Take(pagingModel.Length).ProjectTo<View_EVoucherUserDTO>().ToList();*/
        }

        public virtual int Count(View_EVoucherUserPagingFilterModelDTO pagingModel)
        {
            Org_UserOrg org_UserOrg = _orgUserOrgRepo.Get(x => x.UserName == pagingModel.UserName);
            if (org_UserOrg == null) return 0;
            if (pagingModel.IsExpiryDate == 1)
            {
                return _vRepo.SelectWhere(x => x.CodeUser == org_UserOrg.Code
                                                           && x.Status == pagingModel.Status
                                                           && x.EndDate >= System.DateTime.Today
                                                           ).Count();
            }
            else if (pagingModel.IsExpiryDate == 2)
            {
                return _vRepo.SelectWhere(x => x.CodeUser == org_UserOrg.Code
                                                           && x.Status == pagingModel.Status
                                                           && x.EndDate < System.DateTime.Today
                                                           ).Count();
            }
            else
            {
                return _vRepo.SelectWhere(x => x.CodeUser == org_UserOrg.Code
                                                        && x.Status == pagingModel.Status
                                                        ).Count();
            }
            /* var query = this.PagingFilter(_vRepo.SelectAll(), pagingModel);
             return query.Count();*/
        }
        protected IQueryable<View_EVoucherUser> PagingFilter(IQueryable<View_EVoucherUser> query, View_EVoucherUserPagingFilterModelDTO pagingModel)
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

        public View_EVoucherUserDTO Add(View_EVoucherUserDTO entityDTo)
        {
            var entity = AutoMapperHelper.Map<View_EVoucherUserDTO, View_EVoucherUser>(entityDTo);
            var result = _vRepo.Add(entity);
            _unitOfWork.SaveChanges();
            return AutoMapperHelper.Map<View_EVoucherUser, View_EVoucherUserDTO>(result);
        }

        public IEnumerable<View_EVoucherUserDTO> AddRange(IEnumerable<View_EVoucherUserDTO> entityDTOs)
        {
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<View_EVoucherUserDTO, E_VoucherCode>();
            });
            IMapper mapper = config.CreateMapper();
            IEnumerable<View_EVoucherUser> entities = mapper.Map<List<View_EVoucherUserDTO>, List<View_EVoucherUser>>(entityDTOs as List<View_EVoucherUserDTO>);
            entities = _vRepo.AddRange(entities);
            _unitOfWork.SaveChanges();

            config = new MapperConfiguration(cfg => {
                cfg.CreateMap<E_VoucherCode, View_EVoucherUserDTO>();
            });
            mapper = config.CreateMapper();
            var result = mapper.Map<List<E_VoucherCode>, List<View_EVoucherUserDTO>>(entities as List<E_VoucherCode>);
            return result;
        }

       

        public virtual void Update(View_EVoucherUserDTO entityDto)
        {
            var entity = AutoMapperHelper.Map<View_EVoucherUserDTO, View_EVoucherUser>(entityDto);
            _vRepo.Update(entity);
            _unitOfWork.SaveChanges();
        }

        public void UpdateEvoucherStatus(View_EVoucherUserDTO evoucherCodeLineDTO)
        {
            using (var ts = this._unitOfWork.BeginTransaction())
            {
                var evoucherCodeLineEntity = new E_VoucherCodeLine
                {
                    Code = evoucherCodeLineDTO.Code,
                    Status = evoucherCodeLineDTO.Status,
                    StatusName = evoucherCodeLineDTO.StatusName,                   
                };

                var properties = new List<Expression<Func<E_VoucherCodeLine, object>>>
                {(x => x.StatusName),
                 (x => x.StatusName),
                };
                this._eVoucherCodeLine.UpdateByProperties(evoucherCodeLineEntity, properties);
                this._unitOfWork.SaveChanges();

                ts.Commit();
            }
        }

        public virtual void Delete(View_EVoucherUserDTO entityDto)
        {
            var entity = AutoMapperHelper.Map<View_EVoucherUserDTO, View_EVoucherUser>(entityDto);
            _vRepo.Delete(entity);
            _unitOfWork.SaveChanges();
        }

        public virtual View_EVoucherUserDTO Get(int id)
        {
            var result = _vRepo.Get(x => x.Id == id);
            return AutoMapperHelper.Map<View_EVoucherUser, View_EVoucherUserDTO>(result);
        }

        public virtual IEnumerable<View_EVoucherUserDTO> GetAll()
        {
            return _vRepo.SelectAll().ProjectTo<View_EVoucherUserDTO>().ToList();
        }
    }
}
