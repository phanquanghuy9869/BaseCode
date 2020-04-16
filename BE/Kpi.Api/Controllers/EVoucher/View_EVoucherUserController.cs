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

namespace Kpi.Api.Controllers.EVoucher
{

    public class View_EVoucherUserRS
    {
        public string IsValidate { get; set; }
        public string CodeUser { get; set; }
        public string VoucherCode { get; set; }
        public string StatusName { get; set; }
        public long EvoucheBudgetDetailId { get; set; }
        public long VoucherCodeId { get; set; }
        public int Status { get; set; }
        public int IsExpiryDate { get; set; }

    }

    [Authorize]
    public class View_EVoucherUserController : BaseApiController<IView_EVoucherUserService>
    {
        public View_EVoucherUserController(IView_EVoucherUserService service) : base(service)
        { }
        [HttpPost]
        public RespondData GetUserVoucher(View_EVoucherUserRS view_EVoucherUserRS)
        {          
            View_EVoucherUserPagingFilterModelDTO filter = new View_EVoucherUserPagingFilterModelDTO();
            filter.UserName = System.Web.HttpContext.Current.User.Identity.Name;
            filter.Status = 1;
            return Success(this._service.GetUserVoucher(filter));
        }


        [HttpPost]
        public RespondData UpdateEvoucherStatus(View_EVoucherUserRS view_EVoucherUserRS)
        {
            View_EVoucherUserDTO view_EVoucherUserDTO = new View_EVoucherUserDTO();
            view_EVoucherUserDTO.Code = view_EVoucherUserRS.VoucherCode;
            view_EVoucherUserDTO.Status = view_EVoucherUserRS.Status;
            view_EVoucherUserDTO.StatusName = view_EVoucherUserRS.StatusName;

            this._service.UpdateEvoucherStatus(view_EVoucherUserDTO);
            return DefaultSuccess(string.Empty);
        }

        [HttpPost]
        public virtual RespondData Count(View_EVoucherUserRS view_EVoucherUserRS)
        {
            View_EVoucherUserPagingFilterModelDTO filter = new View_EVoucherUserPagingFilterModelDTO();
            filter.UserName = System.Web.HttpContext.Current.User.Identity.Name;

            filter.Status = 1;
            var result = this._service.Count(filter);
            return Success(result);
        }
    }
}