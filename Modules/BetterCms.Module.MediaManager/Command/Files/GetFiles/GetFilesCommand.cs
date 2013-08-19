using BetterCms.Module.MediaManager.Command.MediaManager;
using BetterCms.Module.MediaManager.Controllers;
using BetterCms.Module.MediaManager.Models;
using BetterCms.Module.MediaManager.ViewModels.MediaManager;
using BetterCms.Module.Root.Mvc.Helpers;

namespace BetterCms.Module.MediaManager.Command.Files.GetFiles
{
    public class GetFilesCommand : GetMediaItemsCommandBase<MediaFile>
    {
        /// <summary>
        /// The files download URL
        /// </summary>
        private static string filesDownloadUrl;

        /// <summary>
        /// Gets the type of the current media items.
        /// </summary>
        /// <value>
        /// The type of the current media items.
        /// </value>
        protected override MediaType MediaType
        {
            get { return MediaType.File; }
        }

        /// <summary>
        /// Gets the files download URL.
        /// </summary>
        /// <returns></returns>
        private string GetFilesDownloadUrl()
        {
            if (filesDownloadUrl == null)
            {
                filesDownloadUrl = CmsUrlHelper.GetActionUrl<FilesController>(f => f.Download("{0}"));
            }

            return filesDownloadUrl;
        }

        /// <summary>
        /// Creates the image view model.
        /// </summary>
        /// <param name="media">The media.</param>
        /// <returns>Created image view model</returns>
        protected override MediaViewModel ToViewModel(Media media)
        {
            var model = base.ToViewModel(media);

            if (CmsConfiguration.AccessControlEnabled)
            {
                var fileModel = model as MediaFileViewModel;
                if (fileModel != null)
                {
                    fileModel.PublicUrl = string.Format(GetFilesDownloadUrl(), fileModel.Id);
                }
            }

            return model;
        }
    }
}