using BetterCms.Core.Modules;
using BetterCms.Core.Modules.Projections;
using BetterCms.Module.MediaManager.Controllers;
using BetterCms.Module.MediaManager.Content.Resources;
using BetterCms.Module.Root.Content.Resources;

namespace BetterCms.Module.MediaManager.Registration
{
    /// <summary>
    /// bcms.pages.js module descriptor.
    /// </summary>
    public class MediaHistoryJsModuleIncludeDescriptor : JsIncludeDescriptor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MediaHistoryJsModuleIncludeDescriptor" /> class.
        /// </summary>
        /// <param name="module">The container module.</param>
        public MediaHistoryJsModuleIncludeDescriptor(CmsModuleDescriptor module)
            : base(module, "bcms.media.history")
        {
            Links = new IActionProjection[]
                {
                    new JavaScriptModuleLinkTo<HistoryController>(this, "loadMediaHistoryDialogUrl", c => c.MediaHistory("{0}")),
                    new JavaScriptModuleLinkTo<HistoryController>(this, "loadMediaVersionPreviewUrl", c => c.MediaVersion("{0}")),
                    new JavaScriptModuleLinkTo<HistoryController>(this, "restoreMediaVersionUrl", c => c.RestoreMediaVersion("{0}", "{1}")),
                    new JavaScriptModuleLinkTo<FilesController>(this, "downloadFileUrl", c => c.Download("{0}"))
                };

            Globalization = new IActionProjection[]
                {
                    new JavaScriptModuleGlobalization(this, "mediaHistoryDialogTitle", () => MediaGlobalization.MediaHistory_DialogTitle),
                    new JavaScriptModuleGlobalization(this, "mediaVersionRestoreConfirmation", () => MediaGlobalization.MediaHistory_Restore_ConfirmationMessage),
                    new JavaScriptModuleGlobalization(this, "restoreButtonTitle", () => MediaGlobalization.MediaHistory_Restore_AcceptButtonTitle),
                    new JavaScriptModuleGlobalization(this, "restoreWithOverrideButtonTitle", () => MediaGlobalization.MediaHistory_RestoreWithOverride_AcceptButtonTitle),
                    new JavaScriptModuleGlobalization(this, "restoreAsNewVersionButtonTitle", () => MediaGlobalization.MediaHistory_RestoreAsNewVersion_AcceptButtonTitle),
                    new JavaScriptModuleGlobalization(this, "closeButtonTitle", () => RootGlobalization.Button_Close)
                };
        }
    }
}