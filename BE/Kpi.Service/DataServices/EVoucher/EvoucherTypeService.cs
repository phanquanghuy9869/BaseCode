using Kpi.Core.DTO;
using Kpi.DataAccess.Repo.EFRepos.Orgs;
using Kpi.Service.BaseServices;
using System.Collections.Generic;
using System;
using Kpi.DataAccess.DataContext;
using Kpi.DataAccess.Repo.EFRepos.EVoucher;

namespace Kpi.Service.DataServices.EVoucher
{
    public interface IEvoucherTypeService : IGridService<EvoucherTypeDTO, EvoucherTypePagingFilterModelDTO>
    {
        EvoucherTypeDTO GetOneVoucherType(EvoucherTypePagingFilterModelDTO filter);
    }

    public class EvoucherTypeService : BaseGridService<IEvoucherTypeRepository, EvoucherTypeDTO, EvoucherTypePagingFilterModelDTO>, IEvoucherTypeService
    {
        public EvoucherTypeService(IEvoucherTypeRepository gridRepository) : base(gridRepository)
        {
        }

        public EvoucherTypeDTO GetOneVoucherType(EvoucherTypePagingFilterModelDTO filter)
        {
            return this._gridRepository.GetOneVoucherType(filter);
        }
    }
}
