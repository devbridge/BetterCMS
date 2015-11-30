// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RouteConfig.cs" company="Devbridge Group LLC">
// 
// Copyright (C) 2015,2016 Devbridge Group LLC
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public License
// along with this program.  If not, see http://www.gnu.org/licenses/. 
// </copyright>
// 
// <summary>
// Better CMS is a publishing focused and developer friendly .NET open source CMS.
// 
// Website: https://www.bettercms.com 
// GitHub: https://github.com/devbridge/bettercms
// Email: info@bettercms.com
// </summary>
// --------------------------------------------------------------------------------------------------------------------
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