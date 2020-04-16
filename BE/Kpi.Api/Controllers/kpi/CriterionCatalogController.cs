using Kpi.Api.Controllers.Base;
using Kpi.Api.Models.Resources;
using Kpi.Api.Models.Utilities;
using Kpi.Core.DTO;
using Kpi.Service.DataServices.Kpi;
using System.Web.Http;

namespace Kpi.Api.Controllers.Kpi
{
    public class CriterionCatalogController : BaseGridApiController<ICriterionCatalogService, KpiCriterionCatalogViewModel, CriterionCatalogDTO, KpiCriterionCatalogPaginFilterModel, KpiCriterionCatalogPaginFilterModelDTO>
    {
        public CriterionCatalogController(ICriterionCatalogService service) : base(service)
        {
        }

        [HttpPost]
        public RespondData GetCriterionCatalog()
        {
            return Success(this._service.GetCriterionCatalog());
        }
    }
}