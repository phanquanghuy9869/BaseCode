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

namespace Kpi.DataAccess.Repo.EFRepos.EVoucher
{
    public interface IEvoucherTypeRepository : IGridRepository<EvoucherTypeDTO, EvoucherTypePagingFilterModelDTO>
    {
        EvoucherTypeDTO GetOneVoucherType(EvoucherTypePagingFilterModelDTO filter);
    }

    public class EvoucherTypeRepository : BaseGridRepository<E_VoucherType, EvoucherTypeDTO, EvoucherTypePagingFilterModelDTO>, IEvoucherTypeRepository
    {
        private readonly IGenericRepository<View_UserOrg_GetPaging> _viewUsrRepo;
        private readonly IGenericRepository<Org_OrganizationType> _orgTypeRepo;

        public EvoucherTypeRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _viewUsrRepo = _unitOfWork.GetDataRepository<View_UserOrg_GetPaging>();
            _orgTypeRepo = _unitOfWork.GetDataRepository<Org_OrganizationType>();
        }

        public EvoucherTypeDTO GetOneVoucherType(EvoucherTypePagingFilterModelDTO filter)
        {
            return _repo.SelectWhere(x => x.Id == filter.Id && x.CompanyId == filter.CompanyId).Select(x => new EvoucherTypeDTO()
            {
                Code = x.Code,
                CompanyId = x.CompanyId,
                Id = x.Id,
                CompanyName = x.CompanyName,
                CreatedDate = x.CreatedDate,
                CreateUser = x.CreateUser,
                Denominations = x.Denominations,
                Description = x.Description,
                IsValidate = x.IsValidate,
                Name = x.Name,
                OderNumber = x.OderNumber,
                UpdateDate = x.UpdateDate,
                UpdateUser = x.UpdateUser
            }).FirstOrDefault();
        }

        protected override IQueryable<E_VoucherType> PagingFilter(IQueryable<E_VoucherType> query, EvoucherTypePagingFilterModelDTO pagingModel)
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
            query = this._repo.SelectWhere(x => ((isValid.HasValue && ((x.IsValidate == null && isValid == false) || (x.IsValidate == isValid))) || !isValid.HasValue)
                && ((x.Name.Contains(pagingModel.Name) && !string.IsNullOrEmpty(pagingModel.Name)) || string.IsNullOrEmpty(pagingModel.Name))
                && ((x.Code.Contains(pagingModel.Code) && !string.IsNullOrEmpty(pagingModel.Code)) || string.IsNullOrEmpty(pagingModel.Code))
                && x.CompanyId == pagingModel.CompanyId);
            return base.PagingFilter(query, pagingModel);
        }

    }
}
