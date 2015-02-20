using System.IO;
using System.Linq;

using BetterModules.Core.DataAccess;
using BetterModules.Core.DataAccess.DataContext;
using BetterCms.Core.Exceptions.Api;

using BetterCms.Module.Api.Operations.MediaManager.Images.Image;
using BetterCms.Module.MediaManager.Models;
using BetterCms.Module.MediaManager.Services;

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
            MediaFolder parentFolder = null;
            if (request.Data.FolderId.HasValue)
            {
                parentFolder = repository.AsQueryable<MediaFolder>()
                    .Where(c => c.Id == request.Data.FolderId.Value && !c.IsDeleted)
                    .FirstOne();

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
                Title = request.Data.Title ?? Path.GetFileName(request.Data.FileName),
                Description = request.Data.Description,
                Size = request.Data.FileStream.Length,
                Folder = parentFolder,
                OriginalFileName = request.Data.FileName,
                OriginalFileExtension = Path.GetExtension(request.Data.FileName)
            };

            var savedImage = mediaImageService.UploadImageWithStream(request.Data.FileStream, mediaImage, request.Data.WaitForUploadResult);

            if (savedImage != null)
            {
                Events.MediaManagerEvents.Instance.OnMediaFileUploaded(savedImage);
            }

            return new UploadImageResponse
            {
                Data = savedImage.Id,
            };
        }
    }
}