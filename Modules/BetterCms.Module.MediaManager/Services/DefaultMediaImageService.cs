using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Helpers;

using BetterCms.Core.DataAccess;
using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Core.Exceptions;
using BetterCms.Core.Exceptions.Mvc;
using BetterCms.Core.Exceptions.Service;
using BetterCms.Core.Services.Storage;
using BetterCms.Module.MediaManager.Content.Resources;
using BetterCms.Module.MediaManager.Helpers;
using BetterCms.Module.MediaManager.Models;
using BetterCms.Module.Root.Mvc;

using NHibernate;

namespace BetterCms.Module.MediaManager.Services
{
    /// <summary>
    /// Default media image service.
    /// </summary>
    internal class DefaultMediaImageService : IMediaImageService
    {
        /// <summary>
        /// The thumbnail size.
        /// </summary>
        private static readonly Size ThumbnailSize = new Size(150, 150);

        /// <summary>
        /// The thumbnail image file prefix.
        /// </summary>
        private const string ThumbnailImageFilePrefix = "t_";

        /// <summary>
        /// The storage service.
        /// </summary>
        private readonly IStorageService storageService;

        /// <summary>
        /// The repository.
        /// </summary>
        private readonly IRepository repository;

        /// <summary>
        /// The unit of work.
        /// </summary>
        private readonly IUnitOfWork unitOfWork;

        /// <summary>
        /// The session factory provider.
        /// </summary>
        private readonly ISessionFactoryProvider sessionFactoryProvider;

        /// <summary>
        /// The media file service
        /// </summary>
        private readonly IMediaFileService mediaFileService;

        /// <summary>
        /// The image file format
        /// </summary>
        public static IDictionary<string, ImageFormat> transparencyFormats = new Dictionary<string, ImageFormat>(StringComparer.OrdinalIgnoreCase) { { "png", ImageFormat.Png }, { "gif", ImageFormat.Gif } };

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultMediaImageService" /> class.
        /// </summary>
        /// <param name="mediaFileService">The media file service.</param>
        /// <param name="storageService">The storage service.</param>
        /// <param name="repository">The repository.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="sessionFactoryProvider">The session factory provider.</param>
        public DefaultMediaImageService(IMediaFileService mediaFileService, IStorageService storageService, IRepository repository, ISessionFactoryProvider sessionFactoryProvider, IUnitOfWork unitOfWork)
        {
            this.mediaFileService = mediaFileService;
            this.sessionFactoryProvider = sessionFactoryProvider;
            this.storageService = storageService;
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Removes an image related files from the storage.
        /// </summary>
        /// <param name="mediaImageId">The media image id.</param>
        /// <param name="version">The version.</param>
        public void RemoveImageWithFiles(Guid mediaImageId, int version, bool doNotCheckVersion = false)
        {   
            var removeImageFileTasks = new List<Task>();
            var image = repository.AsQueryable<MediaImage>()
                          .Where(f => f.Id == mediaImageId)
                          .Select(f => new
                                           {
                                               IsUploaded = f.IsUploaded,
                                               FileUri = f.FileUri,
                                               IsOriginalUploaded = f.IsOriginalUploaded,
                                               OriginalUri = f.OriginalUri,
                                               IsThumbnailUploaded = f.IsThumbnailUploaded,
                                               ThumbnailUri = f.ThumbnailUri
                                           })
                          .FirstOrDefault();

            if (image == null)
            {
                throw new CmsException(string.Format("Image not found by given id={0}", mediaImageId));
            }

            try
            {
                if (image.IsUploaded.HasValue && image.IsUploaded.Value)
                {
                    removeImageFileTasks.Add(
                        new Task(
                            () =>
                                { storageService.RemoveObject(image.FileUri); }));
                }

                if (image.IsOriginalUploaded.HasValue && image.IsOriginalUploaded.Value)
                {
                    removeImageFileTasks.Add(
                        new Task(
                            () =>
                                { storageService.RemoveObject(image.OriginalUri); }));
                }

                if (image.IsThumbnailUploaded.HasValue && image.IsThumbnailUploaded.Value)
                {
                    removeImageFileTasks.Add(
                        new Task(
                            () =>
                                { storageService.RemoveObject(image.ThumbnailUri); }));
                }
                
                if (removeImageFileTasks.Count > 0)
                {
                    Task.Factory.ContinueWhenAll(
                        removeImageFileTasks.ToArray(),
                        result =>
                            { storageService.RemoveFolder(image.FileUri); });

                    removeImageFileTasks.ForEach(task => task.Start());
                }
            }
            finally
            {
                if (doNotCheckVersion)
                {
                    var media = repository.AsQueryable<MediaImage>().FirstOrDefault(f => f.Id == mediaImageId);
                    repository.Delete(media);
                }
                else
                {
                    repository.Delete<MediaImage>(mediaImageId, version);
                }
                unitOfWork.Commit();                
            }
        }

        /// <summary>
        /// Uploads the image.
        /// </summary>
        /// <param name="rootFolderId">The root folder id.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="fileLength">Length of the file.</param>
        /// <param name="fileStream">The file stream.</param>
        /// <returns>Image entity.</returns>
        public MediaImage UploadImage(Guid rootFolderId, string fileName, long fileLength, Stream fileStream, Guid reuploadMediaId)
        {
            MediaImage originalMedia;
            string folderName;
            string versionedFileName;
            Size size;
            if (!reuploadMediaId.HasDefaultValue())
            {
                originalMedia = repository.First<MediaImage>(image => image.Id == reuploadMediaId);
                fileName = string.Concat(Path.GetFileNameWithoutExtension(originalMedia.OriginalFileName), Path.GetExtension(fileName));
                folderName = Path.GetFileName(Path.GetDirectoryName(originalMedia.FileUri.OriginalString));
                versionedFileName = MediaImageHelper.CreateVersionedFileName(fileName, originalMedia.Version + 1);
            }
            else
            {
                folderName = mediaFileService.CreateRandomFolderName();
                versionedFileName = MediaImageHelper.CreateVersionedFileName(fileName, 1);
            }

            try
            {
                size = GetImageSize(fileStream);
            }
            catch (ImagingException ex)
            {
                var message = MediaGlobalization.MultiFileUpload_ImageFormatNotSuported;
                const string logMessage = "Failed to get image size.";
                throw new ValidationException(() => message, logMessage, ex);
            }

            using (var thumbnailImage = new MemoryStream())
            {
                CreatePngThumbnail(fileStream, thumbnailImage, ThumbnailSize);

                MediaImage image = new MediaImage();
                if (!rootFolderId.HasDefaultValue())
                {
                    image.Folder = repository.AsProxy<MediaFolder>(rootFolderId);
                }

                image.Title = Path.GetFileName(fileName);
                image.Caption = null;
                image.OriginalFileName = fileName;
                image.OriginalFileExtension = Path.GetExtension(fileName);
                image.Type = MediaType.Image;

                image.Width = size.Width;
                image.Height = size.Height;
                image.Size = fileLength;
                image.FileUri = mediaFileService.GetFileUri(MediaType.Image, folderName, versionedFileName);
                image.PublicUrl = mediaFileService.GetPublicFileUrl(MediaType.Image, folderName, versionedFileName);

                image.CropCoordX1 = null;
                image.CropCoordY1 = null;
                image.CropCoordX2 = null;
                image.CropCoordY2 = null;

                image.OriginalWidth = size.Width;
                image.OriginalHeight = size.Height;
                image.OriginalSize = fileLength;
                image.OriginalUri = mediaFileService.GetFileUri(MediaType.Image, folderName, MediaImageHelper.OriginalImageFilePrefix + versionedFileName);
                image.PublicOriginallUrl = mediaFileService.GetPublicFileUrl(MediaType.Image, folderName, MediaImageHelper.OriginalImageFilePrefix + versionedFileName);
                    

                image.ThumbnailWidth = ThumbnailSize.Width;
                image.ThumbnailHeight = ThumbnailSize.Height;
                image.ThumbnailSize = thumbnailImage.Length;
                image.ThumbnailUri = mediaFileService.GetFileUri(MediaType.Image, folderName, ThumbnailImageFilePrefix + Path.GetFileNameWithoutExtension(versionedFileName) + ".png");
                image.PublicThumbnailUrl = mediaFileService.GetPublicFileUrl(MediaType.Image, folderName, ThumbnailImageFilePrefix + Path.GetFileNameWithoutExtension(versionedFileName) + ".png");

                image.ImageAlign = null;
                image.IsTemporary = true;
                image.IsUploaded = null;
                image.IsThumbnailUploaded = null;
                image.IsOriginalUploaded = null;

                unitOfWork.BeginTransaction();
                repository.Save(image);
                unitOfWork.Commit();

                Task imageUpload = mediaFileService.UploadMediaFileToStorage<MediaImage>(fileStream, image.FileUri, image.Id, img => { img.IsUploaded = true; }, img => { img.IsUploaded = false; }, true);
                Task originalUpload = mediaFileService.UploadMediaFileToStorage<MediaImage>(fileStream, image.OriginalUri, image.Id, img => { img.IsOriginalUploaded = true; }, img => { img.IsOriginalUploaded = false; }, true);
                Task thumbnailUpload = mediaFileService.UploadMediaFileToStorage<MediaImage>(thumbnailImage, image.ThumbnailUri, image.Id, img => { img.IsThumbnailUploaded = true; }, img => { img.IsThumbnailUploaded = false; }, true);

                Task.Factory.ContinueWhenAll(
                    new[]
                        {
                            imageUpload, 
                            originalUpload, 
                            thumbnailUpload
                        },
                    result =>
                    {
                        // During uploading progress Cancel action can by executed. Need to remove uploaded images from the storage.
                        ExecuteActionOnThreadSeparatedSessionWithNoConcurrencyTracking(session =>
                            {
                                var media = session.Get<MediaImage>(image.Id);
                                var isUploaded = (media.IsUploaded.HasValue && media.IsUploaded.Value)
                                                  || (media.IsThumbnailUploaded.HasValue && media.IsThumbnailUploaded.Value)
                                                  || (media.IsOriginalUploaded.HasValue && media.IsOriginalUploaded.Value);
                                if (media.IsCanceled && isUploaded)
                                {
                                    RemoveImageWithFiles(media.Id, media.Version);
                                }
                            });
                    });

                imageUpload.Start();
                originalUpload.Start();
                thumbnailUpload.Start();

                return image;
            }
        }

        /// <summary>
        /// Gets a size of the image.
        /// </summary>
        /// <param name="imageStream">The image stream.</param>
        /// <returns>A size of the image.</returns>
        public Size GetImageSize(Stream imageStream)
        {
            try
            {
                imageStream.Seek(0, SeekOrigin.Begin);

                using (var img = Image.FromStream(imageStream))
                {                    
                    return img.Size;
                }
            }
            catch (ArgumentException e)
            {
                throw new ImagingException(string.Format("Stream {0} is not valid image stream. Can not determine image size.", imageStream.GetType()), e);
            }
        }        

        /// <summary>
        /// Created image thmbnail in PNG format: resizes the image and crops to fit.
        /// </summary>
        /// <param name="sourceStream">The source stream.</param>
        /// <param name="destinationStream">The destination stream.</param>
        /// <param name="size">The size.</param>
        private void CreatePngThumbnail(Stream sourceStream, Stream destinationStream, Size size)
        {
            using (var tempStream = new MemoryStream())
            {
                sourceStream.Seek(0, SeekOrigin.Begin);
                sourceStream.CopyTo(tempStream);               

                var image = new WebImage(tempStream);
               
                using (Image resizedBitmap = new Bitmap(size.Width, size.Height))
                {
                    using (Bitmap source = new Bitmap(new MemoryStream(image.GetBytes())))
                    {
                        using (Graphics g = Graphics.FromImage(resizedBitmap))
                        {
                            var destination = source;

                            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

                            var diff = (image.Width - image.Height) / 2.0;
                            if (diff > 0)
                            {
                                var x1 = Convert.ToInt32(Math.Floor(diff));
                                var y1 = 0;
                                var x2 = image.Height;
                                var y2 = image.Height;
                                var rect = new Rectangle(x1, y1, x2, y2);
                                destination = CropImage(destination, rect);
                            }
                            else if (diff < 0)
                            {
                                diff = Math.Abs(diff);

                                var x1 = 0;
                                var y1 = Convert.ToInt32(Math.Floor(diff));
                                var x2 = image.Width;
                                var y2 = image.Width;
                                var rect = new Rectangle(x1, y1, x2, y2);
                                destination = CropImage(destination, rect);
                            }

                            g.DrawImage(destination, 0, 0, size.Width, size.Height);
                        }
                    }
                    using (MemoryStream ms = new MemoryStream())
                    {
                        resizedBitmap.Save(ms, ImageFormat.Png);

                        var resizedImage = new WebImage(ms);
                        var bytes = resizedImage.GetBytes();
                        destinationStream.Write(bytes, 0, bytes.Length);      
                    }
                }
            }
        }

        /// <summary>
        /// Updates the thumbnail.
        /// </summary>
        /// <param name="mediaImage">The media image.</param>
        /// <param name="size">The size.</param>
        public void UpdateThumbnail(MediaImage mediaImage, Size size)
        {
            if (size.IsEmpty)
            {
                size = ThumbnailSize;
            }

            var downloadResponse = storageService.DownloadObject(mediaImage.FileUri);

            using (var memoryStream = new MemoryStream())
            {
                CreatePngThumbnail(downloadResponse.ResponseStream, memoryStream, size);

                mediaImage.ThumbnailWidth = size.Width;
                mediaImage.ThumbnailHeight = size.Height;
                mediaImage.ThumbnailSize = memoryStream.Length;

                storageService.UploadObject(new UploadRequest { InputStream = memoryStream, Uri = mediaImage.ThumbnailUri, IgnoreAccessControl = true});
            }
        }

        private void ExecuteActionOnThreadSeparatedSessionWithNoConcurrencyTracking(Action<ISession> work)
        {
            using (var session = sessionFactoryProvider.OpenSession(false))
            {
                try
                {
                    lock (this)
                    {
                        work(session);
                    }
                }
                finally
                {
                    session.Close();
                }
            }
        }

        /// <summary>
        /// Crops the image.
        /// </summary>
        /// <param name="img">The img.</param>
        /// <param name="cropArea">The crop area.</param>
        /// <returns>Cropped image</returns>
        private static Bitmap CropImage(Bitmap img, Rectangle cropArea)
        {
            Bitmap bmpCrop = img.Clone(cropArea, img.PixelFormat);
            return bmpCrop;
        }
    }
}