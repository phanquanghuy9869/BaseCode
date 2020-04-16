using Kpi.Api.Controllers.Base;
using Kpi.Core.DTO;
using Kpi.Service.DataServices.Kpi;
using System.Web.Http;

namespace Kpi.Api.Controllers.Reports
{
    public class UnLockDiaryCriterionController : BaseApiController<IUnLockDiaryCriterionService>
    {
        public UnLockDiaryCriterionController(IUnLockDiaryCriterionService service) : base(service)
        {
        }

        [HttpPost]
        public RespondData GetOrgs()
        {
            return Success(this._service.GetOrgs());
        }

        [HttpPost]
        public RespondData Search(UnLockDiaryCriterionFilterModelDTO filter)
        {
            return Success(this._service.Search(filter));
        }
    }
}