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
    public class ValidVoucherModel {
        public List<string> Model;
    }
   
    [Authorize]
    public class EvoucherCodeLineController : BaseApiController<IEVoucherCodeLineService>
    {
        public EvoucherCodeLineController(IEVoucherCodeLineService service) : base(service)
        { }
        
        [HttpPost]
        public RespondData GetUserVoucher(long EvoucheBudgetDetailId, int Status)
        {
            EvoucherCodeLinePagingFilterModelDTO filter = new EvoucherCodeLinePagingFilterModelDTO();
            filter.EvoucheBudgetDetailId = EvoucheBudgetDetailId;
            filter.Status = Status;
            return Success(this._service.GetUserVoucher(filter));
        }
                
        [HttpPost]
        public RespondData CheckExistedVoucherCode(List<string> model)
        {
            return Success(this._service.CheckExistedVoucherCode(model));
        }

        [HttpPost]
        public RespondData ImportVoucherModel(EVoucherImportModelDTO model)
        {
            model.CurrentUserName = UserName;
            this._service.ImportVoucherModel(model);
            return DefaultSuccess(string.Empty);
        }
    }
}