using Kpi.Core.DTO;
using Kpi.DataAccess.Repo.EFRepos.Orgs;
using Kpi.Service.BaseServices;
using System.Collections.Generic;
using System;
using Kpi.DataAccess.DataContext;
using Kpi.DataAccess.Repo.EFRepos.EVoucher;

namespace Kpi.Service.DataServices.EVoucher
{
    public interface IEvoucherBudgetDistributeService
    {
        PagingData<E_VoucherBudgetDTO> FindBudgetDistributes(EvoucherBudgetDetailPagingFilterModelDTO pagingModel);
        E_VoucherBudgetDTO GetBudgetDistributesById(EvoucherBudgetDetailPagingFilterModelDTO filter);
        string SaveCompanyBudgetDistributes(E_VoucherBudgetDTO model);
        string SaveEmployeeBudgetDistributes(E_VoucherBudgetDetailDTO model, int? companyId);
        E_VoucherBudgetDetailDTO GetEmployeeBudgetDist(EvoucherBudgetDetailPagingFilterModelDTO filter);
        PagingData<E_VoucherBudgetDetailDTO> GetDetails(EvoucherBudgetDetailPagingFilterModelDTO pagingModel);
        IEnumerable<EvoucherTypeDTO> GetDenominations(int? companyId);
        string CompleteDistribute(E_VoucherBudgetDTO model);
        string ReturnDistribute(E_VoucherBudgetDTO model);
        string ApproveDistribute(E_VoucherBudgetDTO model);
    }

    public class EvoucherBudgetDistributeService : BaseDataService<IEvoucherBudgetDistributeRepository>, IEvoucherBudgetDistributeService
    {
        public EvoucherBudgetDistributeService(IEvoucherBudgetDistributeRepository gridRepository) : base(gridRepository)
        {
        }

        public string ApproveDistribute(E_VoucherBudgetDTO model)
        {
            return this._repo.ApproveDistribute(model);
        }

        public string ReturnDistribute(E_VoucherBudgetDTO model)
        {
            return this._repo.ReturnDistribute(model);
        }

        public string CompleteDistribute(E_VoucherBudgetDTO model)
        {
            return this._repo.CompleteDistribute(model);
        }

        public PagingData<E_VoucherBudgetDTO> FindBudgetDistributes(EvoucherBudgetDetailPagingFilterModelDTO pagingModel)
        {
            return this._repo.FindBudgetDistributes(pagingModel);
        }

        public E_VoucherBudgetDTO GetBudgetDistributesById(EvoucherBudgetDetailPagingFilterModelDTO filter)
        {
            return _repo.GetBudgetDistributesById(filter);
        }

        public IEnumerable<EvoucherTypeDTO> GetDenominations(int? companyId)
        {
            return _repo.GetDenominations(companyId);
        }

        public PagingData<E_VoucherBudgetDetailDTO> GetDetails(EvoucherBudgetDetailPagingFilterModelDTO pagingModel)
        {
            return _repo.GetDetails(pagingModel);
        }

        public E_VoucherBudgetDetailDTO GetEmployeeBudgetDist(EvoucherBudgetDetailPagingFilterModelDTO filter)
        {
            return _repo.GetEmployeeBudgetDist(filter);
        }

        public string SaveCompanyBudgetDistributes(E_VoucherBudgetDTO model)
        {
            return _repo.SaveCompanyBudgetDistributes(model);
        }

        public string SaveEmployeeBudgetDistributes(E_VoucherBudgetDetailDTO model, int? companyId)
        {
            return _repo.SaveEmployeeBudgetDistributes(model, companyId);
        }
    }
}
