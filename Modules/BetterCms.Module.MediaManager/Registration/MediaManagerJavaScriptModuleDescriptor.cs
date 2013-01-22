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
                    new JavaScriptModuleLinkTo<MediaManagerController>(this, "loadSiteSettingsMediaManagerUrl", c => c.Index()),
                    new JavaScriptModuleLinkTo<ImagesController>(this, "loadImagesUrl", c => c.GetImagesList(null)),
                    new JavaScriptModuleLinkTo<FilesController>(this, "loadFilesUrl", c => c.GetFilesList(null)),
                    new JavaScriptModuleLinkTo<AudiosController>(this, "loadAudiosUrl", c => c.GetAudiosList(null)),
                    new JavaScriptModuleLinkTo<VideosController>(this, "loadVideosUrl", c => c.GetVideosList(null)),
                    new JavaScriptModuleLinkTo<ImagesController>(this, "insertImageDialogUrl", c => c.ImageInsert()),
                    new JavaScriptModuleLinkTo<FilesController>(this, "insertFileDialogUrl", c => c.FileInsert()),
                    new JavaScriptModuleLinkTo<ImagesController>(this, "deleteImageUrl", c => c.ImageDelete("{0}", "{1}")),
                    new JavaScriptModuleLinkTo<AudiosController>(this, "deleteAudioUrl", c => c.AudioDelete("{0}", "{1}")),
                    new JavaScriptModuleLinkTo<VideosController>(this, "deleteVideoUrl", c => c.VideoDelete("{0}", "{1}")),
                    new JavaScriptModuleLinkTo<FilesController>(this, "deleteFileUrl", c => c.FileDelete("{0}", "{1}")),
                    new JavaScriptModuleLinkTo<ImagesController>(this, "getImageUrl", c => c.GetImage("{0}")),
                    new JavaScriptModuleLinkTo<FolderController>(this, "saveFolderUrl", c => c.SaveFolder(null)),
                    new JavaScriptModuleLinkTo<FolderController>(this, "deleteFolderUrl", c => c.DeleteFolder("{0}", "{1}")),
                    new JavaScriptModuleLinkTo<MediaManagerController>(this, "renameMediaUrl", c => c.RenameMedia(null))
                };

            Globalization = new IActionProjection[]
                {
                    new JavaScriptModuleGlobalization(this, "insertImageDialogTitle", () => MediaGlobalization.InsertImage_Dialog_Title),
                    new JavaScriptModuleGlobalization(this, "insertImageFailureMessageTitle", () => MediaGlobalization.InsertImage_FailureMessage_Title),
                    new JavaScriptModuleGlobalization(this, "insertImageFailureMessageMessage", () => MediaGlobalization.InsertImage_FailureMessage_Message),
                    new JavaScriptModuleGlobalization(this, "imageNotSelectedMessageMessage", () => MediaGlobalization.ImageNotSelected_MessageMessage),

                    new JavaScriptModuleGlobalization(this, "insertFileDialogTitle", () => MediaGlobalization.InsertFile_Dialog_Title),
                    new JavaScriptModuleGlobalization(this, "insertFileFailureMessageTitle", () => MediaGlobalization.InsertFile_FailureMessage_Title),
                    new JavaScriptModuleGlobalization(this, "insertFileFailureMessageMessage", () => MediaGlobalization.InsertFile_FailureMessage_Message),
                    new JavaScriptModuleGlobalization(this, "fileNotSelectedMessageMessage", () => MediaGlobalization.FileNotSelected_MessageMessage),

                    new JavaScriptModuleGlobalization(this, "deleteImageConfirmMessage", () => MediaGlobalization.DeleteImage_Confirmation_Message),
                    new JavaScriptModuleGlobalization(this, "deleteAudioConfirmMessage", () => MediaGlobalization.DeleteAudio_Confirmation_Message),
                    new JavaScriptModuleGlobalization(this, "deleteVideoConfirmMessage", () => MediaGlobalization.DeleteVideo_Confirmation_Message),
                    new JavaScriptModuleGlobalization(this, "deleteFileConfirmMessage", () => MediaGlobalization.DeleteFile_Confirmation_Message),
                    new JavaScriptModuleGlobalization(this, "deleteFolderConfirmMessage", () => MediaGlobalization.DeleteFolder_ConfirmationMessage),

                    new JavaScriptModuleGlobalization(this, "imagesTabTitle", () => MediaGlobalization.ImagesTab_Title),
                    new JavaScriptModuleGlobalization(this, "audiosTabTitle", () => MediaGlobalization.AudiosTab_Title),
                    new JavaScriptModuleGlobalization(this, "videosTabTitle", () => MediaGlobalization.VideosTab_Title),
                    new JavaScriptModuleGlobalization(this, "filesTabTitle", () => MediaGlobalization.FilesTab_Title),

                    new JavaScriptModuleGlobalization(this, "uploadImage", () => MediaGlobalization.ImagesTab_UploadImage),
                    new JavaScriptModuleGlobalization(this, "uploadAudio", () => MediaGlobalization.AudiosTab_UploadAudio),
                    new JavaScriptModuleGlobalization(this, "uploadVideo", () => MediaGlobalization.VideosTab_UploadVideo),
                    new JavaScriptModuleGlobalization(this, "uploadFile", () => MediaGlobalization.FilesTab_UploadFile),
                };
        }
    }
}