using System.Web.Mvc;
using System.Web.Routing;

namespace BetterCms.WebApi.Tests.App_Start
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("bcms-api/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Tests", action = "PagesWebApiTests", id = UrlParameter.Optional }
            );
        }
    }
}