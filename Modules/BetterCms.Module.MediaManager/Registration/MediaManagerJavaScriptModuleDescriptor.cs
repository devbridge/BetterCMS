using BetterCms.Core.Modules;
using BetterCms.Core.Modules.JsModule;
using BetterCms.Core.Modules.Projections;
using BetterCms.Module.MediaManager.Content.Resources;
using BetterCms.Module.MediaManager.Controllers;

namespace BetterCms.Module.MediaManager.Registration
{
    /// <summary>
    /// bcms.pages.js module descriptor.
    /// </summary>
    public class MediaManagerJavaScriptModuleDescriptor : JavaScriptModuleDescriptor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MediaManagerJavaScriptModuleDescriptor" /> class.
        /// </summary>
        /// <param name="containerModule">The container module.</param>
        public MediaManagerJavaScriptModuleDescriptor(ModuleDescriptor containerModule)
            : base(containerModule, "bcms.media", "/file/bcms-media/scripts/bcms.media")
        {
            Links = new IActionProjection[]
                {
                    new JavaScriptModuleLinkTo<MediaManagerController>(this, "loadSiteSettingsMediaManagerUrl", c => c.Index(null)),
                    new JavaScriptModuleLinkTo<ImagesController>(this, "loadImagesUrl", c => c.GetImagesList(null)),
                    new JavaScriptModuleLinkTo<ImagesController>(this, "insertImageDialogUrl", c => c.ImageInsert(null)),
                    new JavaScriptModuleLinkTo<ImagesController>(this, "deleteImageUrl", c => c.ImageDelete("{0}", "{1}")),
                    new JavaScriptModuleLinkTo<ImagesController>(this, "getImageUrl", c => c.GetImage("{0}")),
                    new JavaScriptModuleLinkTo<FolderController>(this, "saveFolderUrl", c => c.SaveFolder(null)),
                    new JavaScriptModuleLinkTo<FolderController>(this, "deleteFolderUrl", c => c.DeleteFolder(null))
                };

            Globalization = new IActionProjection[]
                {
                    new JavaScriptModuleGlobalization(this, "insertImageDialogTitle", () => MediaGlobalization.InsertImage_Dialog_Title),
                    new JavaScriptModuleGlobalization(this, "insertImageFailureMessageTitle", () => MediaGlobalization.InsertImage_FailureMessage_Title),
                    new JavaScriptModuleGlobalization(this, "insertImageFailureMessageMessage", () => MediaGlobalization.InsertImage_FailureMessage_Message),
                    new JavaScriptModuleGlobalization(this, "deleteImageConfirmMessage", () => MediaGlobalization.DeleteImage_Confirmation_Message),
                    new JavaScriptModuleGlobalization(this, "imageNotSelectedMessageMessage", () => MediaGlobalization.ImageNotSelected_MessageMessage),
                    new JavaScriptModuleGlobalization(this, "confirmDeleteFolderMessage", () => MediaGlobalization.DeleteFolder_ConfirmationMessage)
                };
        }
    }
}