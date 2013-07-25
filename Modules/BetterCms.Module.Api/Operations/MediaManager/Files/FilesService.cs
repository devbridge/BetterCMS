using System.Linq;

using BetterCms.Core.DataAccess;
using BetterCms.Module.Api.Helpers;
using BetterCms.Module.Api.Infrastructure;
using BetterCms.Module.MediaManager.Models;

using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Api.Operations.MediaManager.Files
{
    public class FilesService : Service, IFilesService
    {
        private readonly IRepository repository;

        public FilesService(IRepository repository)
        {
            this.repository = repository;
        }

        public GetFilesResponse Get(GetFilesRequest request)
        {
            // TODO: throw new validation exception if request.IncludeFiles == false && request.IncludeFolders == false

            request.Data.SetDefaultOrder("Title");

            var query = repository
                .AsQueryable<Media>()
                .Where(m => m.Original == null && m.Folder.Id == request.Data.FolderId && m.Type == MediaType.File);

            if (!request.Data.IncludeFolders)
            {
                query = query.Where(media => media.ContentType != Module.MediaManager.Models.MediaContentType.Folder);
            }

            if (!request.Data.IncludeFiles)
            {
                query = query.Where(media => media.ContentType != Module.MediaManager.Models.MediaContentType.File);
            }

            if (!request.Data.IncludeArchived)
            {
                query = query.Where(m => !m.IsArchived);
            }

            query = query.ApplyTagsFilter(
                request.Data,
                tagName => { return media => media.MediaTags.Any(tag => tag.Tag.Name == tagName); });

            var listResponse = query.Select(media =>
                    new MediaModel
                        {
                            Id = media.Id,
                            Version = media.Version,
                            CreatedBy = media.CreatedByUser,
                            CreatedOn = media.CreatedOn,
                            LastModifiedBy = media.ModifiedByUser,
                            LastModifiedOn = media.ModifiedOn,

                            Title = media.Title,
                            MediaContentType = media is MediaFolder 
                                                    ? (MediaContentType)((int)MediaContentType.Folder) 
                                                    : (MediaContentType)((int)MediaContentType.File),
                            FileExtension = media is MediaFile ? ((MediaFile)media).OriginalFileExtension : null,
                            FileSize = media is MediaFile ? ((MediaFile)media).Size : (long?)null,
                            FileUrl = media is MediaFile ? ((MediaFile)media).PublicUrl : null,
                            IsArchived = media.IsArchived,
                            ThumbnailCaption = media.Image.Caption,
                            ThumbnailUrl = media.Image.PublicThumbnailUrl,
                            ThumbnailId = media.Image.Id

                        }).ToDataListResponse(request);

            return new GetFilesResponse
                       {
                           Data = listResponse
                       };
        }
    }
}