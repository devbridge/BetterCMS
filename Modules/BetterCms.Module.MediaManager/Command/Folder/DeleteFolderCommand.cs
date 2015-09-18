using BetterCms.Module.MediaManager.Services;
using BetterCms.Module.Root.Mvc;

using BetterModules.Core.Web.Mvc.Commands;

namespace BetterCms.Module.MediaManager.Command.Folder
{
    /// <summary>
    /// A command to delete given folder.
    /// </summary>
    public class DeleteFolderCommand : CommandBase, ICommand<DeleteFolderCommandRequest, bool>
    {
        private readonly IMediaFileService mediaFileService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteFolderCommand" /> class.
        /// </summary>
        /// <param name="mediaFileService">The media file service.</param>
        public DeleteFolderCommand(IMediaFileService mediaFileService)
        {
            this.mediaFileService = mediaFileService;
        }

        /// <summary>
        /// Executes this command.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>Executed command result.</returns>
        public bool Execute(DeleteFolderCommandRequest request)
        {
            mediaFileService.DeleteFolderByMovingToTrash(request.FolderId);

            return true;
        }
                }
}