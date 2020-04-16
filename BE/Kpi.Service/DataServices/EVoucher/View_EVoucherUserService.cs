using Kpi.Core.DTO;
using Kpi.DataAccess.Repo.EFRepos.Orgs;
using Kpi.Service.BaseServices;
using System.Collections.Generic;
using System;
using Kpi.DataAccess.DataContext;
using Kpi.DataAccess.Repo.EFRepos.EVoucher;

namespace Kpi.Service.DataServices.EVoucher
{
    public interface IView_EVoucherUserService : IGridService<View_EVoucherUserDTO, View_EVoucherUserPagingFilterModelDTO>
    {
        IEnumerable<View_EVoucherUserDTO> GetUserVoucher(View_EVoucherUserPagingFilterModelDTO pagingModel);
        void UpdateEvoucherStatus(View_EVoucherUserDTO evoucherUserDTO);

    }

    public class View_EVoucherUserService : BaseGridService<IView_EVoucherUserRepository, View_EVoucherUserDTO, View_EVoucherUserPagingFilterModelDTO>, IView_EVoucherUserService
    {
        public View_EVoucherUserService(IView_EVoucherUserRepository gridRepository) : base(gridRepository)
        {
        }

        public IEnumerable<View_EVoucherUserDTO> GetUserVoucher(View_EVoucherUserPagingFilterModelDTO pagingModel)
        {
            return _gridRepository.GetUserVoucher(pagingModel);
        }


        public void UpdateEvoucherStatus(View_EVoucherUserDTO evoucherUserDTO)
        {
             _gridRepository.UpdateEvoucherStatus(evoucherUserDTO);
        }

    }
}
