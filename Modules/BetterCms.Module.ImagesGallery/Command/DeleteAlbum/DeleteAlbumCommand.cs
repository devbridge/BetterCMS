using BetterCms.Core.Mvc.Commands;

using BetterCms.Module.ImagesGallery.Models;
using BetterCms.Module.ImagesGallery.ViewModels;
using BetterCms.Module.Root.Mvc;

namespace BetterCms.Module.ImagesGallery.Command.DeleteAlbum
{
    public class DeleteAlbumCommand : CommandBase, ICommand<AlbumViewModel, bool>
    {
        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns><c>True</c>, if newsletter subscriber is deleted successfully.</returns>
        public bool Execute(AlbumViewModel request)
        {
            var subscriber = Repository.Delete<Album>(request.Id, request.Version);
            UnitOfWork.Commit();

            Events.ImageGalleryEvents.Instance.OnAlbumDeleted(subscriber);

            return true;
        }
    }
}