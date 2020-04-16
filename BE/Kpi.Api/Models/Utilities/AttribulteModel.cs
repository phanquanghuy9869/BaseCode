using Kpi.Core.DTO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Kpi.Api.Models.Utilities
{
    public class AttribulteModel
    {
        public class StateValidationFilter : ActionFilterAttribute
        {
            /// <summary>
            /// Filter invalid model state
            /// </summary>        
            public override void OnActionExecuting(HttpActionContext actionContext)
            {
                var modelState = actionContext.ModelState;

                if (!modelState.IsValid)
                {
                    var errorMsg = new StringBuilder();

                    foreach (var state in modelState)
                    {
                        foreach (var error in state.Value.Errors)
                        {
                            errorMsg.Append(error.ErrorMessage);
                        }
                    }

                    actionContext.Response = actionContext.Request
                         .CreateResponse(HttpStatusCode.OK, new RespondData { IsSuccess = false, Message = errorMsg.ToString() });
                }
            }
        }

        public class ExceptionHanlderAtrribute : ExceptionFilterAttribute
        {

            public override void OnException(HttpActionExecutedContext context)
            {
                var request = context.ActionContext.Request;
                var exception = context.Exception;
                var response = new RespondData
                {
                    IsSuccess = false,
                    Message = String.Format("Có lỗi xảy ra: {0}", exception.Message)
                };
                context.Response = request.CreateResponse(HttpStatusCode.OK, response);

                // log 
                var url = context.Request.RequestUri;
                var controller = context.ActionContext.ControllerContext.Controller as ApiController;
                var currentUser = controller.User.Identity.Name;
                var args = context.ActionContext.ActionArguments;
                var argsStr = JsonConvert.SerializeObject(args);
                //_log.FatalFormat("Url: {0}, user: {1}, arg: {2} has exception {3}", url, currentUser, argsStr, exception);
            }
        }

        //public class JWTAuthenticationFilter : AuthorizationFilterAttribute
        //{
        //    private readonly IEnumerable<PermissionCode> _codes;

        //    public JWTAuthenticationFilter(IEnumerable<PermissionCode> codes)
        //    {
        //        this._codes = codes;
        //    }

        //    public override void OnAuthorization(HttpActionContext actionContext)
        //    {
        //        base.OnAuthorization(actionContext);
        //    }

        //    private string FetchFromHeader(HttpActionContext actionContext)
        //    {
        //        string requestToken = null;
        //        var authRequest = actionContext.Request.Headers.Authorization;
        //        if (authRequest != null)
        //        {
        //            requestToken = authRequest.Parameter;
        //        }
        //        return requestToken;
        //    }

        //    //public bool IsUserAuthorized(HttpActionContext actionContext)
        //    //{
        //    //    var controllerRef = actionContext.ControllerContext.Controller as BaseApiController;
        //    //    var roles = controllerRef.CurrentRole;
        //    //    var controllerName = controllerRef.ControllerContext.ControllerDescriptor.ControllerName;

        //    //    //var authHeader = FetchFromHeader(actionContext);

        //    //    //if (authHeader != null)
        //    //    //{
        //    //    //    //var auth = new AuthenticationModule();
        //    //    //    //JwtSecurityToken userPayloadToken = auth.GenerateUserClaimFromJWT(authHeader);

        //    //    //    //if (userPayloadToken != null)
        //    //    //    //{

        //    //    //    //    var identity = auth.PopulateUserIdentity(userPayloadToken);
        //    //    //    //    string[] roles = { "All" };
        //    //    //    //    var genericPrincipal = new GenericPrincipal(identity, roles);
        //    //    //    //    Thread.CurrentPrincipal = genericPrincipal;
        //    //    //    //    var authenticationIdentity = Thread.CurrentPrincipal.Identity as JWTAuthenticationIdentity;
        //    //    //    //    if (authenticationIdentity != null && !String.IsNullOrEmpty(authenticationIdentity.UserName))
        //    //    //    //    {
        //    //    //    //        authenticationIdentity.UserId = identity.UserId;
        //    //    //    //        authenticationIdentity.UserName = identity.UserName;
        //    //    //    //    }
        //    //    //    //    return true;
        //    //    //    //}

        //    //    //}
        //    //    //return false;
        //    //}
        //}

        //public class RequiredPermissionAttribute : AuthorizationFilterAttribute
        //{
        //    public PermissionCode Permission { get; set; }

        //    public RequiredPermissionAttribute()
        //    { }

        //    public override void OnAuthorization(HttpActionContext actionContext)
        //    {
        //        if (!HasPermission(actionContext))
        //        {
        //            var response = new RespondData
        //            {
        //                IsSuccess = false,
        //                Message = "Unauthorized",
        //            };
        //            // returns unauthorized error  
        //            actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized, response);
        //        }
        //        base.OnAuthorization(actionContext);
        //    }

        //    private bool HasPermission(HttpActionContext actionContext)
        //    {
        //        var controllerRef = actionContext.ControllerContext.Controller as BaseApiController;
        //        var authorizeModel = controllerRef.Authorization;
        //        switch (Permission)
        //        {
        //            case PermissionCode.Approve:
        //                return authorizeModel.CanApprove.GetValueOrDefault();
        //            case PermissionCode.Create:
        //                return authorizeModel.CanCreate.GetValueOrDefault();
        //            case PermissionCode.Delete:
        //                return authorizeModel.CanDelete.GetValueOrDefault();
        //            case PermissionCode.Edit:
        //                return authorizeModel.CanEdit.GetValueOrDefault();
        //            case PermissionCode.Feedback:
        //                return authorizeModel.CanFeedBack.GetValueOrDefault();
        //            case PermissionCode.ListContent:
        //                return authorizeModel.CanListContent.GetValueOrDefault();
        //            case PermissionCode.Read:
        //                return authorizeModel.CanRead.GetValueOrDefault();
        //            default:
        //                return false;
        //        }
        //    }
        //    //doing
        //}
    }
}