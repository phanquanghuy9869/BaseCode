using Kpi.Api.App_Start;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Kpi.Api
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AutofacConfig.Register();
            AutoMapperConfig.Register();
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            //https://stackoverflow.com/questions/22130431/web-api-serialize-properties-starting-from-lowercase-letter
            GlobalConfiguration.Configuration
               .Formatters
               .JsonFormatter
               .SerializerSettings
               .ContractResolver = new CamelCasePropertyNamesContractResolver();

            GlobalConfiguration.Configuration
                .Formatters
                .JsonFormatter
                .SerializerSettings
                .NullValueHandling = NullValueHandling.Ignore;
        }
    }
}
