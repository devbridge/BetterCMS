using BetterCms.Core.Modules;
using BetterCms.Core.Modules.JsModule;
using BetterCms.Core.Modules.Projections;
using BetterCms.Module.Root.Content.Resources;
using BetterCms.Module.Root.Controllers;

namespace BetterCms.Module.Root.Registration
{
    public class SiteSettingsJavaScriptModuleDescriptor : JavaScriptModuleDescriptor
    {
        public SiteSettingsJavaScriptModuleDescriptor(ModuleDescriptor containerModule)
            : base(containerModule, "bcms.siteSettings", "/file/bcms-root/scripts/bcms.siteSettings")
        {

            Links = new IActionProjection[]
                {
                    new JavaScriptModuleLinkTo<SiteSettingsController>(this, "loadSiteSettingsUrl", c => c.Container())
                };

            Globalization = new IActionProjection[]
                {
                    new JavaScriptModuleGlobalization(this, "siteSettingsTitle", () => RootGlobalization.SiteSettings_Title), 
                };
        }
    }
}