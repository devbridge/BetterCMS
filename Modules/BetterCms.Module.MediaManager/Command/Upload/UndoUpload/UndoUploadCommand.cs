using BetterCms.Core.Exceptions;

using BetterCms.Module.MediaManager.Models;
using BetterCms.Module.MediaManager.Services;
using BetterCms.Module.Root.Mvc;

using BetterModules.Core.Web.Mvc.Commands;

namespace BetterCms.Module.MediaManager.Command.Upload.UndoUpload
{
    /// <summary>
    /// Undo file upload command.
    /// </summary>
    public class UndoUploadCommand : CommandBase, ICommand<UndoUploadRequest, bool>
    {
        /// <summary>
        /// Gets or sets the media file service.
        /// </summary>
        /// <value>
        /// The media file service.
        /// </value>
        public IMediaFileService MediaFileService { get; set; }

        /// <summary>
        /// Gets or sets the media image service.
        /// </summary>
        /// <value>
        /// The media image service.
        /// </value>
        public IMediaImageService MediaImageService { get; set; }

        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns><c>true</c> on success, otherwise <c>false</c></returns>
        /// <exception cref="CmsException">if media format is not supported.</exception>
        public bool Execute(UndoUploadRequest request)
        {
            if (request.Type == MediaType.File || request.Type == MediaType.Audio || request.Type == MediaType.Video)
            {
                MediaFileService.RemoveFile(request.FileId, request.Version, doNotCheckVersion: true);
            }
            else if (request.Type == MediaType.Image)
            {
                MediaImageService.RemoveImageWithFiles(request.FileId, request.Version, doNotCheckVersion: true);
            }
            else
            {
                throw new CmsException(string.Format("A given media type {0} is not supported to upload.", request.Type));
            }
            
            return true;
        }
    }
}