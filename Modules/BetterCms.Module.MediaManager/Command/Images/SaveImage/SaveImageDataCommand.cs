using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Web.Helpers;

using BetterCms.Core.Mvc.Commands;
using BetterCms.Core.Services.Storage;
using BetterCms.Module.MediaManager.Helpers;
using BetterCms.Module.MediaManager.Models;
using BetterCms.Module.MediaManager.Models.Extensions;
using BetterCms.Module.MediaManager.Services;
using BetterCms.Module.MediaManager.ViewModels.Images;
using BetterCms.Module.Root.Mvc;

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
        /// Executes this command.
        /// </summary>
        /// <param name="request">The request.</param>
        public void Execute(ImageViewModel request)
        {
            var mediaImage = Repository.First<MediaImage>(request.Id.ToGuidOrDefault());

            UnitOfWork.BeginTransaction();
            Repository.Save(mediaImage.CreateHistoryItem());
            mediaImage.PublishedOn = DateTime.Now;

            mediaImage.Caption = request.Caption;
            mediaImage.Title = request.Title;
            mediaImage.Description = request.Description;
            mediaImage.ImageAlign = request.ImageAlign;
            mediaImage.Version = request.Version.ToIntOrDefault();

            // Calling resize and after then crop
            ResizeAndCropImage(mediaImage, request);

            Repository.Save(mediaImage);

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
        private void ResizeAndCropImage(MediaImage mediaImage, ImageViewModel request)
        {
            int? x1 = request.CropCoordX1;
            int? x2 = request.CropCoordX2;
            int? y1 = request.CropCoordY1;
            int? y2 = request.CropCoordY2;

            Byte[] bytes = null;
 
            var cropped = true;
            if ((x1 <= 0 && y1 <= 0 && ((x2 >= mediaImage.OriginalWidth && y2 >= mediaImage.OriginalHeight) || (x2 <= 0 && y2 <= 0))))
            {
                x1 = y1 = x2 = y2 = null;
                cropped = false;
            }

            var newWidth = request.ImageWidth;
            var newHeight = request.ImageHeight;
            var resized = (newWidth != mediaImage.OriginalWidth || newHeight != mediaImage.OriginalHeight);

            var hasChanges = (mediaImage.Width != newWidth 
                || mediaImage.Height != newHeight 
                || x1 != mediaImage.CropCoordX1 
                || x2 != mediaImage.CropCoordX2
                || y1 != mediaImage.CropCoordY1 
                || y2 != mediaImage.CropCoordY2); 

            if (hasChanges)
            {
                var downloadResponse = StorageService.DownloadObject(mediaImage.OriginalUri);
                var image = new WebImage(downloadResponse.ResponseStream);
                var dimensionsCalculator = new ImageDimensionsCalculator(newWidth, newHeight, mediaImage.OriginalWidth, mediaImage.OriginalHeight, x1, x2, y1, y2);
                ImageFormat format = null;
                if (DefaultMediaImageService.transparencyFormats.TryGetValue(image.ImageFormat, out format))
                {
                    using (Image resizedBitmap = new Bitmap(newWidth, newHeight))
                    {
                        using (Bitmap source = new Bitmap(new MemoryStream(image.GetBytes())))
                        {
                            using (Graphics g = Graphics.FromImage(resizedBitmap))
                            {
                                if (resized)
                                {
                                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                                    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                                    g.DrawImage(source, 0, 0, newWidth, newHeight);
                                    using (MemoryStream ms = new MemoryStream())
                                    {
                                        resizedBitmap.Save(ms, format);
                                        image = new WebImage(ms.ToArray());
                                    }
                                }
                            }
                        }
                    }

                    if (cropped)
                    {
                        var width = dimensionsCalculator.ResizedCroppedWidth;
                        var heigth = dimensionsCalculator.ResizedCroppedHeight;
                        var cropX12 = dimensionsCalculator.CropCoordX1.Value;
                        var cropY12 = dimensionsCalculator.CropCoordY1.Value;

                        Rectangle rec = new Rectangle(cropX12, cropY12, width, heigth);
                        using (Bitmap source = new Bitmap(new MemoryStream(image.GetBytes())))
                        {
                            var resizedBitmap = source.Clone(rec, source.PixelFormat);
                            using (MemoryStream ms = new MemoryStream())
                            {
                                resizedBitmap.Save(ms, format);
                                image = new WebImage(ms.ToArray());
                                resizedBitmap.Dispose();
                            }
                        }
                    }
                }
                else
                {
                    if (resized)
                    {
                        image = image.Resize(newWidth, newHeight, false);
                    }
                    if (cropped)
                    {
                        var cropX1 = dimensionsCalculator.ResizedCropCoordX1.Value;
                        var cropY1 = dimensionsCalculator.ResizedCropCoordY1.Value;
                        var cropX2 = image.Width - dimensionsCalculator.ResizedCropCoordX2.Value;
                        var cropY2 = image.Height - dimensionsCalculator.ResizedCropCoordY2.Value;

                        // Fix for small resized images
                        if (cropX2 - cropX1 < image.Width && cropY2 - cropY1 < image.Height)
                        {
                            image = image.Crop(cropY1, cropX1, cropY2, cropX2);
                        }
                    }                
                }

                // Change image file names depending on file version
                var newVersion = mediaImage.Version + 1;
                mediaImage.FileUri = ApplyVersionToFileUri(mediaImage.FileUri, mediaImage.OriginalFileName, newVersion);
                mediaImage.PublicUrl = ApplyVersionToFileUrl(mediaImage.PublicUrl, mediaImage.OriginalFileName, newVersion);

                mediaImage.ThumbnailUri = ApplyVersionToFileUri(mediaImage.ThumbnailUri, mediaImage.OriginalFileName, newVersion);
                mediaImage.PublicThumbnailUrl = ApplyVersionToFileUrl(mediaImage.PublicThumbnailUrl, mediaImage.OriginalFileName, newVersion);

                // Upload image to storage
                bytes = image.GetBytes();
                var memoryStream = new MemoryStream(bytes);
                StorageService.UploadObject(new UploadRequest { InputStream = memoryStream, Uri = mediaImage.FileUri, IgnoreAccessControl = true });
                
                mediaImage.Size = bytes.Length;

                // Update thumbnail
                MediaImageService.UpdateThumbnail(mediaImage, Size.Empty);
            }

            mediaImage.CropCoordX1 = x1;
            mediaImage.CropCoordY1 = y1;
            mediaImage.CropCoordX2 = x2;
            mediaImage.CropCoordY2 = y2;

            mediaImage.Width = newWidth;
            mediaImage.Height = newHeight;
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
