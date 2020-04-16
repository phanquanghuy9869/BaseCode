using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Kpi.Api.Configs
{
    public static class AppConfigs
    {
        public const string ROLE_CLAIM = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role";
        public const string NAME_CLAIM = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name";
        //public const string NAME_CLAIM = "username";
        public const string ID_CLAIM = "user_id";
        public const string FULL_NAME = "fullname";
        public const string ROLE_ID_CLAIM = "role_id";
        public const string JOB_TITLE_CLAIM = "job_title";
        public const string JOB_TITLE_ID = "job_title";
        public const string ORG_ID_CLAIM = "org_id";
        public const string MENU_CLAIM = "menu";
        public const string MENU_ID_CLAIM = "menu_id";
        public const int MAX_PAGE_LENGTH = 500;
        public static string AssetPath = ConfigurationManager.AppSettings["AssetPath"];
        public static int TokenExpireTimeSpan = Convert.ToInt32(ConfigurationManager.AppSettings["TokenExpireTimeSpan"]);
        public const string API_SUCCESS_MSG = "Thay đổi thành công";
    }
}