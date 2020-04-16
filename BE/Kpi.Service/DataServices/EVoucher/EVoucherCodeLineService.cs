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
    public interface IEVoucherCodeLineService : IGridService<E_VoucherCodeLineDTO, EvoucherCodeLinePagingFilterModelDTO>
    {
        List<string> CheckExistedVoucherCode(List<string> voucherCodes);
        void ImportVoucherModel(EVoucherImportModelDTO importModel);
        IEnumerable<E_VoucherCodeLineDTO> GetUserVoucher(EvoucherCodeLinePagingFilterModelDTO pagingModel);

    }

    public class EVoucherCodeLineService : BaseGridService<IEvoucherCodeLineRepository, E_VoucherCodeLineDTO, EvoucherCodeLinePagingFilterModelDTO>, IEVoucherCodeLineService
    {
        public EVoucherCodeLineService(IEvoucherCodeLineRepository ieVoucherRepo) : base(ieVoucherRepo)
        {
        }

        public List<string> CheckExistedVoucherCode(List<string> voucherCodes)
        {
            return this._gridRepository.CheckExistedVoucherCode(voucherCodes);
        }

        public void ImportVoucherModel(EVoucherImportModelDTO importModel)
        {
            this._gridRepository.ImportVoucherModel(importModel);
        }
        
        public IEnumerable<E_VoucherCodeLineDTO> GetUserVoucher(EvoucherCodeLinePagingFilterModelDTO pagingModel)
        {
            return _gridRepository.GetUserVoucher(pagingModel);
        }

    }
}
