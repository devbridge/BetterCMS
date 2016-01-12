using System.IO;
using System.Linq;

using BetterCms.Core.DataAccess;
using BetterCms.Core.DataAccess.DataContext;
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

        private readonly ICmsConfiguration configuration;

        public UploadImageService(IRepository repository, IMediaImageService mediaImageService, ICmsConfiguration configuration)
        {
            this.repository = repository;
            this.mediaImageService = mediaImageService;
            this.configuration = configuration;
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

            var maxLength = configuration.Storage.MaximumFileNameLength > 0 ? configuration.Storage.MaximumFileNameLength : 100;
            // Fix for IIS express + IE (if full path is returned)
            var fileName = Path.GetFileName(request.Data.FileName);
            if (fileName.Length > maxLength)
            {
                fileName = string.Concat(Path.GetFileNameWithoutExtension(fileName.Substring(0, maxLength)), Path.GetExtension(fileName));
            }

            var mediaImage = new MediaImage
            {
                Id = request.Data.Id.GetValueOrDefault(),
                Type = Module.MediaManager.Models.MediaType.Image,
                Caption = request.Data.Caption,
                Title = request.Data.Title ?? fileName,
                Description = request.Data.Description,
                Size = request.Data.FileStream.Length,
                Folder = parentFolder,
                OriginalFileName = fileName,
                OriginalFileExtension = Path.GetExtension(fileName)
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