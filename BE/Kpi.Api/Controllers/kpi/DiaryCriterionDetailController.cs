using System;
using System.Linq;
using System.Web.Http;
using Kpi.Api.Controllers.Base;
using Kpi.Api.Models.Resources;
using Kpi.Api.Models.Utilities;
using Kpi.Core.DTO;
using Kpi.Service.DataServices.Kpi;

namespace Kpi.Api.Controllers.kpi
{
    [Authorize]
    public class DiaryCriterionDetailController : BaseGridApiController<IDiaryCriterionDetailService, DiaryCriterionDetailRS, DiaryCriterionDetailDTO, IGridFilterModel, DiaryCriterionDetailFilterModelDTO>
    {
        // GET: DiaryCriterionDetail
        public DiaryCriterionDetailController(IDiaryCriterionDetailService service) : base(service)
        { }

        protected override DiaryCriterionDetailDTO FetchDataCreate(DiaryCriterionDetailRS model)
        {
            var modelDTO = base.FetchDataCreate(model);
            modelDTO.CreatedDate = DateTime.Now;
            modelDTO.CreatedByUser = this.UserOrg.Id.ToString();
            modelDTO.CreatedByUserFullName = this.UserOrg.UserFullName;
            modelDTO.CreatedByUserTitle = this.UserOrg.JobTitle;
            return modelDTO;
        }

        public override RespondData GetPaging(IGridFilterModel filter)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public RespondData RemoveByManagerLv2(DiaryCriterionDetailDTO filter)
        {
            var isHrOrHrMangager = false;
            var user = this.AppUserManager.Users.FirstOrDefault(x => x.UserName == this.UserName);
            if (user != null)
            {
                var roles = this.AppRoleManager.Roles.Where(x => x.Name == KpiUserRole.EVoucherDistributor || x.Name == KpiUserRole.BudgetDistributor);
                foreach (var role in user.Roles)
                {
                    if (roles.FirstOrDefault(x => x.Id == role.RoleId) != null)
                    {
                        isHrOrHrMangager = true;
                        break;
                    }
                }
            }

            this._service.RemoveByManagerLv2(filter, this.UserName, isHrOrHrMangager);
            return Success("OK");
        }


        [HttpPost]
        public RespondData UpdateKpiCriterionPointByKpiId(KpiRS filter)
        {
            this._service.UpdateKpiCriterionPointByKpiId(filter.Id);
            return Success(null);
        }

        [HttpPost]
        public RespondData UpdateKpiCriterionPointByYearMonth(KpiRS filter)
        {
            this._service.UpdateKpiCriterionPointByYearMonth(filter.YearMonth.Value);
            return Success(null);
        }
    }
}