﻿using Kpi.Api.Controllers.Base;
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

namespace Kpi.Api.Controllers.Org
{

    [Authorize]
    public class EmploymentHistoryController : BaseGridApiController<IEmploymentHistoryService, EmploymentHistoryRS, EmploymentHistoryDTO, EmploymentHistoryFilterModel, EmploymentHistoryFilterModelDTO>
    {
        public EmploymentHistoryController(IEmploymentHistoryService service) : base(service)
        { }
    }
}