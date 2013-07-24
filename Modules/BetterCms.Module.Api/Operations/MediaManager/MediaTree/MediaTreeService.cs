using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.DataAccess;
using BetterCms.Module.MediaManager.Models;

using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Api.Operations.MediaManager.MediaTree
{
    public class MediaTreeService : Service, IMediaTreeService
    {
        private readonly IRepository repository;

        public MediaTreeService(IRepository repository)
        {
            this.repository = repository;
        }

        public GetMediaTreeResponse Get(GetMediaTreeRequest request)
        {
            var response = new GetMediaTreeResponse
                               {
                                   Data = new MediaTreeModel()
                               };

            if (request.Data.IncludeFilesTree)
            {
                response.Data.FilesTree = LoadMediaTree<MediaFile>(MediaType.File, request.Data.IncludeArchived, request.Data.IncludeFiles);
            }
            if (request.Data.IncludeImagesTree)
            {
                response.Data.ImagesTree = LoadMediaTree<MediaImage>(MediaType.Image, request.Data.IncludeArchived, request.Data.IncludeImages);
            }
            if (request.Data.IncludeVideosTree)
            {
                // TODO: return MediaVideo type, when it'll be implemented
                response.Data.VideosTree = LoadMediaTree<MediaFile>(MediaType.Video, request.Data.IncludeImages, request.Data.IncludeArchived);
            }

            return response;
        }

        private IList<MediaItemModel> LoadMediaTree<TEntity>(MediaType mediaType, bool includeArchived, bool loadFiles)
            where TEntity: Media
        {
            var query = repository
                    .AsQueryable<Media>()
                    .Where(f => f.Type == mediaType && f.Original == null);

            if (!includeArchived)
            {
                query = query.Where(f => !f.IsArchived);
            }

            if (loadFiles)
            {
                query = query.Where(f => (f is MediaFolder || f is TEntity));
            }
            else
            {
                query = query.Where(f => f is MediaFolder);
            }

            var mediaItems = query
                .Select(media => new MediaItemModel
                                 {
                                     Id = media.Id,
                                     Version = media.Version,
                                     CreatedBy = media.CreatedByUser,
                                     CreatedOn = media.CreatedOn,
                                     LastModifiedBy = media.ModifiedByUser,
                                     LastModifiedOn = media.ModifiedOn,

                                     ParentFolderId = media.Folder.Id,
                                     Title = media.Title,
                                     MediaContentType = media is MediaFolder 
                                                            ? (MediaContentType)((int)MediaContentType.Folder) 
                                                            : (MediaContentType)((int)MediaContentType.File),
                                     Url = media is MediaFile ? ((MediaFile)media).PublicUrl : null,
                                     IsArchived = media.IsArchived
                                 }).ToList();

            return GetChildren(mediaItems, null);
        }

        private List<MediaItemModel> GetChildren(List<MediaItemModel> allItems, System.Guid? parentId)
        {
            var childItems = allItems.Where(item => item.ParentFolderId == parentId && item.Id != parentId).ToList();

            foreach (var item in childItems)
            {
                item.Children = GetChildren(allItems, item.Id);
            }

            return childItems;
        }
    }
}