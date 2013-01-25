using BetterCms.Core.Modules;
using BetterCms.Core.Modules.JsModule;
using BetterCms.Core.Modules.Projections;
using BetterCms.Module.Pages.Command.History.GetContentHistory;
using BetterCms.Module.Pages.Content.Resources;
using BetterCms.Module.Pages.Controllers;

namespace BetterCms.Module.Pages.Registration
{
    /// <summary>
    /// bcms.pages.seo.js module descriptor.
    /// </summary>
    public class HistoryJavaScriptModuleDescriptor : JavaScriptModuleDescriptor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SeoJavaScriptModuleDescriptor" /> class.
        /// </summary>
        /// <param name="containerModule">The container module.</param>
        public HistoryJavaScriptModuleDescriptor(ModuleDescriptor containerModule)
            : base(containerModule, "bcms.pages.history", "/file/bcms-pages/scripts/bcms.pages.history")
        {

            Links = new IActionProjection[]
                {
                    new JavaScriptModuleLinkTo<HistoryController>(this, "loadPageContentHistoryDialogUrl", controller => controller.PageContentHistory("{0}", "{1}")),
                    new JavaScriptModuleLinkTo<HistoryController>(this, "loadPageContentHistoryUrl", controller => controller.PageContentHistory(null)),
                    new JavaScriptModuleLinkTo<HistoryController>(this, "loadPageContentVersionPreviewUrl", controller => controller.PageContentVersion("{0}")),
                    new JavaScriptModuleLinkTo<HistoryController>(this, "restorePageContentVersionUrl", controller => controller.RestorePageContentVersion("{0}")),
                };

            Globalization = new IActionProjection[]
                {
                     new JavaScriptModuleGlobalization(this, "pageContentHistoryDialogTitle", () => PagesGlobalization.PageContentHistory_DialogTitle),
                     new JavaScriptModuleGlobalization(this, "pageContentVersionRestoryConfirmation", () => PagesGlobalization.PageContentVersion_Restore_ConfirmationMessage)
                };
        }
    }
}