using BetterCms.Core.Modules;
using BetterCms.Core.Modules.Projections;
using BetterCms.Module.Root.Content.Resources;
using BetterCms.Module.Root.Controllers;

using BetterModules.Core.Web.Modules;

namespace BetterCms.Module.Root.Registration
{
    public class SiteSettingsJsModuleIncludeDescriptor : JsIncludeDescriptor
    {
        public SiteSettingsJsModuleIncludeDescriptor(RootModuleDescriptor module)
            : base(module, "bcms.siteSettings")
        {

            Links = new IActionProjection[]
                {
                    new JavaScriptModuleLinkTo<SiteSettingsController>(this, "loadSiteSettingsUrl", c => c.Container())
                };

            Globalization = new IActionProjection[]
                {
                    new JavaScriptModuleGlobalization(this, "siteSettingsTitle", () => RootGlobalization.SiteSettings_Title), 
                    new JavaScriptModuleGlobalization(this, "closeButtonTitle", () => RootGlobalization.Button_Close), 
                };
        }
    }
}