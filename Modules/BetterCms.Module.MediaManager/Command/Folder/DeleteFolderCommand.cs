using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.MediaManager.Models;
using BetterCms.Module.MediaManager.Services;
using BetterCms.Module.Root.Mvc;

namespace BetterCms.Module.MediaManager.Command.Folder
{
    /// <summary>
    /// A command to delete given folder.
    /// </summary>
    public class DeleteFolderCommand : CommandBase, ICommand<DeleteFolderCommandRequest, bool>
    {
        /// <summary>
        /// Gets or sets the media service.
        /// </summary>
        /// <value>
        /// The media service.
        /// </value>
        public IMediaService MediaService { get; set; }

        /// <summary>
        /// Executes this command.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>Executed command result.</returns>
        public bool Execute(DeleteFolderCommandRequest request)
        {
            UnitOfWork.BeginTransaction();

            var mediaFolder = Repository.Delete<MediaFolder>(request.FolderId, request.Version);
            var deletedSubMedias = MediaService.DeleteSubMedias(mediaFolder);

            UnitOfWork.Commit();

            deletedSubMedias.Add(mediaFolder);
            MediaService.NotifiyMediaDeleted(deletedSubMedias);

            return true;
        }
    }
}