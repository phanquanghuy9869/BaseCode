using Kpi.Core.DTO;
using Kpi.DataAccess.Repo.EFRepos.EVoucher;
using Kpi.Service.BaseServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kpi.Service.DataServices.EVoucher
{
    public interface IEVoucherCodeService : IGridService<EVoucherCodeDTO, EvoucherCodePagingFilterModelDTO> { }

    public class EVoucherCodeService : BaseGridService<IEvoucherCodeRepository, EVoucherCodeDTO, EvoucherCodePagingFilterModelDTO>, IEVoucherCodeService
    {
        public EVoucherCodeService(IEvoucherCodeRepository gridRepository) : base(gridRepository)
        {
        }
    }
}
