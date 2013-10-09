using System;
using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.DataAccess;
using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Module.MediaManager.Models;
using BetterCms.Module.MediaManager.Services;

using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Api.Operations.MediaManager.Images.Image
{
    public class ImageService : Service, IImageService
    {
        private readonly IRepository repository;

        private readonly IMediaFileUrlResolver fileUrlResolver;

        public ImageService(IRepository repository, IMediaFileUrlResolver fileUrlResolver)
        {
            this.repository = repository;
            this.fileUrlResolver = fileUrlResolver;
        }

        public GetImageResponse Get(GetImageRequest request)
        {
            var model = repository
                .AsQueryable<MediaImage>(media => media.Id == request.ImageId && media.Type == MediaType.Image)
                .Select(media => new ImageModel
                                     {
                                         Id = media.Id,
                                         Version = media.Version,
                                         CreatedBy = media.CreatedByUser,
                                         CreatedOn = media.CreatedOn,
                                         LastModifiedBy = media.ModifiedByUser,
                                         LastModifiedOn = media.ModifiedOn,

                                         Title = media.Title,
                                         Description = media.Description,
                                         Caption = media.Caption,
                                         FileExtension = media.OriginalFileExtension,
                                         FileSize = media.Size,
                                         ImageUrl = media.PublicUrl,
                                         Width = media.Width,
                                         Height = media.Height,
                                         ThumbnailUrl = media.PublicThumbnailUrl,
                                         ThumbnailWidth = media.ThumbnailWidth,
                                         ThumbnailHeight = media.ThumbnailHeight,
                                         ThumbnailSize = media.ThumbnailSize,
                                         IsArchived = media.IsArchived,
                                         FolderId = media.Folder != null && !media.Folder.IsDeleted ? media.Folder.Id : (Guid?)null,
                                         FolderName = media.Folder != null && !media.Folder.IsDeleted ? media.Folder.Title : null,
                                         PublishedOn = media.PublishedOn,
                                         OriginalFileName = media.OriginalFileName,
                                         OriginalFileExtension = media.OriginalFileExtension,
                                         OriginalWidth = media.OriginalWidth,
                                         OriginalHeight = media.OriginalHeight,
                                         OriginalSize = media.OriginalSize,
                                         OriginalUrl = media.PublicOriginallUrl
                                     })
                .FirstOne();

            model.ImageUrl = fileUrlResolver.EnsureFullPathUrl(model.ImageUrl);
            model.ThumbnailUrl = fileUrlResolver.EnsureFullPathUrl(model.ThumbnailUrl);
            model.OriginalUrl = fileUrlResolver.EnsureFullPathUrl(model.OriginalUrl);

            IList<TagModel> tags;
            if (request.Data.IncludeTags)
            {
                tags =
                    repository.AsQueryable<MediaTag>(mediaTag => mediaTag.Media.Id == request.ImageId && !mediaTag.Tag.IsDeleted)
                              .OrderBy(mediaTag => mediaTag.Tag.Name)
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

            return new GetImageResponse
                       {
                           Data = model,
                           Tags = tags
                       };
        }
    }
}