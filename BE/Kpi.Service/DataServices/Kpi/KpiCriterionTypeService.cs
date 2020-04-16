using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kpi.Core.DTO;
using Kpi.DataAccess.Repo.EFRepos.Kpi;

namespace Kpi.Service.DataServices.Kpi
{
    public interface IKpiCriterionTypeService : BaseServices.IGridService<KpiCriterionTypeDTO, KpiCriterionTypePaginFilterModelDTO>
    {
        IEnumerable<CriterionCatalogDTO> GetKpiCatalogs();
    }

    public class KpiCriterionTypeService : BaseServices.BaseGridService<IKpiCriterionTypeRepository, KpiCriterionTypeDTO, KpiCriterionTypePaginFilterModelDTO>, IKpiCriterionTypeService
    {
        public KpiCriterionTypeService(IKpiCriterionTypeRepository gridRepository) : base(gridRepository)
        {
        }

        public IEnumerable<CriterionCatalogDTO> GetKpiCatalogs()
        {
            return this._gridRepository.GetKpiCatalogs();
        }
    }
}
