using BetterCms.Core.Modules;
using BetterCms.Core.Modules.Projections;
using BetterCms.Module.MediaManager.Controllers;
using BetterCms.Module.MediaManager.Content.Resources;

namespace BetterCms.Module.MediaManager.Registration
{
    /// <summary>
    /// bcms.pages.js module descriptor.
    /// </summary>
    public class FileEditorJsModuleIncludeDescriptor : JsIncludeDescriptor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FileEditorJsModuleIncludeDescriptor" /> class.
        /// </summary>
        /// <param name="module">The container module.</param>
        public FileEditorJsModuleIncludeDescriptor(CmsModuleDescriptor module)
            : base(module, "bcms.media.fileeditor")
        {
            Links = new IActionProjection[]
                {
                    new JavaScriptModuleLinkTo<FilesController>(this, "fileEditorDialogUrl", c => c.FileEditor("{0}")),
                };

            Globalization = new IActionProjection[]
                {
                    new JavaScriptModuleGlobalization(this, "fileEditorDialogTitle", () => MediaGlobalization.FileEditor_Dialog_Title),
                    new JavaScriptModuleGlobalization(this, "fileEditorUpdateFailureMessageTitle", () => MediaGlobalization.FileEditor_UpdateFailureMessage_Title),
                    new JavaScriptModuleGlobalization(this, "fileEditorUpdateFailureMessageMessage", () => MediaGlobalization.FileEditor_UpdateFailureMessage_Message),
                    new JavaScriptModuleGlobalization(this, "fileEditorHasChangesMessage", () => MediaGlobalization.FileEditor_HasChanges_Message)
                };
        }
    }
}