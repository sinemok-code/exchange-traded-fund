using System.Web.Configuration;

namespace ETF.Web.Common
{
    public static class Reference
    {
        public enum StatusCode
        {
            Ok = 200,
            Unauthorised = 401,
            Forbidden = 403,
            NotFound = 404,
            Error = 500
        }

        public struct Dictionary
        {
            public static string ApiUri = WebConfigurationManager.AppSettings["APIUrl"];

            public static string PageNotFoundError = "Page not found";
            public static string InternalError = "An internal error has occurred.";
        }
    }
}
