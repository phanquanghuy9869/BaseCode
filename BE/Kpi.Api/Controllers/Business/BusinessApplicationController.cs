using Kpi.Api.Controllers.Base;
using Kpi.Core.DTO;
using Kpi.Service.DataServices.Business;

namespace Kpi.Api.Controllers.Business
{
    public class BusinessApplicationController : BaseApiController<IBusinessApplicationService>
    {
        public BusinessApplicationController(IBusinessApplicationService service) : base(service)
        {
        }

        public RespondData GetFirstKpiProcess()
        {
            return Success(this._service.GetFirstKpiProcess());
        }
    }
}