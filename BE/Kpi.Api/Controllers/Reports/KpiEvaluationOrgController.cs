using Kpi.Api.Controllers.Base;
using Kpi.Api.Models.Resources;
using Kpi.Api.Models.Utilities;
using Kpi.Core.DTO;
using Kpi.Service.DataServices.Reports;
using System.Web.Http;

namespace Kpi.Api.Controllers.Reports
{
    public class KpiEvaluationOrgController : BaseApiController<IView_KpiEvaluation_OrganizationService>
    {
        public KpiEvaluationOrgController(IView_KpiEvaluation_OrganizationService service) : base(service)
        {
        }

        [HttpPost]
        public RespondData GetOrgs()
        {
            return Success(this._service.GetOrgs());
        }

        [HttpPost]
        public RespondData Search(View_KpiEvaluation_OrganizationFilterModelDTO filter)
        {
            return Success(this._service.Search(filter));
        }
    }
}