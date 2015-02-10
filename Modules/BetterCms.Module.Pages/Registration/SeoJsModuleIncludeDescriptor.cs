using BetterCms.Core.Modules;
using BetterCms.Core.Modules.Projections;
using BetterCms.Module.Pages.Content.Resources;
using BetterCms.Module.Pages.Controllers;

namespace BetterCms.Module.Pages.Registration
{
    /// <summary>
    /// bcms.pages.seo.js module descriptor.
    /// </summary>
    public class SeoJsModuleIncludeDescriptor : JsIncludeDescriptor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SeoJsModuleIncludeDescriptor" /> class.
        /// </summary>
        /// <param name="module">The container module.</param>
        public SeoJsModuleIncludeDescriptor(CmsModuleDescriptor module)
            : base(module, "bcms.pages.seo")
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