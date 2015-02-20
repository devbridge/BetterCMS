using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

using BetterCms.Core.Services.Storage;

using BetterCms.Module.MediaManager.Helpers;
using BetterCms.Module.MediaManager.Models;
using BetterCms.Module.MediaManager.Services;
using BetterCms.Module.MediaManager.ViewModels.Images;

using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Services;

using BetterModules.Core.Web.Mvc.Commands;

namespace BetterCms.Module.MediaManager.Command.Images.SaveImage
{
    /// <summary>
    /// Command to save image properties.
    /// </summary>
    public class SaveImageDataCommand : CommandBase, ICommandIn<ImageViewModel>
    {
        /// <summary>
        /// Gets or sets the media image service.
        /// </summary>
        /// <value>
        /// The media image service.
        /// </value>
        public IMediaImageService MediaImageService { get; set; }

        /// <summary>
        /// Gets or sets the storage service.
        /// </summary>
        /// <value>
        /// The storage service.
        /// </value>
        public IStorageService StorageService { get; set; }

        /// <summary>
        /// The tag service.
        /// </summary>
        /// <value>
        /// The tag service.
        /// </value>
        public ITagService TagService { get; set; }

        /// <summary>
        /// The category service
        /// </summary>
        public ICategoryService CategoryService { get; set; }

        /// <summary>
        /// Executes this command.
        /// </summary>
        /// <param name="request">The request.</param>
        public void Execute(ImageViewModel request)
        {
            var mediaImage = Repository.First<MediaImage>(request.Id.ToGuidOrDefault());
            var archivedImage = MediaImageService.MoveToHistory(mediaImage);

            // Calling resize and after then crop
            var croppedFileStream = ResizeAndCropImage(mediaImage, request);

            mediaImage.Caption = request.Caption;
            mediaImage.Title = request.Title;
            mediaImage.Description = request.Description;
            mediaImage.ImageAlign = request.ImageAlign;

            CategoryService.CombineEntityCategories<Media, MediaCategory>(mediaImage, request.Categories); 

            if (croppedFileStream != null)
            {
                MediaImageService.SaveEditedImage(mediaImage, archivedImage, croppedFileStream, request.ShouldOverride);
            }
            else
            {
                MediaImageService.SaveImage(mediaImage);
            }

            UnitOfWork.BeginTransaction();

            // Save tags
            IList<Root.Models.Tag> newTags;
            TagService.SaveMediaTags(mediaImage, request.Tags, out newTags);

            UnitOfWork.Commit();

            // Notify.
            Events.MediaManagerEvents.Instance.OnMediaFileUpdated(mediaImage);
        }

        /// <summary>
        /// Resizes and crops the image.
        /// </summary>
        /// <param name="mediaImage">The image.</param>
        /// <param name="request">The request.</param>
        private MemoryStream ResizeAndCropImage(MediaImage mediaImage, ImageViewModel request)
        {
            int? x1 = request.CropCoordX1;
            int? x2 = request.CropCoordX2;
            int? y1 = request.CropCoordY1;
            int? y2 = request.CropCoordY2;

            var cropped = true;
            if ((x1 <= 0 && y1 <= 0 && ((x2 >= mediaImage.OriginalWidth && y2 >= mediaImage.OriginalHeight) || (x2 <= 0 && y2 <= 0))))
            {
                x1 = y1 = 0;
                x2 = mediaImage.OriginalWidth;
                y2 = mediaImage.OriginalHeight;
                cropped = false;
            }

            var newWidth = request.ImageWidth;
            var newHeight = request.ImageHeight;
            var resized = (newWidth != mediaImage.OriginalWidth || newHeight != mediaImage.OriginalHeight);

            MemoryStream memoryStream = null;

            var hasChanges = (mediaImage.Width != newWidth
                || mediaImage.Height != newHeight
                || x1 != mediaImage.CropCoordX1
                || x2 != mediaImage.CropCoordX2
                || y1 != mediaImage.CropCoordY1
                || y2 != mediaImage.CropCoordY2); 

            if (hasChanges)
            {
                DownloadResponse downloadResponse = StorageService.DownloadObject(mediaImage.OriginalUri);
                var dimensionsCalculator = new ImageDimensionsCalculator(newWidth, newHeight, mediaImage.OriginalWidth, mediaImage.OriginalHeight, x1, x2, y1, y2);
                using (var image = Image.FromStream(downloadResponse.ResponseStream))
                {
                    var destination = image;
                    var codec = ImageHelper.GetImageCodec(destination);

                    if (resized)
                    {
                        destination = ImageHelper.Resize(destination, new Size { Width = newWidth, Height = newHeight });
                    }

                    if (cropped)
                    {
                        var width = dimensionsCalculator.ResizedCroppedWidth;
                        var heigth = dimensionsCalculator.ResizedCroppedHeight;
                        var cropX12 = dimensionsCalculator.ResizedCropCoordX1.Value;
                        var cropY12 = dimensionsCalculator.ResizedCropCoordY1.Value;

                        Rectangle rec = new Rectangle(cropX12, cropY12, width, heigth);
                        destination = ImageHelper.Crop(destination, rec);
                    }

                    memoryStream = new MemoryStream();
                    destination.Save(memoryStream, codec, null);
                    mediaImage.Size = memoryStream.Length;
                }

                mediaImage.CropCoordX1 = x1;
                mediaImage.CropCoordY1 = y1;
                mediaImage.CropCoordX2 = x2;
                mediaImage.CropCoordY2 = y2;

                mediaImage.Width = newWidth;
                mediaImage.Height = newHeight;
            }

            return memoryStream;
        }

        /// <summary>
        /// Extracts the name of the real file.
        /// </summary>
        /// <param name="fileUri">The file URI.</param>
        /// <param name="origFileName">Name of the original file.</param>
        /// <param name="version">The version.</param>
        /// <returns>
        /// File name with new applied version
        /// </returns>
        private static Uri ApplyVersionToFileUri(Uri fileUri, string origFileName, int version)
        {
            return new Uri(ApplyVersionToFileUrl(fileUri.OriginalString, origFileName, version));
        }

        /// <summary>
        /// Applies the version to file URL.
        /// </summary>
        /// <param name="fileUrl">The file URL.</param>
        /// <param name="originalFileUrl">The original file URL.</param>
        /// <param name="version">The version.</param>
        /// <returns>
        /// File name with new applied version
        /// </returns>
        private static string ApplyVersionToFileUrl(string fileUrl, string origFileName, int version)
        {
            origFileName = Path.GetFileNameWithoutExtension(origFileName);
            var realOldFileName = Path.GetFileNameWithoutExtension(fileUrl);
            var realFileNamePath = fileUrl.Substring(0, fileUrl.LastIndexOf(Path.GetFileName(fileUrl)));
            var realFileName = Path.Combine(realFileNamePath, string.Concat(realOldFileName.Substring(0, realOldFileName.IndexOf(origFileName)), origFileName, Path.GetExtension(fileUrl)));

            return MediaImageHelper.CreateVersionedFileName(realFileName, version);
        }
    }
}
