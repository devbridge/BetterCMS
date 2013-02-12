using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace BetterCms.Sandbox.Mvc4
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute("SandboxController_Content", "sandbox/content", new { controller = "Sandbox", action = "Content" });
            routes.MapRoute("SandboxController_Hello", "sandbox/hello", new { controller = "Sandbox", action = "Hello" });
            routes.MapRoute("SandboxController_Widget05", "sandbox/widget05", new { controller = "Sandbox", action = "Widget05" });
            routes.MapRoute("SandboxController_TestApi", "sandbox/testapi", new { controller = "Sandbox", action = "TestApi" });

            routes.MapRoute("login", "login", new { controller = "Sandbox", action = "Login" });
            routes.MapRoute("logout", "logout", new { controller = "Sandbox", action = "Logout" });
        }
    }
}