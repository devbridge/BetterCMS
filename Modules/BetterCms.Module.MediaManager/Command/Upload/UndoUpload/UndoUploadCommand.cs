using BetterCms.Core.DataContracts.Enums;
using BetterCms.Core.Exceptions;
using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.MediaManager.Models;
using BetterCms.Module.MediaManager.Services;
using BetterCms.Module.Root.Mvc;

namespace BetterCms.Module.MediaManager.Command.Upload.UndoUpload
{
    public class UndoUploadCommand : CommandBase, ICommand<UndoUploadRequest, bool>
    {
        public IMediaFileService MediaFileService { get; set; }
        public IMediaImageService MediaImageService { get; set; }

        public bool Execute(UndoUploadRequest request)
        {
            if (request.Type == MediaType.File || request.Type == MediaType.Audio || request.Type == MediaType.Video)
            {
                MediaFileService.RemoveFile(request.FileId, request.Version);
            }
            else if (request.Type == MediaType.Image)
            {
                MediaImageService.RemoveImageWithFiles(request.FileId, request.Version);
            }
            else
            {
                throw new CmsException(string.Format("A given media type {0} is not supported to upload.", request.Type));
            }
            
            return true;
        }
    }
}