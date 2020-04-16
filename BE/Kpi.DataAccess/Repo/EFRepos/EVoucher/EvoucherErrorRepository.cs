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
    public interface IEvoucherErrorRepository : IGridRepository<EvoucherTypeDTO, EvoucherTypePagingFilterModelDTO>
    {
    }

    public class EvoucherErrorRepository : BaseGridRepository<E_VoucherType, EvoucherTypeDTO, EvoucherTypePagingFilterModelDTO>, IEvoucherErrorRepository
    {
        private readonly IGenericRepository<View_UserOrg_GetPaging> _viewUsrRepo;
        private readonly IGenericRepository<Org_OrganizationType> _orgTypeRepo;

        public EvoucherErrorRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _viewUsrRepo = _unitOfWork.GetDataRepository<View_UserOrg_GetPaging>();
            _orgTypeRepo = _unitOfWork.GetDataRepository<Org_OrganizationType>();
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
                && ((x.Code.Contains(pagingModel.Code) && !string.IsNullOrEmpty(pagingModel.Code)) || string.IsNullOrEmpty(pagingModel.Code)));
            return base.PagingFilter(query, pagingModel);
        }

    }
}
