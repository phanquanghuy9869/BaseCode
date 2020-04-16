using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kpi.Api.Models.Const
{
    public class ConstConfig
    {
        public const string ROLE_CLAIM = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role";
        public const string NAME_CLAIM = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name";
        public const string USER_ID_CLAIM = "user_id";
        public const string ROLE_ID_CLAIM = "role_id";
        public const string JOB_TITLE_CLAIM = "job_title";
        public const string JOB_TITLE_ID = "job_title";
        public const string ORG_ID_CLAIM = "org_id";
        public const string MENU_CLAIM = "menu";
        public const string MENU_ID_CLAIM = "menu_id";
        public const int TOTAL_KPI_POINT = 100;
        public const int MAX_PAGE_LENGTH = 500;
    }
}