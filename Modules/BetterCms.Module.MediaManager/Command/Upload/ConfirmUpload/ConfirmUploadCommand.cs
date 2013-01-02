using BetterCms.Core.Exceptions;
using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.MediaManager.Models;
using BetterCms.Module.MediaManager.ViewModels.Upload;
using BetterCms.Module.Root.Mvc;

namespace BetterCms.Module.MediaManager.Command.Upload.ConfirmUpload
{
    public class ConfirmUploadCommand : CommandBase, ICommand<MultiFileUploadViewModel, bool>
    {
        /// <summary>
        /// Executes this command.
        /// </summary>
        public bool Execute(MultiFileUploadViewModel request)
        {            
            if (request.UploadedFiles != null && request.UploadedFiles.Count > 0)
            {
                foreach (var fileId in request.UploadedFiles)
                {
                    var file = Repository.FirstOrDefault<MediaFile>(fileId);
                    file.IsTemporary = false;
                    Repository.Save(file);
                }
                UnitOfWork.Commit();
            }

            return true;
        }
    }
}
