using BetterCms.Core.Modules;
using BetterCms.Core.Modules.JsModule;
using BetterCms.Core.Modules.Projections;

namespace BetterCms.Module.Navigation.Registration
{
    /// <summary>
    /// bcms.sitemap.js module descriptor.
    /// </summary>
    public class SitemapJavaScriptModuleDescriptor : JavaScriptModuleDescriptor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SitemapJavaScriptModuleDescriptor" /> class.
        /// </summary>
        /// <param name="containerModule">The container module.</param>
        public SitemapJavaScriptModuleDescriptor(ModuleDescriptor containerModule)
            : base(containerModule, "bcms.sitemap", "/file/bcms-sitemap/scripts/bcms.sitemap")
        {            
            Links = new IActionProjection[]
                {
                    // TODO:
                    // new JavaScriptModuleLinkTo<PageController>(this, "loadAddNewPageDialogUrl", c => c.AddNewPage()),
                };

            Globalization = new IActionProjection[]
                {
                    // TODO:
                    // new JavaScriptModuleGlobalization(this, "addNewPageDialogTitle", () => PagesGlobalization.AddNewPage_Dialog_Title),
                };
        }
    }
}