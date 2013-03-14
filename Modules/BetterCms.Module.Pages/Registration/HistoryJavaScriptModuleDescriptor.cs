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
    public class HistoryJavaScriptModuleDescriptor : JavaScriptModuleDescriptor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SeoJavaScriptModuleDescriptor" /> class.
        /// </summary>
        /// <param name="containerModule">The container module.</param>
        public HistoryJavaScriptModuleDescriptor(ModuleDescriptor containerModule)
            : base(containerModule, "bcms.pages.history")
        {

            Links = new IActionProjection[]
                {
                    new JavaScriptModuleLinkTo<HistoryController>(this, "loadContentHistoryDialogUrl", controller => controller.ContentHistory("{0}")),
                    new JavaScriptModuleLinkTo<HistoryController>(this, "loadContentVersionPreviewUrl", controller => controller.ContentVersion("{0}")),
                    new JavaScriptModuleLinkTo<HistoryController>(this, "restoreContentVersionUrl", controller => controller.RestorePageContentVersion("{0}")),
                    new JavaScriptModuleLinkTo<HistoryController>(this, "destroyContentDraftVersionUrl", controller => controller.DestroyContentDraft("{0}"))
                };

            Globalization = new IActionProjection[]
                {
                     new JavaScriptModuleGlobalization(this, "contentHistoryDialogTitle", () => PagesGlobalization.ContentHistory_DialogTitle),
                     new JavaScriptModuleGlobalization(this, "contentVersionRestoreConfirmation", () => PagesGlobalization.ContentHistory_Restore_ConfirmationMessage),
                     new JavaScriptModuleGlobalization(this, "contentVersionDestroyDraftConfirmation", () => PagesGlobalization.ContentHistory_DestroyDraft_ConfirmationMessage),
                     new JavaScriptModuleGlobalization(this, "restoreButtonTitle", () => PagesGlobalization.ContentHistory_Restore_AcceptButtonTitle),
                     new JavaScriptModuleGlobalization(this, "destroyButtonTitle", () => PagesGlobalization.ContentHistory_Destroy_AcceptButtonTitle)
                };
        }
    }
}