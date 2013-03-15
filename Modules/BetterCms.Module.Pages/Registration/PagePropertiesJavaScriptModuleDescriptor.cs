using BetterCms.Core.Modules;
using BetterCms.Core.Modules.JsModule;
using BetterCms.Core.Modules.Projections;
using BetterCms.Module.Pages.Content.Resources;
using BetterCms.Module.Pages.Controllers;

namespace BetterCms.Module.Pages.Registration
{
    /// <summary>
    /// bcms.pages.properties.js module descriptor.
    /// </summary>
    public class PagePropertiesJavaScriptModuleDescriptor : JavaScriptModuleDescriptor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PagePropertiesJavaScriptModuleDescriptor" /> class.
        /// </summary>
        /// <param name="containerModule">The container module.</param>
        public PagePropertiesJavaScriptModuleDescriptor(ModuleDescriptor containerModule)
            : base(containerModule, "bcms.pages.properties")
        {

            Links = new IActionProjection[]
                {
                    new JavaScriptModuleLinkTo<PageController>(this, "loadEditPropertiesDialogUrl", c => c.EditPageProperties("{0}"))
                };

            Globalization = new IActionProjection[]
                {
                    new JavaScriptModuleGlobalization(this, "editPagePropertiesModalTitle", () => PagesGlobalization.EditPageProperties_Title),
                };
        }
    }
}