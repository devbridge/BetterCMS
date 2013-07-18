using BetterCms.Core.Exceptions.DataTier;
using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.MediaManager.Command.MediaManager.DeleteMedia;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Viddler.Models;
using BetterCms.Module.Viddler.Services;

namespace BetterCms.Module.Viddler.Command.DeleteVideo
{
    /// <summary>
    /// Command to delete video.
    /// </summary>
    internal class DeleteVideoCommand : CommandBase, ICommand<DeleteMediaCommandRequest, bool>
    {
        private readonly IViddlerService viddlerService;

        public DeleteVideoCommand(IViddlerService viddlerService)
        {
            this.viddlerService = viddlerService;
        }

        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns><c>true</c> on success.</returns>
        public bool Execute(DeleteMediaCommandRequest request)
        {
            var video = Repository.First<Video>(request.Id);
            if (video.Version != request.Version)
            {
                throw new ConcurrentDataException("Video with id {0} was updated by another transaction.");
            }

            var sesionId = viddlerService.GetSessionId();
            if (!viddlerService.RemoveVideo(sesionId, video.VideoId))
            {
                return false;
            }

            Repository.Delete(video);
            UnitOfWork.Commit();

            // Notify.
            Events.MediaManagerEvents.Instance.OnMediaFileDeleted(video);

            return true;
        }
    }
}