using BetterCms.Module.Api.Operations.MediaManager.Images.Image;

namespace BetterCms.Module.Api.Extensions
{
    public static class MediaExtensions
    {
        public static PutImageRequest ToPutRequest(this GetImageResponse response)
        {
            var model = MapPageModel(response);

            return new PutImageRequest { Data = model, ImageId = response.Data.Id };
        }

        public static PostImageRequest ToPostRequest(this GetImageResponse response)
        {
            var model = MapPageModel(response);

            return new PostImageRequest { Data = model };
        }

        private static SaveImageModel MapPageModel(GetImageResponse request)
        {
            var model = new SaveImageModel
                            {
                                Version = request.Data.Version,
                                Title = request.Data.Title,
                                Description = request.Data.Description,
                                Caption = request.Data.Caption,
                                FileSize = request.Data.FileSize,
                                ImageUrl = request.Data.ImageUrl,
                                Width = request.Data.Width,
                                Height = request.Data.Height,
                                ThumbnailUrl = request.Data.ThumbnailUrl,
                                ThumbnailWidth = request.Data.ThumbnailWidth,
                                ThumbnailHeight = request.Data.ThumbnailHeight,
                                ThumbnailSize = request.Data.ThumbnailSize,
                                IsArchived = request.Data.IsArchived,
                                FolderId = request.Data.FolderId,
                                PublishedOn = request.Data.PublishedOn,
                                OriginalFileName = request.Data.OriginalFileName,
                                OriginalFileExtension = request.Data.OriginalFileExtension,
                                OriginalWidth = request.Data.OriginalWidth,
                                OriginalHeight = request.Data.OriginalHeight,
                                OriginalSize = request.Data.OriginalSize,
                                OriginalUrl = request.Data.OriginalUrl,
                                FileUri = request.Data.FileUri,
                                IsUploaded = request.Data.IsUploaded,
                                IsTemporary = request.Data.IsTemporary,
                                IsCanceled = request.Data.IsCanceled,
                                OriginalUri = request.Data.OriginalUri,
                                ThumbnailUri = request.Data.ThumbnailUri
                            };

            return model;
        }
    }
}
