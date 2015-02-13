using System.Web.Mvc;
using System.Web.Routing;

namespace BetterCms.Sandbox.Mvc4
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            //routes.MapRoute("SandboxController_Content", "sandbox/content", new { controller = "Sandbox", action = "Content" });
            //routes.MapRoute("SandboxController_Hello", "sandbox/hello", new { controller = "Sandbox", action = "Hello" });
            //routes.MapRoute("SandboxController_Widget05", "sandbox/widget05", new { controller = "Sandbox", action = "Widget05" });

            routes.MapRoute("SandboxController_Upload",
                "sandbox/upload",
                new { controller = "Upload", action = "Upload" },
                new[] { "BetterCms.Sandbox.Mvc4.Controllers" });
            
            routes.MapRoute("SandboxController_TestApi", "sandbox/{action}", new { controller = "Sandbox", action = "TestApi" });

            routes.MapRoute("SandboxController_TestRewrite", "sandbox/test-rewrite/{*url}", new { controller = "Sandbox", action = "TestRewrite" });

            // Widgets inheritance testing
            routes.MapRoute("WidgetController_TestPostAndGet", 
                "cmswidget/testpostandget", 
                new { controller = "Widgets", action = "TestPostAndGet" },
                new[] { "BetterCms.Sandbox.Mvc4.Controllers" });
            
            routes.MapRoute("WidgetController_TestPostAndGetWithInheritance", 
                "cmswidget/testpostandgetwithinheritance", 
                new { controller = "Widgets", action = "TestPostAndGetWithInheritance" },
                new[] { "BetterCms.Sandbox.Mvc4.Controllers" });

            // Blog post tests
            routes.MapRoute("BlogController_TestIndex", 
                "testblogindex", 
                new { controller = "Blog",  action = "Index" },
                new[] { "BetterCms.Sandbox.Mvc4.Controllers" });

            routes.MapRoute(
                "Upload",
                "Widgets/MyFileUploadWidget_Upload",
                new { controller = "Widgets", action = "MyFileUploadWidget_Upload" },
                new[] { "BetterCms.Sandbox.Mvc4.Controllers" });

            routes.MapRoute("WidgetController_ApiTestWidget",
                "cmswidget/apitestwidget",
                new { controller = "Widgets", action = "ApiTestWidget" },
                new[] { "BetterCms.Sandbox.Mvc4.Controllers" });
        }
    }
}