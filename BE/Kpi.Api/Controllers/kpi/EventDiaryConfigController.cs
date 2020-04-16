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
    public class EventDiaryConfigController : BaseGridApiController<IEventDiaryConfigService, ViewEventDiaryConfigRS, EventDiaryConfigDTO, ViewEventDiaryConfigFilterModel, ViewEventDiaryConfigFilterModelDTO>
    {
        public EventDiaryConfigController(IEventDiaryConfigService service) : base(service)
        {
        }

        [HttpPost]
        public RespondData GetConfigOrgByLevel1Manager()
        {
            return Success(this._service.GetConfigOrgByLevel1Manager(this.UserName));
        }

        [HttpPost]
        public RespondData GetConfigOrgByLevel2Manager()
        {
            return Success(this._service.GetConfigOrgByLevel2Manager(this.UserName));
        }

        [HttpPost]
        public RespondData GetConfigOrgByKpiManager()
        {
            return Success(this._service.GetConfigOrgByKpiManager());
        }

        [HttpPost]
        public RespondData GetConfigOrgByDivManager()
        {
            return Success(this._service.GetConfigOrgByDivManager(this.UserName));
        }

        [HttpPost]
        public RespondData GetOrgs()
        {
            return Success(this._service.GetOrgs());
        }

        [HttpPost]
        public RespondData GetUsers()
        {
            return Success(this._service.GetUsers());
        }

        [HttpPost]
        public RespondData GetConfigLevel1Manager()
        {
            return Success(this._service.GetConfigLevel1Manager());
        }
    }
}