using System;
using System.Linq;

using Autofac;

using BetterCms.Core.Api.DataContracts;
using BetterCms.Core.Api.Extensions;
using BetterCms.Core.DataAccess;
using BetterCms.Core.DataAccess.DataContext.Fetching;
using BetterCms.Core.Exceptions.Api;
using BetterCms.Module.MediaManager.Api.DataContracts;
using BetterCms.Module.MediaManager.Api.Events;
using BetterCms.Module.MediaManager.Models;

// ReSharper disable CheckNamespace
namespace BetterCms.Api
// ReSharper restore CheckNamespace
{
    /// <summary>
    /// Media Manager API Context.
    /// </summary>
    public class MediaManagerApiContext : DataApiContext
    {
        private static readonly MediaManagerEvents events;

        /// <summary>
        /// Initializes the <see cref="MediaManagerApiContext" /> class.
        /// </summary>
        static MediaManagerApiContext()
        {
            events = new MediaManagerEvents();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MediaManagerApiContext" /> class.
        /// </summary>
        /// <param name="lifetimeScope">The lifetime scope.</param>
        /// <param name="repository">The repository.</param>
        public MediaManagerApiContext(ILifetimeScope lifetimeScope, IRepository repository = null)
            : base(lifetimeScope, repository)
        {
        }

        public new static MediaManagerEvents Events
        {
            get
            {
                return events;
            }
        }

        /// <summary>
        /// Gets the list of folder media entities.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>
        /// The list of folder media entities
        /// </returns>
        /// <exception cref="CmsApiException"></exception>
        public DataListResponse<Media> GetFolderMedias(GetFolderMediasRequest request)
        {
            try
            {
                var query = Repository
                    .AsQueryable<Media>()
                    .Where(f => f.Type == request.MediaType)
                    .ApplyFilters(request);

                if (request.FolderId.HasValue)
                {
                    query = query.Where(f => f.Folder != null && f.Folder.Id == request.FolderId.Value);
                }
                else
                {
                    query = query.Where(f => f.Folder == null);
                }

                var totalCount = query.ToRowCountFutureValue(request);
                query = query.AddOrderAndPaging(request);

                return query.ToDataListResponse(totalCount);
            }
            catch (Exception inner)
            {
                var message = string.Format("Failed to get folder medias list for media type={0} and folderId={1}.",
                    request.MediaType,
                    request.FolderId.HasValue ? request.FolderId.Value.ToString() : "null");
                Logger.Error(message, inner);
                throw new CmsApiException(message, inner);
            }
        }

        /// <summary>
        /// Gets the list of media image entities.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>
        /// The list of tag entities
        /// </returns>
        /// <exception cref="CmsApiException"></exception>
        public DataListResponse<MediaImage> GetImages(GetImagesRequest request = null)
        {
            try
            {
                var query = Repository
                    .AsQueryable<MediaImage>()
                    .ApplyFilters(request);

                var totalCount = query.ToRowCountFutureValue(request);
                query = query
                    .AddOrderAndPaging(request)
                    .Fetch(m => m.Folder);

                return query.ToDataListResponse(totalCount);
            }
            catch (Exception inner)
            {
                const string message = "Failed to get images list.";
                Logger.Error(message, inner);
                throw new CmsApiException(message, inner);
            }
        }

        /// <summary>
        /// Gets the list of media file entities.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>
        /// The list of media file entities
        /// </returns>
        /// <exception cref="CmsApiException"></exception>
        public DataListResponse<MediaFile> GetFiles(GetFilesRequest request = null)
        {
            try
            {
                var query = Repository
                    .AsQueryable<MediaFile>()
                    .Where(m => m.Type == MediaType.File)
                    .ApplyFilters(request);

                var totalCount = query.ToRowCountFutureValue(request);
                query = query
                    .AddOrderAndPaging(request)
                    .Fetch(m => m.Folder);

                return query.ToDataListResponse(totalCount);
            }
            catch (Exception inner)
            {
                const string message = "Failed to get files list.";
                Logger.Error(message, inner);
                throw new CmsApiException(message, inner);
            }
        }

        /// <summary>
        /// Gets the list of media folder entities.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>
        /// The list of folder entities
        /// </returns>
        /// <exception cref="CmsApiException"></exception>
        public DataListResponse<MediaFolder> GetFolders(GetFoldersRequest request)
        {
            try
            {
                var query = Repository
                    .AsQueryable<MediaFolder>()
                    .Where(f => f.Type == request.MediaType)
                    .ApplyFilters(request);

                var totalCount = query.ToRowCountFutureValue(request);
                query = query
                    .AddOrderAndPaging(request)
                    .Fetch(m => m.Folder);

                return query.ToDataListResponse(totalCount);

            }
            catch (Exception inner)
            {
                var message = string.Format("Failed to get folders list for media type={0}.", request.MediaType);
                Logger.Error(message, inner);
                throw new CmsApiException(message, inner);
            }
        }

        /// <summary>
        /// Gets the file.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>
        /// File entity
        /// </returns>
        public MediaFile GetFile(Guid id)
        {
            try
            {
                return Repository
                    .AsQueryable<MediaFile>()
                    .Where(f => f.Id == id)
                    .Fetch(f => f.Folder)
                    .FirstOrDefault();
            }
            catch (Exception inner)
            {
                var message = string.Format("Failed to get media file by id {0}.", id);
                Logger.Error(message, inner);
                throw new CmsApiException(message, inner);
            }
        }

        /// <summary>
        /// Gets the image.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>
        /// Image entity
        /// </returns>
        public MediaImage GetImage(Guid id)
        {
            try
            {
                return Repository
                    .AsQueryable<MediaImage>()
                    .Where(f => f.Id == id)
                    .Fetch(f => f.Folder)
                    .FirstOrDefault();
            }
            catch (Exception inner)
            {
                var message = string.Format("Failed to get media image by id {0}.", id);
                Logger.Error(message, inner);
                throw new CmsApiException(message, inner);
            }
        }
    }
}