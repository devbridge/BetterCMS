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
            var file = Repository.FirstOrDefault<MediaFile>(request.FileId);
            file.Version = request.Version;

            if (file is MediaImage)
            {
                MediaImageService.RemoveImageFiles((MediaImage)file);
            }
            else
            {
                MediaFileService.RemoveFile(file);
            }
            
            Repository.Delete(file);
            UnitOfWork.Commit();
            
            return true;
        }
    }
}