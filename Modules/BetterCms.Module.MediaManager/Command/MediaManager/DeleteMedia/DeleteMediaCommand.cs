using BetterCms.Api;
using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.MediaManager.Models;
using BetterCms.Module.Root.Mvc;

namespace BetterCms.Module.MediaManager.Command.MediaManager.DeleteMedia
{
    /// <summary>
    /// Command for delete a media
    /// </summary>
    public class DeleteMediaCommand : CommandBase, ICommand<DeleteMediaCommandRequest, bool>
    {
        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public bool Execute(DeleteMediaCommandRequest request)
        {
            var media = Repository.Delete<Media>(request.Id, request.Version);
            UnitOfWork.Commit();

            if (media is MediaFolder)
            {
                MediaManagerApiContext.Events.OnMediaFolderDeleted((MediaFolder)media);
            }
            else if (media is MediaFile)
            {
                MediaManagerApiContext.Events.OnMediaFileDeleted((MediaFile)media);
            }

            return true;
        }
    }
}