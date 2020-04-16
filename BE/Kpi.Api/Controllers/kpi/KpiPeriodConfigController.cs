using Kpi.Api.Controllers.Base;
using Kpi.Api.Models.Resources;
using Kpi.Api.Models.Utilities;
using Kpi.Core.DTO;
using Kpi.Service.DataServices.Kpi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Kpi.Api.Controllers.Kpi
{
    
    [Authorize]
    public class KpiPeriodConfigController : BaseGridApiController<IKpiPeriodConfigService, KpiPeriodConfigRS, KpiPeriodConfigDTO, KpiPeriodConfigGridFilterModel, KpiPeriodConfigFilterModelDTO>
    {
        public KpiPeriodConfigController(IKpiPeriodConfigService service) : base(service)
        {

        }

        protected override KpiPeriodConfigDTO FetchDataCreate(KpiPeriodConfigRS model)
        {
            var dto = base.FetchDataUpdate(model);
            dto.CreatedDate = DateTime.Now;
            dto.CreatedByUser = this.UserName;
            return dto;
        }

        [HttpPost]
        public RespondData UpdateKPIByYearMonthEvent(UpdateKPIByYearMonthEventFilter model)
        {
            this._service.UpdateKPIByYearMonthEvent(model);
            return Success("OK");
        }
    }
}