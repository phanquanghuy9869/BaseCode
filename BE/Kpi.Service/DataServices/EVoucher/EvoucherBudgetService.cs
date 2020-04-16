using Kpi.Core.DTO;
using Kpi.DataAccess.Repo.EFRepos.Orgs;
using Kpi.Service.BaseServices;
using System.Collections.Generic;
using System;
using Kpi.DataAccess.DataContext;
using Kpi.DataAccess.Repo.EFRepos.EVoucher;

namespace Kpi.Service.DataServices.EVoucher
{
    public interface IEvoucherBudgetService
    {
        PagingData<E_VoucherBudgetDetailDTO> GetDetailsPaging(EvoucherBudgetDetailPagingFilterModelDTO pagingModel);
        PagingData<E_VoucherBudgetDTO> Find(EvoucherBudgetDetailPagingFilterModelDTO filter);
        IEnumerable<OrganizationDTO> GetCompanies();
        ImportDataResult AddVoucherBudget(E_VoucherBudgetDTO model);
        E_VoucherBudgetDTO FindById(EvoucherBudgetDetailPagingFilterModelDTO filter);
        string FinishBudget(E_VoucherBudgetDTO model);
    }

    public class EvoucherBudgetService : BaseDataService<IEvoucherBudgetRepository>, IEvoucherBudgetService
    {
        public EvoucherBudgetService(IEvoucherBudgetRepository gridRepository) : base(gridRepository)
        {
        }

        public ImportDataResult AddVoucherBudget(E_VoucherBudgetDTO model)
        {
            return this._repo.AddVoucherBudget(model);
        }

        public PagingData<E_VoucherBudgetDTO> Find(EvoucherBudgetDetailPagingFilterModelDTO filter)
        {
            return this._repo.Find(filter);
        }

        public E_VoucherBudgetDTO FindById(EvoucherBudgetDetailPagingFilterModelDTO filter)
        {
            return this._repo.FindById(filter);
        }

        public string FinishBudget(E_VoucherBudgetDTO model)
        {
            return this._repo.FinishBudget(model);
        }

        public IEnumerable<OrganizationDTO> GetCompanies()
        {
            return this._repo.GetCompanies();
        }

        public PagingData<E_VoucherBudgetDetailDTO> GetDetailsPaging(EvoucherBudgetDetailPagingFilterModelDTO pagingModel)
        {
            return this._repo.GetDetailsPaging(pagingModel);
        }
    }
}
