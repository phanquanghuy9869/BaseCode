using System;
using Kpi.Api.Controllers.Base;
using Kpi.Api.Models.Resources;
using Kpi.Api.Models.Utilities;
using Kpi.Core.DTO;
using Kpi.Service.DataServices.Kpi;
using Kpi.Service.DataServices.Orgs;

namespace Kpi.Api.Controllers.kpi
{
    public class ViewUserEventDiaryController : BaseGridApiController<IViewUserEventDiaryService, ViewUserEventDiaryRS, ViewUserEventDiaryDTO, IGridFilterModel, ViewUserEventDiaryFilterModelDTO>
    {
        // GET: DiaryCriterionDetail
        public ViewUserEventDiaryController(IViewUserEventDiaryService service) : base(service)
        {}

        protected override ViewUserEventDiaryDTO FetchDataCreate(ViewUserEventDiaryRS model)
        {
            var modelDTO = base.FetchDataCreate(model);
            modelDTO.CreatedDate = DateTime.Now;
            modelDTO.CreatedByUser = this.UserId;
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