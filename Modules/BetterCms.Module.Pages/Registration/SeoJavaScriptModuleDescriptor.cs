using BetterCms.Core.Modules;
using BetterCms.Core.Modules.JsModule;
using BetterCms.Core.Modules.Projections;
using BetterCms.Module.Pages.Content.Resources;
using BetterCms.Module.Pages.Controllers;

namespace BetterCms.Module.Pages.Registration
{
    /// <summary>
    /// bcms.pages.seo.js module descriptor.
    /// </summary>
    public class SeoJavaScriptModuleDescriptor : JavaScriptModuleDescriptor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SeoJavaScriptModuleDescriptor" /> class.
        /// </summary>
        /// <param name="containerModule">The container module.</param>
        public SeoJavaScriptModuleDescriptor(ModuleDescriptor containerModule)
            : base(containerModule, "bcms.pages.seo")
        {

            Links = new IActionProjection[]
                {
                    new JavaScriptModuleLinkTo<SeoController>(this, "loadEditSeoDialogUrl", controller => controller.EditSeo("{0}"))     
                };

            Globalization = new IActionProjection[]
                {
                     new JavaScriptModuleGlobalization(this, "editSeoDialogTitle", () => PagesGlobalization.EditSeo_Dialog_Title)                     
                };
        }
    }
}