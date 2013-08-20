using System.Web.Mvc;
using System.Web.Routing;

using BetterCms.Api.Tests.Controllers;

namespace BetterCms.Api.Tests.App_Start
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.IgnoreRoute("bcms-api/{*pathInfo}");

            routes.MapRoute(
                "ResetData",
                "test/reset-data",
                new { controller = "DataMaintenance", action = "RegisterNewDataSet", area = "" }
            );

            routes.MapRoute(
                "Login",
                "test/login",
                new { controller = "Authentication", action = "Login", area = "" },
                null,
                new[] { typeof(AuthenticationController).Namespace }
            );
        }
    }
}