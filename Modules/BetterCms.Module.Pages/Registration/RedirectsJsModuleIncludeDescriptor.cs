using BetterCms.Core.Modules;
using BetterCms.Core.Modules.Projections;
using BetterCms.Module.Pages.Content.Resources;
using BetterCms.Module.Pages.Controllers;

namespace BetterCms.Module.Pages.Registration
{
    public class RedirectsJsModuleIncludeDescriptor : JsIncludeDescriptor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RedirectsJsModuleIncludeDescriptor" /> class.
        /// </summary>
        /// <param name="module">The container module.</param>
        public RedirectsJsModuleIncludeDescriptor(CmsModuleDescriptor module)
            : base(module, "bcms.pages.redirects")
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