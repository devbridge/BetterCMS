using System;
using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.DataAccess;
using BetterCms.Core.Exceptions.Api;
using BetterCms.Module.Api.Operations.MediaManager.Images.Image;
using BetterCms.Module.MediaManager.Models;
using BetterCms.Module.MediaManager.Services;

using NHibernate.Linq;

namespace BetterCms.Module.Api.Operations.MediaManager.Images
{
    /// <summary>
    /// The upload image service.
    /// </summary>
    public class UploadImageService: IUploadImageService
    {
        private readonly IRepository repository;

        private readonly IMediaImageService mediaImageService;

        public UploadImageService(IRepository repository, IMediaImageService mediaImageService)
        {
            this.repository = repository;
            this.mediaImageService = mediaImageService;
        }

        /// <summary>
        /// Upload image from the stream.
        /// </summary>
        /// <param name="request">The upload image request.</param>
        /// <returns>The upload image response.</returns>
        public UploadImageResponse Post(UploadImageRequest request)
        {
            IEnumerable<MediaFolder> parentFolderFuture = null;
            if (request.Data.FolderId.HasValue)
            {
                parentFolderFuture = repository.AsQueryable<MediaFolder>()
                    .Where(c => c.Id == request.Data.FolderId.Value && !c.IsDeleted)
                    .ToFuture();
            }

            
            MediaFolder parentFolder = null;
            if (parentFolderFuture != null)
            {
                parentFolder = parentFolderFuture.First();
                if (parentFolder.Type != Module.MediaManager.Models.MediaType.Image)
                {
                    throw new CmsApiValidationException("Folder must be type of an image.");
                }
            }

            var mediaImage = new MediaImage
            {
                Id = request.Data.Id.GetValueOrDefault(),
                Type = Module.MediaManager.Models.MediaType.Image,
                Caption = request.Data.Caption,
                Title = request.Data.Title,
                Description = request.Data.Description
            };

            var savedImage = mediaImageService.UploadImage(
                parentFolder != null ? parentFolder.Id : Guid.Empty,
                request.Data.FileName,
                request.Data.FileStream.Length,
                request.Data.FileStream,
                Guid.Empty,
                mediaImage);

            if (savedImage != null)
            {
                Events.MediaManagerEvents.Instance.OnMediaFileUploaded(savedImage);
            }

            return new UploadImageResponse
            {
                Data = savedImage.Id
            };
        }
    }
}