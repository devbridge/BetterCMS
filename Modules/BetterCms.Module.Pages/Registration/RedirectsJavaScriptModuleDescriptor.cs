using BetterCms.Core.Modules;
using BetterCms.Core.Modules.JsModule;
using BetterCms.Core.Modules.Projections;
using BetterCms.Module.Pages.Content.Resources;
using BetterCms.Module.Pages.Controllers;

namespace BetterCms.Module.Pages.Registration
{
    public class RedirectsJavaScriptModuleDescriptor : JavaScriptModuleDescriptor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RedirectsJavaScriptModuleDescriptor" /> class.
        /// </summary>
        /// <param name="containerModule">The container module.</param>
        public RedirectsJavaScriptModuleDescriptor(ModuleDescriptor containerModule)
            : base(containerModule, "bcms.pages.redirects")
        {

            Links = new IActionProjection[]
                {
                    new JavaScriptModuleLinkTo<RedirectController>(this, "loadSiteSettingsRedirectListUrl", c => c.Redirects(null)),
                    new JavaScriptModuleLinkTo<RedirectController>(this, "deleteRedirectUrl", c => c.DeleteRedirect(null)),
                    new JavaScriptModuleLinkTo<RedirectController>(this, "saveRedirectUrl", c => c.SaveRedirect(null))
                };

            Globalization = new IActionProjection[]
                {      
                    new JavaScriptModuleGlobalization(this, "deleteRedirectMessage", () => PagesGlobalization.DeleteRedirect_Confirmation_Message)
                };
        }
    }
}