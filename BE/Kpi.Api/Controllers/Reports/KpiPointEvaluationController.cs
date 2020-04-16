using Kpi.Api.Controllers.Base;
using Kpi.Api.Models.Resources;
using Kpi.Api.Models.Utilities;
using Kpi.Core.DTO;
using Kpi.Service.DataServices.Reports;

namespace Kpi.Api.Controllers.Reports
{
    public class KpiPointEvaluationController : BaseGridApiController<IKpiPointEvaluationService, KpiRS, KpiDTO, IGridFilterModel, KpiPointEvaluationFilterModelDTO>
    {
        public KpiPointEvaluationController(IKpiPointEvaluationService service) : base(service)
        {
        }


    }
}