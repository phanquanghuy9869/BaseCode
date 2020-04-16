using Kpi.Api.Controllers.Base;
using Kpi.Core.DTO;
using Kpi.Service.DataServices.SysServices;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Kpi.Api.Controllers.Sys
{
    [Authorize]
    public class MenuController : BaseApiController<IMenuService>
    {
        public MenuController(IMenuService service) : base(service)
        {}
        
        [HttpPost]
        public RespondData GetMenu()
        {
            var isActived = this.AppUserManager.Users.Where(x => x.UserName == this.UserName).Select(x => x.IsActived).FirstOrDefault();
            if (!isActived) {
                return Fail("User chưa đổi password!");
            }
            return Success(this._service.GetMenuByRole(UserName));
        }
    }
}