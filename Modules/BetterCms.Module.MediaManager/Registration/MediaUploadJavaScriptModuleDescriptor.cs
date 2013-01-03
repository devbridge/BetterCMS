using BetterCms.Core.Modules;
using BetterCms.Core.Modules.JsModule;
using BetterCms.Core.Modules.Projections;
using BetterCms.Module.MediaManager.Controllers;

namespace BetterCms.Module.MediaManager.Registration
{
    /// <summary>
    /// bcms.pages.js module descriptor.
    /// </summary>
    public class MediaUploadJavaScriptModuleDescriptor : JavaScriptModuleDescriptor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MediaManagerJavaScriptModuleDescriptor" /> class.
        /// </summary>
        /// <param name="containerModule">The container module.</param>
        public MediaUploadJavaScriptModuleDescriptor(ModuleDescriptor containerModule)
            : base(containerModule, "bcms.media.upload", "/file/bcms-media/scripts/bcms.media.upload")
        {            
            Links = new IActionProjection[]
                {    
                    new JavaScriptModuleLinkTo<UploadController>(this, "loadUploadFilesDialogUrl", f => f.MultiFileUpload("{0}", "{1}")),
                    new JavaScriptModuleLinkTo<UploadController>(this, "uploadFileToServerUrl", f => f.UploadMedia(null)),
                    new JavaScriptModuleLinkTo<UploadController>(this, "undoFileUploadUrl", f => f.RemoveFileUpload("{0}", "{1}", "{2}"))
                    
                };

            Globalization = new IActionProjection[]
                {
                };
        }
    }
}