using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Web.Helpers;

using BetterCms.Api;
using BetterCms.Core.Mvc.Commands;
using BetterCms.Core.Services.Storage;
using BetterCms.Module.MediaManager.Models;
using BetterCms.Module.MediaManager.Services;
using BetterCms.Module.MediaManager.ViewModels.Images;
using BetterCms.Module.Root.Mvc;

namespace BetterCms.Module.MediaManager.Command.Images.SaveImage
{
    /// <summary>
    /// Command to save image properties.
    /// </summary>
    public class SaveImageDataCommand : CommandBase, ICommand<ImageViewModel>
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
        /// Executes this command.
        /// </summary>
        /// <param name="request">The request.</param>
        public void Execute(ImageViewModel request)
        {
            var mediaImage = Repository.First<MediaImage>(request.Id.ToGuidOrDefault());

            mediaImage.Caption = request.Caption;
            mediaImage.Title = request.Title;
            mediaImage.ImageAlign = request.ImageAlign;
            mediaImage.Version = request.Version.ToIntOrDefault();

            // Calling resize and after then crop
            ResizeAndCropImage(mediaImage, request);

            Repository.Save(mediaImage);
            UnitOfWork.Commit();

            MediaManagerApiContext.Events.OnMediaFileUpdated(mediaImage);
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
                        var xRatio2 = (decimal)newWidth / (mediaImage.OriginalWidth == 0 ? newWidth : mediaImage.OriginalWidth);
                        var yRatio2 = (decimal)newHeight / (mediaImage.OriginalHeight == 0 ? newHeight : mediaImage.OriginalHeight);

                        var weigth = (int)((x2.Value - x1.Value) * xRatio2);
                        var heigth = (int)((y2.Value - y1.Value) * yRatio2);
                        var cropX12 = (int)Math.Floor(x1.Value * xRatio2);
                        var cropY12 = (int)Math.Floor(y1.Value * yRatio2);

                        Rectangle rec = new Rectangle(cropX12, cropY12, weigth, heigth);
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
                        var xRatio = (decimal)newWidth / (mediaImage.OriginalWidth == 0 ? newWidth : mediaImage.OriginalWidth);
                        var yRatio = (decimal)newHeight / (mediaImage.OriginalHeight == 0 ? newHeight : mediaImage.OriginalHeight);

                        var cropX1 = (int)Math.Floor(x1.Value * xRatio);
                        var cropY1 = (int)Math.Floor(y1.Value * yRatio);
                        var cropX2 = image.Width - (int)Math.Floor(x2.Value * xRatio);
                        var cropY2 = image.Height - (int)Math.Floor(y2.Value * yRatio);

                        // Fix for small resized images
                        if (cropX2 - cropX1 < image.Width && cropY2 - cropY1 < image.Height)
                        {
                            image = image.Crop(cropY1, cropX1, cropY2, cropX2);
                        }
                    }                
                }

                // Upload image to storage
                bytes = image.GetBytes();
                var memoryStream = new MemoryStream(bytes);
                StorageService.UploadObject(new UploadRequest { InputStream = memoryStream, Uri = mediaImage.FileUri });
                
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
    }
}
