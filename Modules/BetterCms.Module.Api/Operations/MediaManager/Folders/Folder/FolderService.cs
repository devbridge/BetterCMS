using System;
using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.Exceptions.Api;

using BetterCms.Module.MediaManager.Models;
using BetterCms.Module.MediaManager.Services;
using BetterCms.Module.Root.Mvc;

using BetterModules.Core.DataAccess;
using BetterModules.Core.DataAccess.DataContext;
using BetterModules.Core.Exceptions.DataTier;

using NHibernate.Linq;

using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Api.Operations.MediaManager.Folders.Folder
{
    /// <summary>
    /// Default folder CRUD service.
    /// </summary>
    public class FolderService : Service, IFolderService
    {
        /// <summary>
        /// The repository.
        /// </summary>
        private readonly IRepository repository;

        /// <summary>
        /// The unit of work.
        /// </summary>
        private readonly IUnitOfWork unitOfWork;

        /// <summary>
        /// The media service.
        /// </summary>
        private readonly IMediaService mediaService;

        /// <summary>
        /// Initializes a new instance of the <see cref="FolderService" /> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="mediaService">The media service.</param>
        public FolderService(IRepository repository, IUnitOfWork unitOfWork, IMediaService mediaService)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
            this.mediaService = mediaService;
        }

        /// <summary>
        /// Gets the specified folder.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>
        ///   <c>GetFolderRequest</c> with an folder.
        /// </returns>
        public GetFolderResponse Get(GetFolderRequest request)
        {
            var model =
                repository.AsQueryable<MediaFolder>(media => media.Id == request.FolderId && media.ContentType == Module.MediaManager.Models.MediaContentType.Folder)
                    .Select(
                        media =>
                        new FolderModel
                            {
                                Id = media.Id,
                                Version = media.Version,
                                CreatedBy = media.CreatedByUser,
                                CreatedOn = media.CreatedOn,
                                LastModifiedBy = media.ModifiedByUser,
                                LastModifiedOn = media.ModifiedOn,
                                Title = media.Title,
                                IsArchived = media.IsArchived,
                                Type = (MediaType)(int)media.Type,
                                ParentFolderId = media.Folder != null ? (Guid?)media.Folder.Id : null
                            }).FirstOne();

            return new GetFolderResponse { Data = model };
        }

        /// <summary>
        /// Replaces the folder or if it doesn't exist, creates it.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>
        ///   <c>PutFolderResponse</c> with a folder id.
        /// </returns>
        public PutFolderResponse Put(PutFolderRequest request)
        {
            IEnumerable<MediaFolder> parentFolderFuture = null;
            if (request.Data.ParentFolderId.HasValue)
            {
                parentFolderFuture = repository.AsQueryable<MediaFolder>()
                    .Where(c => c.Id == request.Data.ParentFolderId.Value && !c.IsDeleted)
                    .ToFuture();
            }

            var mediaFolder = repository.AsQueryable<MediaFolder>()
                .Fetch(media => media.Folder)
                .Distinct()
                .ToFuture()
                .FirstOrDefault(folder => folder.Id == request.Id);

            MediaFolder parentFolder = null;
            if (parentFolderFuture != null)
            {
                parentFolder = parentFolderFuture.First();
                if (parentFolder.Type != (Module.MediaManager.Models.MediaType)(int)request.Data.Type)
                {
                    throw new CmsApiValidationException("Parent folder type does not match to this folder type.");
                }
            }

            var createFolder = mediaFolder == null;
            if (createFolder)
            {
                mediaFolder = new MediaFolder
                                  {
                                      Id = request.Id.GetValueOrDefault(),
                                      ContentType = Module.MediaManager.Models.MediaContentType.Folder,
                                      Type = (Module.MediaManager.Models.MediaType)(int)request.Data.Type
                                  };
            }
            else if (request.Data.Version > 0)
            {
                mediaFolder.Version = request.Data.Version;
            }

            unitOfWork.BeginTransaction();

            mediaFolder.Title = request.Data.Title;
            mediaFolder.Folder = parentFolder;

            mediaFolder.PublishedOn = DateTime.Now;

            var archivedMedias = new List<Media>();
            var unarchivedMedias = new List<Media>();
            if (mediaFolder.IsArchived != request.Data.IsArchived)
            {
                if (request.Data.IsArchived)
                {
                    archivedMedias.Add(mediaFolder);
                    mediaService.ArchiveSubMedias(mediaFolder, archivedMedias);
                }
                else
                {
                    unarchivedMedias.Add(mediaFolder);
                    mediaService.UnarchiveSubMedias(mediaFolder, unarchivedMedias);
                }
            }

            mediaFolder.IsArchived = request.Data.IsArchived;

            repository.Save(mediaFolder);

            unitOfWork.Commit();

            // Fire events.
            if (createFolder)
            {
                Events.MediaManagerEvents.Instance.OnMediaFolderCreated(mediaFolder);
            }
            else
            {
                Events.MediaManagerEvents.Instance.OnMediaFolderUpdated(mediaFolder);
            }

            foreach (var archivedMedia in archivedMedias.Distinct())
            {
                Events.MediaManagerEvents.Instance.OnMediaArchived(archivedMedia);
            }

            foreach (var archivedMedia in unarchivedMedias.Distinct())
            {
                Events.MediaManagerEvents.Instance.OnMediaUnarchived(archivedMedia);
            }

            return new PutFolderResponse { Data = mediaFolder.Id };
        }

        /// <summary>
        /// Deletes the specified folder.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>
        ///   <c>DeleteFolderResponse</c> with success status.
        /// </returns>
        public DeleteFolderResponse Delete(DeleteFolderRequest request)
        {
            if (request.Data == null || request.Id.HasDefaultValue())
            {
                return new DeleteFolderResponse { Data = false };
            }

            var itemToDelete = repository.AsQueryable<MediaFolder>().Where(p => p.Id == request.Id).FirstOne();

            if (request.Data.Version > 0 && itemToDelete.Version != request.Data.Version)
            {
                throw new ConcurrentDataException(itemToDelete);
            }

            unitOfWork.BeginTransaction();

            mediaService.DeleteMedia(itemToDelete);

            unitOfWork.Commit();

            Events.MediaManagerEvents.Instance.OnMediaFolderDeleted(itemToDelete);

            return new DeleteFolderResponse { Data = true };
        }
    }
}