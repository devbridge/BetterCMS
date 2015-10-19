using BetterCms.Core.Modules;
using BetterCms.Core.Modules.Projections;
using BetterCms.Module.Root.Content.Resources;
using BetterCms.Module.Root.Controllers;

using BetterModules.Core.Web.Modules;
using BetterModules.Core.Web.Mvc.Extensions;
using Microsoft.Framework.Logging;

namespace BetterCms.Module.Root.Registration
{
    public class SiteSettingsJsModuleIncludeDescriptor : JsIncludeDescriptor
    {
        public SiteSettingsJsModuleIncludeDescriptor(RootModuleDescriptor module, ILoggerFactory loggerFactory, IControllerExtensions controllerExtensions)
            : base(module, "bcms.siteSettings")
        {

            Links = new IActionUrlProjection[]
                {
                    new JavaScriptModuleLinkTo<SiteSettingsController>(this, "loadSiteSettingsUrl", c => c.Container(), loggerFactory, controllerExtensions)
                };

            Globalization = new IActionProjection[]
                {
                    new JavaScriptModuleGlobalization(this, "siteSettingsTitle", () => RootGlobalization.SiteSettings_Title, loggerFactory), 
                    new JavaScriptModuleGlobalization(this, "closeButtonTitle", () => RootGlobalization.Button_Close, loggerFactory) 
                };
        }
    }
}