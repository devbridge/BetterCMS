using BetterCms.Core.Modules;
using BetterCms.Core.Modules.JsModule;
using BetterCms.Core.Modules.Projections;
using BetterCms.Module.Navigation.Content.Resources;
using BetterCms.Module.Navigation.Controllers;
using BetterCms.Module.Root.Content.Resources;

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
                    new JavaScriptModuleLinkTo<SitemapController>(this, "loadSiteSettingsSitemapUrl", c => c.Index(null)),
                    new JavaScriptModuleLinkTo<SitemapController>(this, "saveSitemapNodeUrl", c => c.SaveSitemapNode(null)),
                    new JavaScriptModuleLinkTo<SitemapController>(this, "deleteSitemapNodeUrl", c => c.DeleteSitemapNode(null)),
                    new JavaScriptModuleLinkTo<SitemapController>(this, "sitemapEditDialogUrl", c => c.EditSitemap()),
                    new JavaScriptModuleLinkTo<SitemapController>(this, "sitemapAddNewPageDialogUrl", c => c.AddNewPage())
                };

            Globalization = new IActionProjection[]
                {
                    new JavaScriptModuleGlobalization(this, "sitemapEditorDialogTitle", () => NavigationGlobalization.Sitemap_EditorDialog_Title),
                    new JavaScriptModuleGlobalization(this, "sitemapEditorDialogClose", () => RootGlobalization.Button_Close),
                    new JavaScriptModuleGlobalization(this, "sitemapEditorDialogCustomLinkTitle", () => NavigationGlobalization.Sitemap_EditorDialog_CustomLinkTitle),
                    new JavaScriptModuleGlobalization(this, "sitemapAddNewPageDialogTitle", () => NavigationGlobalization.Sitemap_AddNewPageDialog_Title),
                    new JavaScriptModuleGlobalization(this, "sitemapAddNewPageDialogClose", () => RootGlobalization.Button_Close),
                    new JavaScriptModuleGlobalization(this, "sitemapDeleteNodeConfirmationMessage", () => NavigationGlobalization.Sitemap_DeleteNode_Confirmation_Message)
                };
        }
    }
}