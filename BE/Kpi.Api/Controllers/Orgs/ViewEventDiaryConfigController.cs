using System;
using Kpi.Api.Controllers.Base;
using Kpi.Api.Models.Resources;
using Kpi.Api.Models.Utilities;
using Kpi.Core.DTO;
using Kpi.Service.DataServices.Kpi;
using Kpi.Service.DataServices.Orgs;

namespace Kpi.Api.Controllers.kpi
{
    public class ViewEventDiaryConfigController : BaseGridApiController<IViewEventDiaryConfigService, ViewEventDiaryConfigRS, ViewEventDiaryConfigDTO, IGridFilterModel, ViewEventDiaryConfigFilterModelDTO>
    {
        // GET: DiaryCriterionDetail
        public ViewEventDiaryConfigController(IViewEventDiaryConfigService service) : base(service)
        {}

        protected override ViewEventDiaryConfigDTO FetchDataCreate(ViewEventDiaryConfigRS model)
        {
            var modelDTO = base.FetchDataCreate(model);
            //modelDTO.CreatedDate = DateTime.Now;
           // modelDTO.CreatedByUser = this.UserId;
            //modelDTO.CreatedByUserFullName = this.UserOrg.UserFullName;
            //modelDTO.CreatedByUserTitle = this.UserOrg.JobTitle;
            return modelDTO;
        }

        public override RespondData GetPaging(IGridFilterModel filter)
        {
            return base.GetPaging(filter);
        }
    }
}