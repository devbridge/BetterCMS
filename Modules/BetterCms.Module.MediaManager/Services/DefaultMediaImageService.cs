using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;

using BetterCms.Configuration;
using BetterCms.Core.DataAccess;
using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Core.Exceptions;
using BetterCms.Core.Exceptions.Service;
using BetterCms.Core.Services.Storage;
using BetterCms.Core.Web;
using BetterCms.Module.MediaManager.Models;
using BetterCms.Module.Root.Mvc;

namespace BetterCms.Module.MediaManager.Services
{
    /// <summary>
    /// Default media image service.
    /// </summary>
    public class DefaultMediaImageService : IMediaImageService
    {
        /// <summary>
        /// The images folder name.
        /// </summary>
        private const string ImagesFolderName = "images";

        /// <summary>
        /// The thumbnail size.
        /// </summary>
        private static readonly Size ThumbnailSize = new Size(150, 150);

        /// <summary>
        /// The original image file prefix.
        /// </summary>
        private const string OriginalImageFilePrefix = "o_";

        /// <summary>
        /// The thumbnail image file prefix.
        /// </summary>
        private const string ThumbnailImageFilePrefix = "t_";

        /// <summary>
        /// The storage service.
        /// </summary>
        private readonly IStorageService storageService;

        /// <summary>
        /// The configuration.
        /// </summary>
        private readonly ICmsConfiguration configuration;

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
        /// The HTTP context accessor.
        /// </summary>
        private IHttpContextAccessor httpContextAccessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultMediaImageService" /> class.
        /// </summary>
        /// <param name="storageService">The storage service.</param>
        /// <param name="configuration">The configuration.</param>
        /// <param name="repository">The repository.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="sessionFactoryProvider">The session factory provider.</param>
        /// <param name="httpContextAccessor">The HTTP context accessor.</param>
        public DefaultMediaImageService(IStorageService storageService, ICmsConfiguration configuration, IRepository repository, IUnitOfWork unitOfWork, ISessionFactoryProvider sessionFactoryProvider, IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.sessionFactoryProvider = sessionFactoryProvider;
            this.storageService = storageService;
            this.configuration = configuration;
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Gets the content root.
        /// </summary>
        /// <param name="rootPath">The root path.</param>
        /// <returns>Root path.</returns>
        private string GetContentRoot(string rootPath)
        {
            if (configuration.Storage.ServiceType == StorageServiceType.FileSystem && VirtualPathUtility.IsAppRelative(rootPath))
            {
                return httpContextAccessor.MapPath(rootPath);
            }

            return rootPath;
        }

        /// <summary>
        /// Uploads the image.
        /// </summary>
        /// <param name="rootFolderId">The root folder id.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="fileLength">Length of the file.</param>
        /// <param name="fileStream">The file stream.</param>
        /// <returns>Image entity.</returns>
        public MediaImage UploadImage(Guid rootFolderId, string fileName, long fileLength, Stream fileStream)
        {
            string folderName = Guid.NewGuid().ToString().Replace("-", string.Empty);

            Size size = GetImageSize(fileStream);

            using (var thumbnailImage = new MemoryStream())
            {
                ResizeImageAndCropToFit(fileStream, thumbnailImage, ThumbnailSize);

                MediaImage image = new MediaImage();
                if (!rootFolderId.HasDefaultValue())
                {
                    image.Folder = repository.AsProxy<MediaFolder>(rootFolderId);
                }
                image.Title = Path.GetFileName(fileName);
                image.Caption = null;
                image.FileName = fileName;
                image.PublicUrl = new Uri(Path.Combine(GetContentRoot(configuration.Storage.PublicContentUrlRoot), Path.Combine(ImagesFolderName, folderName, fileName))).AbsolutePath;
                image.Type = MediaType.Image;

                image.Width = size.Width;
                image.Height = size.Height;
                image.Size = fileLength;
                image.FileUri = new Uri(Path.Combine(GetContentRoot(configuration.Storage.ContentRoot), ImagesFolderName, folderName, fileName));

                image.CropCoordX1 = null;
                image.CropCoordY1 = null;
                image.CropCoordX2 = null;
                image.CropCoordY2 = null;

                image.OriginalWidth = size.Width;
                image.OriginalHeight = size.Height;
                image.OriginalSize = fileLength;
                image.OriginalUri = new Uri(Path.Combine(GetContentRoot(configuration.Storage.ContentRoot), ImagesFolderName, folderName, OriginalImageFilePrefix + fileName));

                image.ThumbnailWidth = ThumbnailSize.Width;
                image.ThumbnailHeight = ThumbnailSize.Height;
                image.ThumbnailSize = thumbnailImage.Length;
                image.ThumbnailUri = new Uri(Path.Combine(GetContentRoot(configuration.Storage.ContentRoot), ImagesFolderName, folderName, ThumbnailImageFilePrefix + Path.GetFileName(fileName) + ".png"));

                image.ImageAlign = null;
                image.IsTemporary = true;
                image.IsStored = false;

                unitOfWork.BeginTransaction();
                repository.Save(image);
                unitOfWork.Commit();

                Task imageUpload = UploadImageToStorage(fileStream, image.FileUri);
                Task originalUpload = UploadImageToStorage(fileStream, image.FileUri);
                Task thumbnailUpload = UploadImageToStorage(thumbnailImage, image.FileUri);

                Task.Factory.ContinueWhenAll(new[] { imageUpload, originalUpload, thumbnailUpload }, tasks => new[]
                                                                                                                  {
                                                                                                                      UpdateImageAsUploadedToStorage(image.Id)
                                                                                                                  });
                imageUpload.Start();
                originalUpload.Start();
                thumbnailUpload.Start();

                return image;
            }
        }

        /// <summary>
        /// Updates the image as uploaded to storage.
        /// </summary>
        /// <param name="mediaId">The media id.</param>
        /// <returns>Upload task.</returns>
        private Task UpdateImageAsUploadedToStorage(Guid mediaId)
        {
            return Task.Factory.StartNew(
                () =>
                    {
                        using (var session = sessionFactoryProvider.SessionFactory.OpenSession())
                        {
                            var media = session.Get<MediaImage>(mediaId);
                            media.IsStored = true;
                            session.Flush();
                            session.Close();
                        }
                    });
        }

        /// <summary>
        /// Uploads the image to storage.
        /// </summary>
        /// <param name="sourceStream">The source stream.</param>
        /// <param name="fileUri">The file URI.</param>
        /// <returns>Upload task.</returns>
        private Task UploadImageToStorage(Stream sourceStream, Uri fileUri)
        {
            var stream = new MemoryStream();
            sourceStream.CopyTo(stream);

            var task = new Task(
                () =>
                    {
                            var upload = new UploadRequest();
                            upload.CreateDirectory = true;
                            upload.Uri = fileUri;
                            upload.InputStream = stream;

                            storageService.UploadObject(upload);
                    });

            task.ContinueWith(
                task1 =>
                    {
                        stream.Close();
                        stream.Dispose();
                    });

            return task;
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
                using (var img = Image.FromStream(imageStream))
                {
                    return img.Size;
                }
            }
            catch (Exception e)
            {
                throw new ImagingException(string.Format("Stream {0} is not valid image stream. Can not determine image size.", imageStream.GetType()), e);
            }
        }

        /// <summary>
        /// Resizes the image and crop to fit.
        /// </summary>
        /// <param name="sourceStream">The source stream.</param>
        /// <param name="destinationStream">The destination stream.</param>
        /// <param name="size">The size.</param>
        private void ResizeImageAndCropToFit(Stream sourceStream, Stream destinationStream, Size size)
        {
            var image = new WebImage(sourceStream);

            // Make image rectangular.
            WebImage croppedImage;
            var diff = (image.Width - image.Height) / 2.0;
            if (diff > 0)
            {
                croppedImage = image.Crop(0, Convert.ToInt32(Math.Floor(diff)), 0, Convert.ToInt32(Math.Ceiling(diff)));
            }
            else if (diff < 0)
            {
                diff = Math.Abs(diff);
                croppedImage = image.Crop(Convert.ToInt32(Math.Floor(diff)), 0, Convert.ToInt32(Math.Ceiling(diff)));
            }
            else
            {
                croppedImage = image;
            }

            var resizedImage = croppedImage.Resize(size.Width, size.Height);

            var bytes = resizedImage.GetBytes();
            destinationStream.Write(bytes, 0, bytes.Length);
        }

        /// <summary>
        /// Crops the image.
        /// </summary>
        /// <param name="mediaImageId">The media image id.</param>
        /// <param name="version">The version.</param>
        /// <param name="x1">The x1.</param>
        /// <param name="y1">The y1.</param>
        /// <param name="x2">The x2.</param>
        /// <param name="y2">The y2.</param>
        public void CropImage(Guid mediaImageId, int version, int x1, int y1, int x2, int y2)
        {
            var imageEntity = this.GetImageEntity(mediaImageId, version);

            var downloadResponse = storageService.DownloadObject(imageEntity.OriginalUri);
            var image = new WebImage(downloadResponse.ResponseStream);
            var croppedImage = image.Crop(y1, x1, image.Height - y2, image.Width - x2);
            var bytes = croppedImage.GetBytes();
            var memoryStream = new MemoryStream(bytes);
            storageService.UploadObject(new UploadRequest { InputStream = memoryStream, Uri = imageEntity.FileUri });

            imageEntity.Width = croppedImage.Width;
            imageEntity.Height = croppedImage.Height;
            imageEntity.Size = bytes.Length;
            imageEntity.Version = version;

            UpdateThumbnail(imageEntity, ThumbnailSize);

            repository.Save(imageEntity);
        }

        /// <summary>
        /// Resizes the image.
        /// </summary>
        /// <param name="mediaImageId">The media image id.</param>
        /// <param name="version">The version.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        public void ResizeImage(Guid mediaImageId, int version, int width, int height)
        {
            var imageEntity = this.GetImageEntity(mediaImageId, version);

            var downloadResponse = storageService.DownloadObject(imageEntity.OriginalUri);
            var image = new WebImage(downloadResponse.ResponseStream);
            var resizedImage = image.Resize(width, height, false);
            var bytes = resizedImage.GetBytes();
            var memoryStream = new MemoryStream(bytes);
            storageService.UploadObject(new UploadRequest { InputStream = memoryStream, Uri = imageEntity.FileUri });

            imageEntity.Width = resizedImage.Width;
            imageEntity.Height = resizedImage.Height;
            imageEntity.Size = bytes.Length;
            imageEntity.Version = version;

            UpdateThumbnail(imageEntity, ThumbnailSize);

            repository.Save(imageEntity);
        }

        /// <summary>
        /// Updates the thumbnail.
        /// </summary>
        /// <param name="mediaImage">The media image.</param>
        /// <param name="size">The size.</param>
        private void UpdateThumbnail(MediaImage mediaImage, Size size)
        {
            var downloadResponse = storageService.DownloadObject(mediaImage.FileUri);

            using (var memoryStream = new MemoryStream())
            {
                ResizeImageAndCropToFit(downloadResponse.ResponseStream, memoryStream, size);

                storageService.UploadObject(new UploadRequest { InputStream = memoryStream, Uri = mediaImage.ThumbnailUri });

                mediaImage.ThumbnailWidth = size.Width;
                mediaImage.ThumbnailHeight = size.Height;
                mediaImage.ThumbnailSize = memoryStream.Length;
            }
        }

        /// <summary>
        /// Gets the image entity.
        /// </summary>
        /// <param name="mediaImageId">The media image id.</param>
        /// <param name="version">The version.</param>
        /// <returns>Image entity.</returns>
        /// <exception cref="CmsException">Image not found or Image was modified.</exception>
        private MediaImage GetImageEntity(Guid mediaImageId, int version)
        {
            var imageEntity = repository.AsQueryable<MediaImage>().FirstOrDefault(f => f.Id == mediaImageId);
            if (imageEntity == null)
            {
                throw new CmsException(string.Format("Image not found by id={0}.", mediaImageId));
            }

            if (imageEntity.Version != version)
            {
                throw new CmsException(string.Format("Image with id={0} was modified.", mediaImageId));
            }

            if (!storageService.ObjectExists(imageEntity.OriginalUri))
            {
                throw new CmsException(string.Format("Image not found in the storage by uri={0}.", imageEntity.OriginalUri));
            }

            return imageEntity;
        }
    }
}