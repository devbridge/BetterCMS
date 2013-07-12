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
            var media = Repository.Delete<Media>(request.Id, request.Version, false);
            UnitOfWork.Commit();

            // Notify.
            if (media is MediaFolder)
            {
                Events.MediaManagerEvents.Instance.OnMediaFolderDeleted((MediaFolder)media);
            }
            else if (media is MediaFile)
            {
                Events.MediaManagerEvents.Instance.OnMediaFileDeleted((MediaFile)media);
            }        

            return true;
        }
    }
}