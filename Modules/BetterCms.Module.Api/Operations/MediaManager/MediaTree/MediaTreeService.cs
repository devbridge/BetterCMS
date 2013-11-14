using System;
using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.DataAccess;
using BetterCms.Module.MediaManager.Models;
using BetterCms.Module.MediaManager.Services;

using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Api.Operations.MediaManager.MediaTree
{
    public class MediaTreeService : Service, IMediaTreeService
    {
        private readonly IRepository repository;
        
        private readonly IMediaFileService fileService;

        private readonly IMediaFileUrlResolver fileUrlResolver;

        public MediaTreeService(IRepository repository, IMediaFileService fileService, IMediaFileUrlResolver fileUrlResolver)
        {
            this.repository = repository;
            this.fileService = fileService;
            this.fileUrlResolver = fileUrlResolver;
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

            return response;
        }

        private IList<MediaItemModel> LoadMediaTree<TEntity>(MediaType mediaType, bool includeArchived, bool loadFiles)
            where TEntity: Media
        {
            var query = repository
                    .AsQueryable<Media>()
                    .OrderBy(m => m.Title)
                    .Where(f => f.Type == mediaType && f.Original == null);

            if (mediaType == MediaType.Image)
            {
                query = query.Where(f => !(f is MediaImage) || (!((MediaImage)f).IsTemporary && ((MediaImage)f).IsUploaded == true));
            }
            else
            {
                query = query.Where(f => !(f is MediaFile) || (!((MediaFile)f).IsTemporary && ((MediaFile)f).IsUploaded == true));
            }

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

                                     ParentFolderId = media.Folder != null && !media.Folder.IsDeleted ? media.Folder.Id : (Guid?)null,
                                     Title = media.Title,
                                     MediaContentType = media is MediaFolder 
                                                            ? (MediaContentType)((int)MediaContentType.Folder) 
                                                            : (MediaContentType)((int)MediaContentType.File),
                                     Url = (media is MediaFile || media is MediaImage) ? fileUrlResolver.EnsureFullPathUrl(((MediaFile)media).PublicUrl) : null,
                                     IsArchived = media.IsArchived
                                 }).ToList();

            mediaItems.ForEach(media =>
                                   {
                                       if (media.MediaContentType == MediaContentType.File)
                                       {
                                           media.Url = fileService.GetDownloadFileUrl(mediaType, media.Id, media.Url);
                                       }
                                   });

            return GetChildren(mediaItems, null);
        }

        private List<MediaItemModel> GetChildren(List<MediaItemModel> allItems, Guid? parentId)
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