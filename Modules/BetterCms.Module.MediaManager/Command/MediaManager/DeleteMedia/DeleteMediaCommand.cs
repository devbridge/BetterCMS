using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.MediaManager.Models;
using BetterCms.Module.MediaManager.Services;
using BetterCms.Module.Root.Mvc;

namespace BetterCms.Module.MediaManager.Command.MediaManager.DeleteMedia
{
    /// <summary>
    /// Command for delete a media
    /// </summary>
    public class DeleteMediaCommand : CommandBase, ICommand<DeleteMediaCommandRequest, bool>
    {
        /// <summary>
        /// Gets or sets the media service.
        /// </summary>
        /// <value>
        /// The media service.
        /// </value>
        public IMediaService MediaService { get; set; }

        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public bool Execute(DeleteMediaCommandRequest request)
        {
            UnitOfWork.BeginTransaction();

            var media = Repository.Delete<Media>(request.Id, request.Version, false);
            var deletedSubMedias = MediaService.DeleteSubMedias(media);
            
            UnitOfWork.Commit();

            deletedSubMedias.Add(media);
            MediaService.NotifiyMediaDeleted(deletedSubMedias);

            return true;
        }
    }
}