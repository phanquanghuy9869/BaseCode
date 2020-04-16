using Kpi.Api.App_Start;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Kpi.Api.Configs;
using Autofac;
using Kpi.Service.DataServices.SysServices;
using Kpi.Core.DTO;
using Microsoft.Owin.Security.Jwt;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Kpi.Api.Models.OwinManager;
using Kpi.Service.ClientServices;
using Kpi.Api.Models;
using System.Net;
using System.Web;

namespace Kpi.Api.Providers
{

    public class JWTOAuthProvider : OAuthAuthorizationServerProvider, IDisposable
    {
        //private static readonly ILog _log = LogManager.GetLogger("Exception");

        private class LoginModel
        {
            public string Username { get; set; }
            public string Password { get; set; }
            public string Grant_Type { get; set; }
        }

        public JWTOAuthProvider()
        { }

        //add data to token
        public override Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            try
            {
                using (var localScope = AutofacConfig.Container.BeginLifetimeScope())
                {
                    var identity = new ClaimsIdentity("otc");
                    var username = context.OwinContext.Get<string>("otc:username");
                    var userManager = context.OwinContext.GetUserManager<ApplicationUserManager>();
                    var user = userManager.FindByName(username);
                    //username
                    identity.AddClaim(new Claim(Configs.AppConfigs.NAME_CLAIM, username));
                    identity.AddClaim(new Claim(Configs.AppConfigs.FULL_NAME, user.Name));
                    
                    ////menu claim
                    //var roles = info.Select(x => x.RoleID).Distinct().ToList();
                    IEnumerable<MenuDTO> currentMenus = null;
                    using (var menuService = localScope.Resolve<IMenuService>())
                    {
                        currentMenus = menuService.GetAll();
                    }
                    var menuClaim = currentMenus.Where(x => x.Url != null && x.Url != string.Empty).Select(x => new Claim(Configs.AppConfigs.MENU_CLAIM, x.Url));
                    if (menuClaim != null)
                    {
                        identity.AddClaims(menuClaim);
                    }

                    var menuIdClaim = currentMenus.Where(x => x.Url != null && x.Url != string.Empty).Select(x => new Claim(Configs.AppConfigs.MENU_ID_CLAIM, x.Id.ToString()));
                    if (menuIdClaim != null)
                    {
                        identity.AddClaims(menuIdClaim);
                    }

                    context.Validated(identity);
                    //context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });
                    return Task.FromResult(0);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //receive username password
        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            try
            {
                using (var localScope = AutofacConfig.Container.BeginLifetimeScope())
                {
                    var userManager = context.OwinContext.GetUserManager<ApplicationUserManager>();
                    var username = context.Parameters["username"];
                    var password = HttpUtility.UrlDecode(context.Parameters["password"]);

                    // huypq modified 9/12/19 add login ad
                    ApplicationUser user = null;
                    using (var adLoginService = localScope.Resolve<IClientLoginAdService>())
                    {
                        var isLoginAdSuccess = false;
                        try
                        {
                            user = userManager.FindByName(username);

                            // tài khoản ko tồn tại trong hệ thống
                            if (user == null)
                            {
                                context.SetError("Tài khoản không tồn tại");
                                //context.Rejected();
                                context.Response.Headers.Add("AuthorizationResponse", new[] { "Failed" });
                                return Task.FromResult(0);
                            }
                            // check ad    
                            isLoginAdSuccess = Task.Run(() => adLoginService.PostLoginAsync(username, password)).GetAwaiter().GetResult().IsSuccessStatusCode;

                        } catch (Exception ex)
                        {
                            isLoginAdSuccess = false;
                        }

                        // tài khoản ko có trên ad => check database
                        if (!isLoginAdSuccess)
                        {
                            if (!userManager.CheckPassword(user, password))
                            {
                                context.SetError("Tài khoản không hợp lệ");
                                context.Response.Headers.Add("AuthorizationResponse", new[] { "Failed" });
                                return Task.FromResult(0);
                            }
                        }
                    }                              

                    context.OwinContext.Set("otc:username", username);
                    context.OwinContext.Set("otc:userid", user.Id);
                    context.Validated();
                    //context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });
                    return Task.FromResult(0);
                }
            }
            catch (Exception ex)
            {
                context.SetError("Server error");
                context.Rejected();
                return Task.FromResult(0);
            }
        }

        public override Task MatchEndpoint(OAuthMatchEndpointContext context)
        {
            if (context.IsTokenEndpoint && context.Request.Method == "OPTIONS")
            {
                context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });
                context.OwinContext.Response.Headers.Add("Access-Control-Allow-Headers", new[] { "Content-Type" });
                context.RequestCompleted();
                return Task.FromResult(0);
            }

            return base.MatchEndpoint(context);
        }

        public void Dispose()
        {
        }
    }


    /// <summary>
    /// HuyPQ modified 28/6/2019, code to validate token
    /// https://stackoverflow.com/questions/21484982/asp-net-web-api-how-to-pass-an-access-token-oauth-2-0-using-url-parameter/21496536
    /// </summary>
    public class QueryStringOAuthBearerProvider : OAuthBearerAuthenticationProvider
    {
        readonly string _name;

        public QueryStringOAuthBearerProvider(string name)
        {
            _name = name;
        }

        public override Task RequestToken(OAuthRequestTokenContext context)
        {
            var value = context.Request.Query.Get(_name);

            if (!string.IsNullOrEmpty(value))
            {
                context.Token = value;
            }

            return Task.FromResult<object>(null);
        }
    }

    public class JWTAuthenticationOption : JwtBearerAuthenticationOptions
    {
        public JWTAuthenticationOption()
        {
            var issuer = "localhost";
            var audience = "all";
            var key = Convert.FromBase64String("UHxNtYMRYwvfpO1dS5pWLKL0M2DgOj40EbN4SoBWgfc==");
            AllowedAudiences = new[] { audience };
            IssuerSecurityKeyProviders = new[]
            {new SymmetricKeyIssuerSecurityKeyProvider(issuer, key)};

            // huypq modified 28-06-2019, try to access code to validate token
            Provider = new QueryStringOAuthBearerProvider("Authorization");
        }
    }

    public class JWTAuthorizationOption : OAuthAuthorizationServerOptions
    {
        public JWTAuthorizationOption()
        {
            TokenEndpointPath = new PathString("/oauth/token");
            AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(Configs.AppConfigs.TokenExpireTimeSpan);
            AccessTokenFormat = new JWTAuthorizationFormat();
            Provider = new JWTOAuthProvider();
            AllowInsecureHttp = true;
        }
    }

    public class JWTAuthorizationFormat : ISecureDataFormat<AuthenticationTicket>
    {
        public JWTAuthorizationFormat()
        {
        }

        public string SignatureAlgorithm
        {
            get { return "http://www.w3.org/2001/04/xmldsig-more#hmac-sha256"; }
        }

        public string DigestAlgorithm
        {
            get { return "http://www.w3.org/2001/04/xmlenc#sha256"; }
        }

        public string Protect(AuthenticationTicket data)
        {
            if (data == null) throw new ArgumentNullException("data");

            var issuer = "localhost";
            var audience = "all";
            var key = Convert.FromBase64String("UHxNtYMRYwvfpO1dS5pWLKL0M2DgOj40EbN4SoBWgfc==");
            var now = DateTime.UtcNow;
            var expires = now.AddMinutes(TimeSpan.FromMinutes(60).TotalMinutes);
            var signingCredentials = new Microsoft.IdentityModel.Tokens.SigningCredentials(
                                        new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(key),
                                        SecurityAlgorithms.HmacSha256Signature, DigestAlgorithm);
            var token = new JwtSecurityToken(issuer, audience, data.Identity.Claims,
                                             now, expires, signingCredentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public AuthenticationTicket Unprotect(string protectedText)
        {
            throw new NotImplementedException();
        }
    }
}