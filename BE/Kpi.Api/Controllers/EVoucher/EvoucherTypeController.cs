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

    [Authorize]
    public class EvoucherTypeController : BaseGridApiController<IEvoucherTypeService, EvoucherTypeViewModel, EvoucherTypeDTO, EvoucherTypePagingFilterModel, EvoucherTypePagingFilterModelDTO>
    {
        public EvoucherTypeController(IEvoucherTypeService service, IUserOrgService userOrgService) : base(service)
        {
        }

        public override RespondData Get(int id)
        {
            var filter = new EvoucherTypePagingFilterModelDTO()
            {
                Id = id,
                CompanyId = this.GetOrg()
            };
            var result = _service.GetOneVoucherType(filter);
            return Success(result);
        }

        public override RespondData GetPaging(EvoucherTypePagingFilterModel filter)
        {
            filter.CompanyId = this.GetOrg();
            return base.GetPaging(filter);
        }
    }
}