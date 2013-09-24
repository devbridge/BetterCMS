using BetterCms.Core.DataAccess;
using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Core.Mvc.Commands;

using BetterCms.Module.ImagesGallery.Models;
using BetterCms.Module.ImagesGallery.ViewModels;
using BetterCms.Module.Root.Mvc;

namespace BetterCms.Module.ImagesGallery.Command.SaveAlbum
{
    public class SaveAlbumCommand : CommandBase, ICommand<AlbumViewModel, AlbumViewModel>
    {
        /// <summary>
        /// The repository
        /// </summary>
        private IRepository repository;

        /// <summary>
        /// The unit of work
        /// </summary>
        private IUnitOfWork unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="SaveAlbumCommand" /> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        public SaveAlbumCommand(IRepository repository, IUnitOfWork unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public AlbumViewModel Execute(AlbumViewModel request)
        {
            unitOfWork.BeginTransaction();

            var isNew = request.Id.HasDefaultValue();
            Album album;

            if (isNew)
            {
                album = new Album();
            }
            else
            {
                album = repository.AsQueryable<Album>(w => w.Id == request.Id).FirstOne();
            }

            album.Title = request.Title;
            album.Version = request.Version;

            repository.Save(album);
            unitOfWork.Commit();

            if (isNew)
            {
                Events.ImageGalleryEvents.Instance.OnAlbumCreated(album);
            }
            else
            {
                Events.ImageGalleryEvents.Instance.OnAlbumCreated(album);
            }

            return new AlbumViewModel
                {
                    Id = album.Id,
                    Version = album.Version,
                    Title = album.Title
                };
        }
    }
}