using Kpi.Api.Controllers.Base;
using Kpi.Api.Models;
using Kpi.Api.Models.Resources;
using Kpi.Api.Models.Utilities;
using Kpi.Core.DTO;
using Kpi.Core.Helper;
using Kpi.DataAccess.DataContext;
using Kpi.Service.DataServices.Org;
using System.Collections.Generic;
using System.Web.Http;
using System.Linq;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Threading.Tasks;
using Kpi.Api.Models.OwinManager;
using Microsoft.AspNet.Identity.Owin;
using System.Web;
using System.Net.Http;
using Kpi.Service.DataServices.EVoucher;
using System;

namespace Kpi.Api.Controllers.EVoucher
{

    [Authorize]
    public class EvoucherBudgetDistributeController : BaseApiController<IEvoucherBudgetDistributeService>
    {
        public EvoucherBudgetDistributeController(IEvoucherBudgetDistributeService service, IUserOrgService userOrgService) : base(service, userOrgService)
        {
        }

        [HttpPost]
        public RespondData FindBudgetDistributes(EvoucherBudgetDetailPagingFilterModelDTO filter)
        {
            filter.CompanyId = this.GetOrg();
            var result = _service.FindBudgetDistributes(filter);
            return Success(result);
        }

        [HttpPost]
        public RespondData GetBudgetDistributesById(EvoucherBudgetDetailPagingFilterModelDTO filter)
        {
            filter.CompanyId = this.GetOrg();
            var result = _service.GetBudgetDistributesById(filter);
            return Success(result);
        }

        [HttpPost]
        public RespondData GetDetails(EvoucherBudgetDetailPagingFilterModelDTO filter)
        {
            filter.CompanyId = this.GetOrg();
            var result = _service.GetDetails(filter);
            return Success(result);
        }

        [HttpPost]
        public RespondData GetDenominations()
        {
            var result = _service.GetDenominations(this.GetOrg());
            return Success(result);
        }

        [HttpPost]
        public RespondData GetEmployeeBudgetDist(EvoucherBudgetDetailPagingFilterModelDTO filter)
        {
            filter.CompanyId = this.GetOrg();
            var result = _service.GetEmployeeBudgetDist(filter);
            return Success(result);
        }

        [HttpPost]
        public RespondData SaveCompanyBudgetDistributes(E_VoucherBudgetDTO model)
        {
            model.CompanyId = this.GetOrg();
            var result = _service.SaveCompanyBudgetDistributes(model);
            if (string.IsNullOrWhiteSpace(result))
            {
                return Success(null);
            }

            return Fail(result);
        }

        [HttpPost]
        public RespondData SaveEmployeeBudgetDistributes(E_VoucherBudgetDetailDTO model)
        {
            var result = _service.SaveEmployeeBudgetDistributes(model, GetOrg());
            if (string.IsNullOrWhiteSpace(result))
            {
                return Success(null);
            }

            return Fail(result);
        }

        [HttpPost]
        public RespondData CompleteDistribute(E_VoucherBudgetDTO model)
        {
            model.UpdateUser = HttpContext.Current.User.Identity.Name;
            model.UpdateDate = DateTime.Now;
            model.CompanyId = this.GetOrg();
            var result = _service.CompleteDistribute(model);
            if (!string.IsNullOrWhiteSpace(result))
            {
                return Fail(result);
            }
            return Success(null);
        }

        [HttpPost]
        public RespondData ReturnDistribute(E_VoucherBudgetDTO model)
        {
            model.UpdateUser = HttpContext.Current.User.Identity.Name;
            model.UpdateDate = DateTime.Now;
            model.CompanyId = this.GetOrg();
            var result = _service.ReturnDistribute(model);
            if (!string.IsNullOrWhiteSpace(result))
            {
                return Fail(result);
            }
            return Success(null);
        }

        [HttpPost]
        public RespondData ApproveDistribute(E_VoucherBudgetDTO model)
        {
            model.UpdateUser = HttpContext.Current.User.Identity.Name;
            model.UpdateDate = DateTime.Now;
            model.CompanyId = this.GetOrg();
            var result = _service.ApproveDistribute(model);
            if (!string.IsNullOrWhiteSpace(result))
            {
                return Fail(result);
            }
            return Success(null);
        }
    }
}