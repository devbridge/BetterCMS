using BetterCms.Core.Modules;
using BetterCms.Core.Modules.Projections;
using BetterCms.Module.MediaManager.Content.Resources;
using BetterCms.Module.MediaManager.Controllers;

namespace BetterCms.Module.MediaManager.Registration
{
    /// <summary>
    /// bcms.pages.js module descriptor.
    /// </summary>
    public class MediaUploadJsModuleIncludeDescriptor : JsIncludeDescriptor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MediaManagerJsModuleIncludeDescriptor" /> class.
        /// </summary>
        /// <param name="module">The container module.</param>
        public MediaUploadJsModuleIncludeDescriptor(CmsModuleDescriptor module)
            : base(module, "bcms.media.upload")
        {            
            Links = new IActionProjection[]
                {    
                    new JavaScriptModuleLinkTo<UploadController>(this, "loadUploadFilesDialogUrl", f => f.MultiFileUpload("{0}", "{1}", "{2}")),
                    new JavaScriptModuleLinkTo<UploadController>(this, "uploadFileToServerUrl", f => f.UploadMedia(null)),
                    new JavaScriptModuleLinkTo<UploadController>(this, "undoFileUploadUrl", f => f.RemoveFileUpload("{0}", "{1}", "{2}")),
                    new JavaScriptModuleLinkTo<UploadController>(this, "loadUploadSingleFileDialogUrl", f => f.SingleFileUpload("{0}", "{1}", "{2}")),
                    new JavaScriptModuleLinkTo<UploadController>(this, "checkUploadedFileStatuses", f => f.CheckFilesStatuses(null))
                };

            Globalization = new IActionProjection[]
                {
                    new JavaScriptModuleGlobalization(this, "uploadFilesDialogTitle", () => MediaGlobalization.MultiFileUpload_DialogTitle),
                    new JavaScriptModuleGlobalization(this, "failedToProcessFile", () => MediaGlobalization.MediaManager_FailedToProcessFile_Message),
                    new JavaScriptModuleGlobalization(this, "multipleFilesWarningMessageOnReupload", () => MediaGlobalization.MediaManager_MultipleFilesWarning_Message)
                };
        }
    }
}