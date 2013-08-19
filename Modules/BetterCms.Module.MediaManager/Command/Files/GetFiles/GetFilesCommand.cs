using BetterCms.Module.MediaManager.Command.MediaManager;
using BetterCms.Module.MediaManager.Models;
using BetterCms.Module.MediaManager.Services;
using BetterCms.Module.MediaManager.ViewModels.MediaManager;

namespace BetterCms.Module.MediaManager.Command.Files.GetFiles
{
    public class GetFilesCommand : GetMediaItemsCommandBase<MediaFile>
    {
        /// <summary>
        /// The file service
        /// </summary>
        public IMediaFileService FileService { get; set; }

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
        /// Creates the image view model.
        /// </summary>
        /// <param name="media">The media.</param>
        /// <returns>Created image view model</returns>
        protected override MediaViewModel ToViewModel(Media media)
        {
            var model = base.ToViewModel(media);

            var fileModel = model as MediaFileViewModel;
            if (fileModel != null)
            {
                fileModel.PublicUrl = FileService.GetDownloadFileUrl(MediaType.File, fileModel.Id, fileModel.PublicUrl);
            }

            return model;
        }
    }
}