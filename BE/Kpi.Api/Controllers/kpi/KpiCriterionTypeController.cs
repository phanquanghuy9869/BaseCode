using Kpi.Api.Controllers.Base;
using Kpi.Api.Models.Resources;
using Kpi.Api.Models.Utilities;
using Kpi.Core.DTO;
using Kpi.Core.Helper;
using Kpi.Service.DataServices.Kpi;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Linq;
using System.Web.Http;

namespace Kpi.Api.Controllers.kpi
{

    //[Authorize]
    public class KpiCriterionTypeController : BaseGridApiController<IKpiCriterionTypeService, KpiCriterionTypeViewModel, KpiCriterionTypeDTO, KpiCriterionTypePaginFilterModel, KpiCriterionTypePaginFilterModelDTO>
    {
        public KpiCriterionTypeController(IKpiCriterionTypeService service) : base(service)
        {
        }

        [HttpPost]
        public RespondData GetKpiCatalogs()
        {
            return Success(this._service.GetKpiCatalogs());
        }
    }
}