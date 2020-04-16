using Autofac;
using Kpi.Api.App_Start;
using Kpi.Api.Models;
using Kpi.Api.Models.Const;
using Kpi.Api.Models.OwinManager;
using Kpi.Core.DTO;
using Kpi.Service.DataServices.Org;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;
using System.Web.Http;

namespace Kpi.Api.Controllers.Base
{
    public class BaseApiController : ApiController
    {
        #region service
        private readonly ApplicationRoleManager _appRoleManager = null;
        protected ApplicationRoleManager AppRoleManager => _appRoleManager ?? Request.GetOwinContext().GetUserManager<ApplicationRoleManager>();

        private readonly ApplicationUserManager _appUserManager = null;
        protected ApplicationUserManager AppUserManager => _appUserManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();

        protected IPrincipal Principal => RequestContext.Principal;
        private IUserOrgService _userOrgService;
        protected IUserOrgService UserOrgService
        {
            get
            {
                if (_userOrgService == null)
                {
                    _userOrgService = AutofacConfig.Container.Resolve<IUserOrgService>();
                }
                return _userOrgService;
            }
        }
        #endregion

        #region property
        protected string UserId => (Principal as ClaimsPrincipal).Claims.Where(x => x.Type == ConstConfig.USER_ID_CLAIM).Select(x => x.Value).FirstOrDefault();
        protected string UserName => User.Identity.Name;
        protected ApplicationUser AppUser => AppUserManager.FindByName(this.UserName);
        protected int OrgId => Convert.ToInt32((Principal as ClaimsPrincipal).Claims.Where(x => x.Type == ConstConfig.ORG_ID_CLAIM).Select(x => x.Value).FirstOrDefault());

        private UserInfoModel _userOrg;
        protected UserInfoModel UserOrg
        {
            get
            {
                if (this._userOrg != null)
                {
                    return this._userOrg;
                }

                this._userOrg = this.UserOrgService.Get(this.UserName);
                return this._userOrg;
            }

        }
        #endregion

        private ApplicationSignInManager _signInManager;
        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? Request.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        protected ApplicationUserManager UserManager => Request.GetOwinContext().GetUserManager<ApplicationUserManager>();

        protected RespondData Success(object data)
        {
            return new RespondData { IsSuccess = true, Data = data };
        }

        protected RespondData DefaultSuccess(string msg)
        {
            return new RespondData { IsSuccess = true, Message = msg };
        }

        protected RespondData Fail(string message)
        {
            return new RespondData { IsSuccess = false, Message = message };
        }

        protected RespondData DefaultFail()
        {
            return new RespondData { IsSuccess = false, Message = "Có lỗi xảy ra , không thể thực hiện thao tác" };
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }

    public class BaseApiController<TService> : BaseApiController where TService : class
    {
        protected readonly TService _service;
        protected IUserOrgService _userOrgService;

        public BaseApiController(TService service)
        {
            _service = service;
        }

        public BaseApiController(TService service, IUserOrgService userOrgService)
        {
            _service = service;
            this._userOrgService = userOrgService;
        }

        protected virtual int? GetOrg()
        {
            var users = this._userOrgService.GetAllUsers().Where(x => !string.IsNullOrWhiteSpace(x.UserName)).ToList();
            var user = users.FirstOrDefault(
                            x => x.UserName.ToLower() == HttpContext.Current.User.Identity.Name.ToLower());
            return user != null ? user.OrgId : (int?)null;

        }
    }
}