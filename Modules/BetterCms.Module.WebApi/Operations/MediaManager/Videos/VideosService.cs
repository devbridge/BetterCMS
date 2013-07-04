using System.Linq;

using BetterCms.Core.DataAccess;
using BetterCms.Module.Api.Helpers;
using BetterCms.Module.MediaManager.Models;

using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Api.Operations.MediaManager.Videos
{
    //
    // TODO: Currently everything works with MediaFile type. When video will be implemented, change classes to MediaVideo
    //
    public class VideosService : Service, IVideosService
    {
        private readonly IRepository repository;

        public VideosService(IRepository repository)
        {
            this.repository = repository;
        }

        public GetVideosResponse Get(GetVideosRequest request)
        {
            // TODO: throw new validation exception if request.IncludeVideos == false && request.IncludeFolders == false

            request.SetDefaultOrder("Title");

            var query = repository
                .AsQueryable<Media>()
                .Where(m => m.Original == null && m.Folder.Id == request.FolderId);

            if (!request.IncludeArchived)
            {
                query = query.Where(m => !m.IsArchived);
            }

            if (request.IncludeFolders && request.IncludeVideos)
            {
                query = query.Where(media => (media is MediaFile || media is MediaFolder));
            }
            else if (!request.IncludeFolders)
            {
                query = query.Where(media => media is MediaFile);
            }
            else
            {
                query = query.Where(media => media is MediaFolder);
            }

            query = query.ApplyTagsFilter(
                request,
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
                            MediaContentType = media is MediaFolder ? MediaContentType.Folder : MediaContentType.File,
                            VideoUrl = media is MediaFile ? ((MediaFile)media).PublicUrl : null,
                            // TODO: ThumbnailUrl

                        }).ToDataListResponse(request);

            return new GetVideosResponse
                       {
                           Data = listResponse
                       };
        }
    }
}