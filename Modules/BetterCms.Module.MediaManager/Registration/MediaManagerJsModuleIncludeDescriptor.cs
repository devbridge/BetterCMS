using BetterCms.Core.Modules;
using BetterCms.Core.Modules.Projections;
using BetterCms.Module.MediaManager.Content.Resources;
using BetterCms.Module.MediaManager.Controllers;

namespace BetterCms.Module.MediaManager.Registration
{
    /// <summary>
    /// bcms.pages.js module descriptor.
    /// </summary>
    public class MediaManagerJsModuleIncludeDescriptor : JsIncludeDescriptor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MediaManagerJsModuleIncludeDescriptor" /> class.
        /// </summary>
        /// <param name="module">The container module.</param>
        public MediaManagerJsModuleIncludeDescriptor(CmsModuleDescriptor module)
            : base(module, "bcms.media")
        {
            Links = new IActionProjection[]
                {
                    new JavaScriptModuleLinkTo<MediaManagerController>(this, "loadSiteSettingsMediaManagerUrl", c => c.Index()),
                    new JavaScriptModuleLinkTo<ImagesController>(this, "loadImagesUrl", c => c.GetImagesList(null)),
                    new JavaScriptModuleLinkTo<FilesController>(this, "loadFilesUrl", c => c.GetFilesList(null)),
                    new JavaScriptModuleLinkTo<AudiosController>(this, "loadAudiosUrl", c => c.GetAudiosList(null)),
                    new JavaScriptModuleLinkTo<VideosController>(this, "loadVideosUrl", c => c.GetVideosList(null)),
                    new JavaScriptModuleLinkTo<ImagesController>(this, "insertImageDialogUrl", c => c.ImageInsert("{0}")),
                    new JavaScriptModuleLinkTo<FilesController>(this, "insertFileDialogUrl", c => c.FileInsert()),
                    new JavaScriptModuleLinkTo<ImagesController>(this, "deleteImageUrl", c => c.ImageDelete("{0}", "{1}")),
                    new JavaScriptModuleLinkTo<AudiosController>(this, "deleteAudioUrl", c => c.AudioDelete("{0}", "{1}")),
                    new JavaScriptModuleLinkTo<VideosController>(this, "deleteVideoUrl", c => c.VideoDelete("{0}", "{1}")),
                    new JavaScriptModuleLinkTo<FilesController>(this, "deleteFileUrl", c => c.FileDelete("{0}", "{1}")),
                    new JavaScriptModuleLinkTo<ImagesController>(this, "getImageUrl", c => c.GetImage("{0}")),
                    new JavaScriptModuleLinkTo<FilesController>(this, "downloadFileUrl", c => c.Download("{0}")),
                    new JavaScriptModuleLinkTo<FilesController>(this, "getFileUrl", c => c.Download("{0}")),
                    new JavaScriptModuleLinkTo<FolderController>(this, "saveFolderUrl", c => c.SaveFolder(null)),
                    new JavaScriptModuleLinkTo<FolderController>(this, "deleteFolderUrl", c => c.DeleteFolder("{0}", "{1}")),
                    new JavaScriptModuleLinkTo<MediaManagerController>(this, "renameMediaUrl", c => c.RenameMedia(null)),
                    new JavaScriptModuleLinkTo<MediaManagerController>(this, "archiveMediaUrl", c => c.ArchiveMedia("{0}", "{1}")),
                    new JavaScriptModuleLinkTo<MediaManagerController>(this, "unarchiveMediaUrl", c => c.UnarchiveMedia("{0}", "{1}")),
                };

            Globalization = new IActionProjection[]
                {
                    new JavaScriptModuleGlobalization(this, "insertImageDialogTitle", () => MediaGlobalization.InsertImage_Dialog_Title),
                    new JavaScriptModuleGlobalization(this, "insertImageFailureMessageTitle", () => MediaGlobalization.InsertImage_FailureMessage_Title),
                    new JavaScriptModuleGlobalization(this, "insertImageFailureMessageMessage", () => MediaGlobalization.InsertImage_FailureMessage_Message),
                    new JavaScriptModuleGlobalization(this, "imageNotSelectedMessageMessage", () => MediaGlobalization.ImageNotSelected_MessageMessage),
                    new JavaScriptModuleGlobalization(this, "insertImageInsertButtonTitle", () => MediaGlobalization.InsertImage_InsertButton_Title),
                    
                    new JavaScriptModuleGlobalization(this, "selectFolderDialogTitle", () => MediaGlobalization.SelectFolder_Dialog_Title),
                    new JavaScriptModuleGlobalization(this, "selectFolderFailureMessageTitle", () => MediaGlobalization.SelectFolder_FailureMessage_Title),
                    new JavaScriptModuleGlobalization(this, "selectFolderFailureMessageMessage", () => MediaGlobalization.SelectFolder_FailureMessage_Message),
                    new JavaScriptModuleGlobalization(this, "selectFolderSelectButtonTitle", () => MediaGlobalization.SelectFolder_SelectButton_Title),
                    new JavaScriptModuleGlobalization(this, "rootFolderTitle", () => MediaGlobalization.RootFolder_Title),

                    new JavaScriptModuleGlobalization(this, "insertFileDialogTitle", () => MediaGlobalization.InsertFile_Dialog_Title),
                    new JavaScriptModuleGlobalization(this, "insertFileFailureMessageTitle", () => MediaGlobalization.InsertFile_FailureMessage_Title),
                    new JavaScriptModuleGlobalization(this, "insertFileFailureMessageMessage", () => MediaGlobalization.InsertFile_FailureMessage_Message),
                    new JavaScriptModuleGlobalization(this, "fileNotSelectedMessageMessage", () => MediaGlobalization.FileNotSelected_MessageMessage),

                    new JavaScriptModuleGlobalization(this, "searchedInPathPrefix", () => MediaGlobalization.MediaManager_SearchedInPath_Prefix),
                    new JavaScriptModuleGlobalization(this, "noResultFoundMessage", () => MediaGlobalization.MediaManager_Search_NoMatchesFound),

                    new JavaScriptModuleGlobalization(this, "deleteImageConfirmMessage", () => MediaGlobalization.DeleteImage_Confirmation_Message),
                    new JavaScriptModuleGlobalization(this, "deleteAudioConfirmMessage", () => MediaGlobalization.DeleteAudio_Confirmation_Message),
                    new JavaScriptModuleGlobalization(this, "deleteVideoConfirmMessage", () => MediaGlobalization.DeleteVideo_Confirmation_Message),
                    new JavaScriptModuleGlobalization(this, "deleteFileConfirmMessage", () => MediaGlobalization.DeleteFile_Confirmation_Message),
                    new JavaScriptModuleGlobalization(this, "deleteFolderConfirmMessage", () => MediaGlobalization.DeleteFolder_ConfirmationMessage),
                    
                    new JavaScriptModuleGlobalization(this, "archiveMediaConfirmMessage", () => MediaGlobalization.ArchiveMedia_ConfirmationMessage),
                    new JavaScriptModuleGlobalization(this, "unarchiveMediaConfirmMessage", () => MediaGlobalization.UnarchiveMedia_ConfirmationMessage),
                    new JavaScriptModuleGlobalization(this, "archiveImageConfirmMessage", () => MediaGlobalization.ArchiveImage_ConfirmationMessage),
                    new JavaScriptModuleGlobalization(this, "unarchiveImageConfirmMessage", () => MediaGlobalization.UnarchiveImage_ConfirmationMessage),
                    new JavaScriptModuleGlobalization(this, "archiveVideoConfirmMessage", () => MediaGlobalization.ArchiveVideo_ConfirmationMessage),
                    new JavaScriptModuleGlobalization(this, "unarchiveVideoConfirmMessage", () => MediaGlobalization.UnarchiveVideo_ConfirmationMessage),
                    new JavaScriptModuleGlobalization(this, "archiveFileConfirmMessage", () => MediaGlobalization.ArchiveFile_ConfirmationMessage),
                    new JavaScriptModuleGlobalization(this, "unarchiveFileConfirmMessage", () => MediaGlobalization.UnarchiveFile_ConfirmationMessage),

                    new JavaScriptModuleGlobalization(this, "imagesTabTitle", () => MediaGlobalization.ImagesTab_Title),
                    new JavaScriptModuleGlobalization(this, "audiosTabTitle", () => MediaGlobalization.AudiosTab_Title),
                    new JavaScriptModuleGlobalization(this, "videosTabTitle", () => MediaGlobalization.VideosTab_Title),
                    new JavaScriptModuleGlobalization(this, "filesTabTitle", () => MediaGlobalization.FilesTab_Title),

                    new JavaScriptModuleGlobalization(this, "uploadImage", () => MediaGlobalization.ImagesTab_UploadImage),
                    new JavaScriptModuleGlobalization(this, "uploadAudio", () => MediaGlobalization.AudiosTab_UploadAudio),
                    new JavaScriptModuleGlobalization(this, "uploadVideo", () => MediaGlobalization.VideosTab_UploadVideo),
                    new JavaScriptModuleGlobalization(this, "uploadFile", () => MediaGlobalization.FilesTab_UploadFile)
                };
        }
    }
}