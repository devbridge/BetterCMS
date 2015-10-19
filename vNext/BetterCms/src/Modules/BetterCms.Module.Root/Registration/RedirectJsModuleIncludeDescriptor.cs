using BetterCms.Core.Modules;
using BetterCms.Core.Modules.Projections;
using BetterCms.Module.Root.Content.Resources;

using BetterModules.Core.Web.Modules;
using Microsoft.Framework.Logging;

namespace BetterCms.Module.Root.Registration
{
    public class RedirectJsModuleIncludeDescriptor : JsIncludeDescriptor
    {
        public RedirectJsModuleIncludeDescriptor(RootModuleDescriptor module, ILoggerFactory loggerFactory)
            : base(module, "bcms.redirect")
        {

            Links = new IActionUrlProjection[]
                {                       
                };

            Globalization = new IActionProjection[]
                {                    
                    new JavaScriptModuleGlobalization(this, "reloadingPageTitle", () => RootGlobalization.Reload_Title, loggerFactory), 
                    new JavaScriptModuleGlobalization(this, "reloadingPageMessage", () => RootGlobalization.Reload_Message, loggerFactory), 
                    new JavaScriptModuleGlobalization(this, "redirectingPageTitle", () => RootGlobalization.Redirect_Title, loggerFactory),
                    new JavaScriptModuleGlobalization(this, "redirectingPageMessage", () => RootGlobalization.Redirect_Message, loggerFactory)
                };
        }
    }
}