using BetterCms.Core.Modules;
using BetterCms.Core.Modules.Projections;
using BetterCms.Module.Root.Content.Resources;

using BetterModules.Core.Web.Modules;

namespace BetterCms.Module.Root.Registration
{
    public class RedirectJsModuleIncludeDescriptor : JsIncludeDescriptor
    {
        public RedirectJsModuleIncludeDescriptor(RootModuleDescriptor module)
            : base(module, "bcms.redirect")
        {

            Links = new IActionProjection[]
                {                       
                };

            Globalization = new IActionProjection[]
                {                    
                    new JavaScriptModuleGlobalization(this, "reloadingPageTitle", () => RootGlobalization.Reload_Title), 
                    new JavaScriptModuleGlobalization(this, "reloadingPageMessage", () => RootGlobalization.Reload_Message), 
                    new JavaScriptModuleGlobalization(this, "redirectingPageTitle", () => RootGlobalization.Redirect_Title),
                    new JavaScriptModuleGlobalization(this, "redirectingPageMessage", () => RootGlobalization.Redirect_Message)
                };
        }
    }
}