using Kpi.Core.DTO;
using Kpi.DataAccess.Repo.EFRepos.Kpi;
using System;
using System.Collections.Generic;

namespace Kpi.Service.DataServices.Kpi
{
    public interface ICriterionCatalogService : BaseServices.IGridService<CriterionCatalogDTO, KpiCriterionCatalogPaginFilterModelDTO>
    {
        IEnumerable<CriterionCatalogDTO> GetCriterionCatalog();
    }

    public class CriterionCatalogService : BaseServices.BaseGridService<ICriterionCatalogRepository, CriterionCatalogDTO, KpiCriterionCatalogPaginFilterModelDTO>, ICriterionCatalogService
    {

        public CriterionCatalogService(ICriterionCatalogRepository repo) : base(repo)
        {
        }

        public IEnumerable<CriterionCatalogDTO> GetCriterionCatalog()
        {
            return this._gridRepository.GetCriterionCatalog();
        }
    }
}
