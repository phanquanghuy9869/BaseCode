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
    public class EvoucherBudgetController : BaseApiController<IEvoucherBudgetService>
    {
        public EvoucherBudgetController(IEvoucherBudgetService service, IUserOrgService userOrgService) : base(service, userOrgService)
        {
        }

        #region evoucher budget list
        [HttpPost]
        public RespondData Find(EvoucherBudgetDetailPagingFilterModelDTO filter)
        {
            filter.CompanyId = this.GetOrg();
            var result = _service.Find(filter);
            return Success(result);
        }

        [HttpPost]
        public RespondData AddVoucherBudget(E_VoucherBudgetDTO model)
        {
            model.CreateUser = HttpContext.Current.User.Identity.Name;
            model.CompanyId = this.GetOrg();
            var result = _service.AddVoucherBudget(model);
            return Success(result);
        }
        #endregion

        [HttpPost]
        public RespondData GetCompanies()
        {
            var result = _service.GetCompanies();
            return Success(result);
        }

        #region evoucher budget infor
        [HttpPost]
        public RespondData FindById(EvoucherBudgetDetailPagingFilterModelDTO filter)
        {
            filter.CompanyId = this.GetOrg();
            var result = _service.FindById(filter);
            return Success(result);
        }

        [HttpPost]
        public RespondData GetDetailsPaging(EvoucherBudgetDetailPagingFilterModelDTO filter)
        {
            filter.CompanyId = this.GetOrg();
            var result = _service.GetDetailsPaging(filter);
            return Success(result);
        }

        [HttpPost]
        public RespondData FinishBudget(E_VoucherBudgetDTO model)
        {
            model.UpdateUser = HttpContext.Current.User.Identity.Name;
            model.UpdateDate = DateTime.Now;
            model.CompanyId = this.GetOrg();
            var result = _service.FinishBudget(model);
            if (!string.IsNullOrWhiteSpace(result))
            {
                return Fail(result);
            }
            return Success(null);
        }
        #endregion

    }
}