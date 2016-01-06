using System.IO;

using BetterCms.Core.Exceptions;

using BetterCms.Module.MediaManager.Models;
using BetterCms.Module.MediaManager.Services;
using BetterCms.Module.Root.Mvc;

using BetterModules.Core.Web.Mvc.Commands;

namespace BetterCms.Module.MediaManager.Command.Upload.Upload
{
    public class UploadCommand : CommandBase, ICommand<UploadFileRequest, MediaFile>
    {
        public IMediaFileService MediaFileService { get; set; }

        public IMediaImageService MediaImageService { get; set; }

        public IMediaVideoService MediaVideoService { get; set; }

        public IMediaAudioService MediaAudioService { get; set; }
        
        public ICmsConfiguration CmsConfiguration { get; set; }

        public MediaFile Execute(UploadFileRequest request)
        {
            var maxLength = CmsConfiguration.Storage.MaximumFileNameLength > 0 ? CmsConfiguration.Storage.MaximumFileNameLength : 100;

            // Fix for IIS express + IE (if full path is returned)
            var fileName = Path.GetFileName(request.FileName);
            if (fileName.Length > maxLength)
            {
                fileName = string.Concat(Path.GetFileNameWithoutExtension(fileName.Substring(0, maxLength)), Path.GetExtension(fileName));
            }

            if (request.Type == MediaType.File || request.Type == MediaType.Audio || request.Type == MediaType.Video)
            {
                var media = MediaFileService.UploadFile(request.Type, request.RootFolderId, fileName, request.FileLength, request.FileStream);

                Events.MediaManagerEvents.Instance.OnMediaFileUploaded(media);

                return media;
            }

            if (request.Type == MediaType.Image)
            {
                var media = MediaImageService.UploadImage(
                    request.RootFolderId,
                    fileName,
                    request.FileLength,
                    request.FileStream,
                    request.ReuploadMediaId,
                    request.ShouldOverride);
                
                Events.MediaManagerEvents.Instance.OnMediaFileUploaded(media);

                return media;
            }

            throw new CmsException(string.Format("A given media type {0} is not supported to upload.", request.Type));
        }
    }

}