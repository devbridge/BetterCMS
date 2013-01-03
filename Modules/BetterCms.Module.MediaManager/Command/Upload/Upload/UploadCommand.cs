using BetterCms.Core.Exceptions;
using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.MediaManager.Models;
using BetterCms.Module.MediaManager.Services;
using BetterCms.Module.Root.Mvc;

namespace BetterCms.Module.MediaManager.Command.Upload.Upload
{
    public class UploadCommand : CommandBase, ICommand<UploadFileRequest, MediaFile>
    {
        public IMediaFileService MediaFileService { get; set; }

        public IMediaImageService MediaImageService { get; set; }

        public IMediaVideoService MediaVideoService { get; set; }

        public IMediaAudioService MediaAudioService { get; set; }

        public MediaFile Execute(UploadFileRequest request)
        {
            if (request.Type == MediaType.File || request.Type == MediaType.Audio || request.Type == MediaType.Video)
            {
                return MediaFileService.UploadFile(request.Type, request.RootFolderId, request.FileName, request.FileLength, request.FileStream);
            }

            if (request.Type == MediaType.Image)
            {
                return MediaImageService.UploadImage(request.RootFolderId, request.FileName, request.FileLength, request.FileStream);
            }

            throw new CmsException(string.Format("A given media type {0} is not supported to upload.", request.Type));
        }
    }

}