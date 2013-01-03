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
            Repository.Delete<Media>(request.Id, request.Version);
            UnitOfWork.Commit();
            return true;
        }
    }
}