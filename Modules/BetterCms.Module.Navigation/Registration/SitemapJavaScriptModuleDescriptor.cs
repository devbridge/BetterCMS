using BetterCms.Core.Modules;
using BetterCms.Core.Modules.JsModule;
using BetterCms.Core.Modules.Projections;
using BetterCms.Module.Navigation.Controllers;

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
            : base(containerModule, "bcms.sitemap", "/file/bcms-navigation/scripts/bcms.sitemap")
        {            
            Links = new IActionProjection[]
                {
                    new JavaScriptModuleLinkTo<SitemapController>(this, "loadSiteSettingsSitemapUrl", c => c.Index(null))
                };

            Globalization = new IActionProjection[]
                {
                    // TODO:
                    // new JavaScriptModuleGlobalization(this, "addNewPageDialogTitle", () => PagesGlobalization.AddNewPage_Dialog_Title),
                };
        }
    }
}