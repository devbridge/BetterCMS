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
        public IMediaVideoService MediaVideoService { get; set; }
        public IMediaAudioService MediaAudioService { get; set; }

        public bool Execute(UndoUploadRequest request)
        {            
            if (request.Type == MediaType.Image)
            {
                MediaImageService.RemoveImageWithFiles(request.FileId, request.Version);
            }
            else
            {
                MediaFileService.RemoveFile(request.FileId, request.Version);
            }
            
            return true;
        }
    }
}