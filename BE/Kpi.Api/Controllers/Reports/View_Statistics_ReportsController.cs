using Kpi.Api.Controllers.Base;
using Kpi.Api.Models.Resources;
using Kpi.Api.Models.Utilities;
using Kpi.Core.DTO;
using Kpi.Service.DataServices.Reports;
using System.Web.Http;

namespace Kpi.Api.Controllers.Reports
{
    public class View_Statistics_ReportsController : BaseApiController<IView_Statistics_ReportsService>
    {
        public View_Statistics_ReportsController(IView_Statistics_ReportsService service) : base(service)
        {
        }

        [HttpPost]
        public RespondData GetOrgs()
        {
            return Success(this._service.GetOrgs());
        }

        [HttpPost]
        public RespondData Search(View_Statistics_ReportsFilterModelDTO filter)
        {
            if (filter.checkOrg )
            {
                return Success(this._service.Search(filter));
            }
            else
            {
                return Success(this._service.Search1(filter));
            }
        }
    }
}