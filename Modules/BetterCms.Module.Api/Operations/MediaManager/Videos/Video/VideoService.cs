using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.DataAccess;
using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Module.MediaManager.Models;

using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Api.Operations.MediaManager.Videos.Video
{
    public class VideoService : Service, IVideoService
    {
        private readonly IRepository repository;

        public VideoService(IRepository repository)
        {
            this.repository = repository;
        }

        //
        // TODO: implement with MediaVideo (instead of MediaFile), when it'll be implemented
        //
        public GetVideoResponse Get(GetVideoRequest request)
        {
            var model = repository
                .AsQueryable<MediaFile>(media => media.Id == request.VideoId && media.Type == MediaType.Video)
                .Select(media => new VideoModel
                    {
                        Id = media.Id,
                        Version = media.Version,
                        CreatedBy = media.CreatedByUser,
                        CreatedOn = media.CreatedOn,
                        LastModifiedBy = media.ModifiedByUser,
                        LastModifiedOn = media.ModifiedOn,

                        Title = media.Title,
                        VideoUrl = media.PublicUrl,
                        IsArchived = media.IsArchived,
                        FolderId = media.Folder.Id,
                        FolderName = media.Folder.Title,
                        PublishedOn = media.PublishedOn,
                        ThumbnailCaption = media.Image.Caption,
                        ThumbnailUrl = media.Image.PublicThumbnailUrl,
                        ThumbnailId = media.Image.Id
                    })
                .FirstOne();

            IList<TagModel> tags;
            if (request.IncludeTags)
            {
                tags =
                    repository.AsQueryable<MediaTag>(mediaTag => mediaTag.Media.Id == request.VideoId)
                              .Select(media => new TagModel
                                    {
                                        Id = media.Tag.Id,
                                        Version = media.Tag.Version,
                                        CreatedBy = media.Tag.CreatedByUser,
                                        CreatedOn = media.Tag.CreatedOn,
                                        LastModifiedBy = media.Tag.ModifiedByUser,
                                        LastModifiedOn = media.Tag.ModifiedOn,

                                        Name = media.Tag.Name
                                    })
                              .ToList();
            }
            else
            {
                tags = null;
            }

            return new GetVideoResponse
                       {
                           Data = model,
                           Tags = tags
                       };
        }
    }
}