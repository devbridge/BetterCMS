using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;

using BetterModules.Core.DataAccess;
using BetterModules.Core.DataAccess.DataContext;
using BetterCms.Core.Exceptions;
using BetterCms.Core.Exceptions.Mvc;
using BetterCms.Core.Exceptions.Service;
using BetterCms.Core.Services.Storage;
using BetterCms.Module.MediaManager.Content.Resources;
using BetterCms.Module.MediaManager.Helpers;
using BetterCms.Module.MediaManager.Models;
using BetterCms.Module.Root.Mvc;

using Common.Logging;

using NHibernate;
using NHibernate.Linq;

namespace BetterCms.Module.MediaManager.Services
{
    /// <summary>
    /// Default media image service.
    /// </summary>
    internal class DefaultMediaImageService : IMediaImageService
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// The thumbnail size.
        /// </summary>
        private static readonly Size ThumbnailSize = new Size(150, 150);

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

        private readonly IMediaImageVersionPathService mediaImageVersionPathService;

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
        /// <param name="mediaImageVersionPathService"></param>
        public DefaultMediaImageService(IMediaFileService mediaFileService, IStorageService storageService, 
            IRepository repository, ISessionFactoryProvider sessionFactoryProvider, IUnitOfWork unitOfWork,
            IMediaImageVersionPathService mediaImageVersionPathService)
        {
            this.mediaFileService = mediaFileService;
            this.sessionFactoryProvider = sessionFactoryProvider;
            this.storageService = storageService;
            this.repository = repository;
            this.unitOfWork = unitOfWork;
            this.mediaImageVersionPathService = mediaImageVersionPathService;
        }

        /// <summary>
        /// Removes an image related files from the storage.
        /// </summary>
        /// <param name="mediaImageId">The media image id.</param>
        /// <param name="version">The version.</param>
        public void RemoveImageWithFiles(Guid mediaImageId, int version, bool doNotCheckVersion = false, bool originalWasNotUploaded = false)
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

                if (image.IsOriginalUploaded.HasValue && image.IsOriginalUploaded.Value && !originalWasNotUploaded)
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
                        {
                            // TODO: add functionality to remove folder if it is empty
                        });

                    removeImageFileTasks.ForEach(task => task.Start());
                }
            }
            finally
            {
                if (doNotCheckVersion)
                {
                    var media = repository.AsQueryable<MediaImage>().FirstOrDefault(f => f.Id == mediaImageId);
                    var archivedImage = RevertChanges(media);
                    if (archivedImage != null)
                    {
                        repository.Delete(archivedImage);
                    }
                }
                else
                {
                    //var media = repository.AsQueryable<MediaImage>().FirstOrDefault(f => f.Id == mediaImageId);
                    //var archivedImage = RevertChanges(media);
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
        /// <param name="reuploadMediaId">The reupload media identifier.</param>
        /// <param name="overrideUrl">if set to <c>true</c> override URL.</param>
        /// <returns>
        /// Image entity.
        /// </returns>
        public MediaImage UploadImage(Guid rootFolderId, string fileName, long fileLength, Stream fileStream, Guid reuploadMediaId, bool overrideUrl = true)
        {
            overrideUrl = false; // TODO: temporary disabling feature #1055.
            using (var thumbnailFileStream = new MemoryStream())
            {
                var folderName = mediaFileService.CreateRandomFolderName();

                fileStream = RotateImage(fileStream);
                var size = GetSize(fileStream);

                CreatePngThumbnail(fileStream, thumbnailFileStream, ThumbnailSize);

                if (!reuploadMediaId.HasDefaultValue())
                {
                    // Re-uploading image: Get original image, folder name, file extension, file name
                    MediaImage reuploadImage = (MediaImage)repository.First<MediaImage>(image => image.Id == reuploadMediaId).Clone();
                    reuploadImage.IsTemporary = true;
                    var publicFileName = RemoveInvalidHtmlSymbols(MediaImageHelper.CreatePublicFileName(fileName, Path.GetExtension(fileName)));

                    // Create new original image and upload file stream to the storage
                    reuploadImage = CreateImage(rootFolderId, fileName, Path.GetExtension(fileName), fileName, size, fileLength, thumbnailFileStream.Length);
                    mediaImageVersionPathService.SetPathForNewOriginal(reuploadImage, folderName, publicFileName);

                    unitOfWork.BeginTransaction();
                    repository.Save(reuploadImage);
                    unitOfWork.Commit();

                    StartTasksForImage(reuploadImage, fileStream, thumbnailFileStream, false);

                    return reuploadImage;
                }
                else
                {
                    // Uploading new image
                    var publicFileName = RemoveInvalidHtmlSymbols(MediaImageHelper.CreatePublicFileName(fileName, Path.GetExtension(fileName)));

                    // Create new original image and upload file stream to the storage
                    MediaImage originalImage = CreateImage(rootFolderId, fileName, Path.GetExtension(fileName), fileName, size, fileLength, thumbnailFileStream.Length);
                    mediaImageVersionPathService.SetPathForNewOriginal(originalImage, folderName, publicFileName);

                    unitOfWork.BeginTransaction();
                    repository.Save(originalImage);
                    unitOfWork.Commit();

                    StartTasksForImage(originalImage, fileStream, thumbnailFileStream, false);

                    return originalImage;
                }
            }
        }

        public MediaImage UploadImageWithStream(Stream fileStream, MediaImage image, bool waitForUploadResult = false)
        {
            using (var thumbnailFileStream = new MemoryStream())
            {
                fileStream = RotateImage(fileStream);
                var size = GetSize(fileStream);

                CreatePngThumbnail(fileStream, thumbnailFileStream, ThumbnailSize);

                var folderName = mediaFileService.CreateRandomFolderName();
                var publicFileName = RemoveInvalidHtmlSymbols(MediaImageHelper.CreatePublicFileName(image.OriginalFileName, image.OriginalFileExtension));

                // Create new original image and upload file stream to the storage
                var originalImage = CreateImage(null, image.OriginalFileName, image.OriginalFileExtension, image.Title, size, image.Size, thumbnailFileStream.Length, image);
                mediaImageVersionPathService.SetPathForNewOriginal(originalImage, folderName, publicFileName);

                unitOfWork.BeginTransaction();
                repository.Save(originalImage);

                if (!waitForUploadResult)
                {
                    unitOfWork.Commit();
                }
                
                StartTasksForImage(originalImage, fileStream, thumbnailFileStream, false, waitForUploadResult);
                
                if (waitForUploadResult)
                {
                    unitOfWork.Commit();
                    Events.MediaManagerEvents.Instance.OnMediaFileUpdated(originalImage);
                }

                return originalImage;
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

        /// <summary>
        /// Makes image as original.
        /// </summary>
        /// <param name="image">The new original image.</param>
        /// <param name="originalImage">The current original image.</param>
        /// <param name="archivedImage">The archived image.</param>
        /// <param name="overrideUrl">To override public Url ot not.</param>
        /// <returns>The new original image.</returns>
        public MediaImage MakeAsOriginal(MediaImage image, MediaImage originalImage, MediaImage archivedImage, bool overrideUrl = true)
        {
            overrideUrl = false; // TODO: temporary disabling feature #1055.
            var folderName = Path.GetFileName(Path.GetDirectoryName(originalImage.FileUri.OriginalString));
            
            using (var fileStream = DownloadFileStream(image.PublicUrl))
            {
                string publicUrlTemp = string.Empty, 
                    publicThumbnailUrlTemp = string.Empty, 
                    publicOriginallUrlTemp = string.Empty;
                Uri fileUriTemp = null, thumbnailUriTemp = null, originalUriTemp = null;

                if (overrideUrl)
                {
                    publicUrlTemp = originalImage.PublicUrl;
                    fileUriTemp = originalImage.FileUri;
                    publicThumbnailUrlTemp = originalImage.PublicThumbnailUrl;
                    thumbnailUriTemp = originalImage.ThumbnailUri;
                    publicOriginallUrlTemp = originalImage.PublicOriginallUrl;
                    originalUriTemp = originalImage.OriginalUri;
                }

                image.CopyDataTo(originalImage, false);
                MediaHelper.SetCollections(repository, image, originalImage);

                if (!overrideUrl)
                {
                    var publicFileName = RemoveInvalidHtmlSymbols(MediaImageHelper.CreateVersionedFileName(originalImage.OriginalFileName, GetVersion(originalImage)));
                    mediaImageVersionPathService.SetPathForNewOriginal(originalImage, folderName, publicFileName, archivedImage.OriginalUri, archivedImage.PublicOriginallUrl);
                }
                else
                {
                    originalImage.PublicUrl = publicUrlTemp;
                    originalImage.FileUri = fileUriTemp;
                    originalImage.PublicThumbnailUrl = publicThumbnailUrlTemp;
                    originalImage.ThumbnailUri = thumbnailUriTemp;
                    originalImage.PublicOriginallUrl = publicOriginallUrlTemp;
                    originalImage.OriginalUri = originalUriTemp;
                }

                
                originalImage.Original = null;
                originalImage.PublishedOn = DateTime.Now;
                
                if (image.IsEdited())
                {
                    originalImage.PublicOriginallUrl = image.PublicOriginallUrl;
                    originalImage.OriginalUri = image.OriginalUri;
                }

                unitOfWork.BeginTransaction();
                repository.Save(originalImage);
                unitOfWork.Commit();

                if (!image.IsEdited())
                {
                    using (var fileStreamReplica = new MemoryStream())
                    {
                        fileStream.CopyTo(fileStreamReplica);
                        storageService.UploadObject(new UploadRequest { InputStream = fileStreamReplica, Uri = originalImage.OriginalUri, IgnoreAccessControl = true });
                    }
                }
                storageService.UploadObject(new UploadRequest { InputStream = fileStream, Uri = originalImage.FileUri, IgnoreAccessControl = true });

                UpdateThumbnail(originalImage, Size.Empty);

                return originalImage;
            }
        }

        /// <summary>
        /// Saves edited image as original.
        /// </summary>
        /// <param name="image">The edited image.</param>
        /// <param name="archivedImage">The archived image.</param>
        /// <param name="croppedImageFileStream">The stream with edited image.</param>
        /// <param name="overrideUrl">To override public url or not.</param>
        public void SaveEditedImage(MediaImage image, MediaImage archivedImage, MemoryStream croppedImageFileStream, bool overrideUrl = true)
        {
            overrideUrl = false; // TODO: temporary disabling feature #1055.
            var folderName = Path.GetFileName(Path.GetDirectoryName(image.FileUri.OriginalString));
            
            using (var fileStream = croppedImageFileStream ?? DownloadFileStream(image.PublicUrl))
            {
                image.Original = null;
                image.PublishedOn = DateTime.Now;

                if (!overrideUrl)
                {
                    var publicFileName = RemoveInvalidHtmlSymbols(MediaImageHelper.CreateVersionedFileName(image.OriginalFileName, GetVersion(image)));
                    mediaImageVersionPathService.SetPathForNewOriginal(image, folderName, publicFileName, archivedImage.OriginalUri, archivedImage.PublicOriginallUrl);
                }
                
                unitOfWork.BeginTransaction();
                repository.Save(image);
                unitOfWork.Commit();

                storageService.UploadObject(new UploadRequest { InputStream = fileStream, Uri = image.FileUri, IgnoreAccessControl = true });
                UpdateThumbnail(image, Size.Empty);
            }
        }

        public void SaveImage(MediaImage image)
        {
            unitOfWork.BeginTransaction();
            repository.Save(image);
            unitOfWork.Commit();
        }
        
        /// <summary>
        /// Moves current original image to history.
        /// </summary>
        /// <param name="originalImage">The current original image.</param>
        /// <returns>The archived image.</returns>
        public MediaImage MoveToHistory(MediaImage originalImage)
        {
            var clonnedOriginalImage = (MediaImage)originalImage.Clone();
            clonnedOriginalImage.Original = originalImage;

            var historicalFileName = MediaImageHelper.CreateHistoricalVersionedFileName(
                                originalImage.OriginalFileName,
                                originalImage.OriginalFileExtension);

            var folderName = Path.GetFileName(Path.GetDirectoryName(originalImage.FileUri.OriginalString));
            
            using (var originalFileStream = DownloadFileStream(clonnedOriginalImage.PublicUrl))
            {
                using (var originalThumbnailFileStream = DownloadFileStream(clonnedOriginalImage.PublicThumbnailUrl))
                {
                    mediaImageVersionPathService.SetPathForArchive(clonnedOriginalImage, folderName, historicalFileName);

                    unitOfWork.BeginTransaction();
                    repository.Save(clonnedOriginalImage);
                    unitOfWork.Commit();

                    StartTasksForImage(clonnedOriginalImage, originalFileStream, originalThumbnailFileStream, originalImage.IsEdited());
                }
            }
            return clonnedOriginalImage;
        }

        #region Private methods

        private MediaImage RevertChanges(MediaImage canceledImage)
        {
            var previousOriginal =
                repository.AsQueryable<MediaImage>().OrderByDescending(i => i.PublishedOn).FirstOrDefault(f => f.Original != null && f.Original.Id == canceledImage.Id);

            if (previousOriginal != null)
            {
                var folderName = Path.GetFileName(Path.GetDirectoryName(previousOriginal.FileUri.OriginalString));
                var publicFileName = RemoveInvalidHtmlSymbols(MediaImageHelper.CreatePublicFileName(previousOriginal.OriginalFileName, previousOriginal.OriginalFileExtension));

                // Get original file stream
                using (var fileStream = DownloadFileStream(previousOriginal.PublicUrl))
                {
                    // Get thumbnail file stream
                    using (var thumbnailFileStream = DownloadFileStream(previousOriginal.PublicThumbnailUrl))
                    {
                        previousOriginal.CopyDataTo(canceledImage);

                        mediaImageVersionPathService.SetPathForArchive(canceledImage, folderName, publicFileName);

                        StartTasksForImage(canceledImage, fileStream, thumbnailFileStream, previousOriginal.IsEdited());

                        canceledImage.Original = null;
                        unitOfWork.BeginTransaction();
                        repository.Save(canceledImage);
                        unitOfWork.Commit();
                    }
                }
            }

            return previousOriginal;
        }

        private Stream UpdateCodec(Stream fileStream, Stream originalFileStream)
        {
            var originalCodec = ImageHelper.GetImageCodec(Image.FromStream(originalFileStream));
            var uploadedImage = Image.FromStream(fileStream);
            var updatedWithCodecFileStream = new MemoryStream();
            uploadedImage.Save(updatedWithCodecFileStream, originalCodec, null);
            fileStream = updatedWithCodecFileStream;

            return fileStream;
        }

        private Stream RotateImage(Stream fileStream)
        {
            var originalImage = Image.FromStream(fileStream);

            if (originalImage.PropertyIdList.Contains(0x0112))
            {
                int rotationValue = originalImage.GetPropertyItem(0x0112).Value[0];
                var wasRotated = true;

                switch (rotationValue)
                {
                    case 1: // landscape, do nothing
                        break;

                    case 8: // rotated 90 right
                        // de-rotate:
                        originalImage.RotateFlip(RotateFlipType.Rotate270FlipNone);
                        break;

                    case 3: // bottoms up
                        originalImage.RotateFlip(RotateFlipType.Rotate180FlipNone);
                        break;

                    case 6: // rotated 90 left
                        originalImage.RotateFlip(RotateFlipType.Rotate90FlipNone);
                        break;
                    default:
                        wasRotated = false;
                        break;
                }

                if (wasRotated)
                {
                    var rotatedStream = new MemoryStream();
                    var codec = ImageHelper.GetImageCodec(originalImage);
                    if (codec == null)
                    {
                        originalImage.Save(rotatedStream, ImageFormat.Bmp);
                    }
                    else
                    {
                        originalImage.Save(rotatedStream, codec, null);
                    }
                    fileStream = rotatedStream;
                }
            }

            return fileStream;
        }

        private void UpdateImageProperties(
            MediaImage image,
            Guid? rootFolderId,
            string fileName,
            string extension,
            string imageTitle,
            Size size,
            long fileLength,
            long thumbnailImageLength)
        {
            if (rootFolderId != null && !((Guid)rootFolderId).HasDefaultValue())
            {
                image.Folder = repository.AsProxy<MediaFolder>((Guid)rootFolderId);
            }

            image.Title = Path.GetFileName(imageTitle);
            image.Caption = null;
            image.Size = fileLength;
            image.IsTemporary = true;
            

            image.OriginalFileName = fileName;
            image.OriginalFileExtension = extension;
            image.Type = MediaType.Image;

            image.Width = size.Width;
            image.Height = size.Height;


            image.CropCoordX1 = null;
            image.CropCoordY1 = null;
            image.CropCoordX2 = null;
            image.CropCoordY2 = null;

            image.OriginalWidth = size.Width;
            image.OriginalHeight = size.Height;
            image.OriginalSize = fileLength;

            image.ThumbnailWidth = ThumbnailSize.Width;
            image.ThumbnailHeight = ThumbnailSize.Height;
            image.ThumbnailSize = thumbnailImageLength;

            image.ImageAlign = null;

            image.IsUploaded = null;
            image.IsThumbnailUploaded = null;
            image.IsOriginalUploaded = null;
        }

        private MediaImage CreateImage(
            Guid? rootFolderId,
            string fileName,
            string extension,
            string imageTitle,
            Size size,
            long fileLength,
            long thumbnailImageLength,
            MediaImage filledInImage = null)
        {
            MediaImage image;

            if (filledInImage == null)
            {
                image = new MediaImage();

                if (rootFolderId != null && !((Guid)rootFolderId).HasDefaultValue())
                {
                    image.Folder = repository.AsProxy<MediaFolder>((Guid)rootFolderId);
                }

                image.Title = Path.GetFileName(imageTitle);
                image.Caption = null;
                image.Size = fileLength;
                image.IsTemporary = true;
            }
            else
            {
                image = filledInImage;
            }


            image.OriginalFileName = fileName;
            image.OriginalFileExtension = extension;
            image.Type = MediaType.Image;

            image.Width = size.Width;
            image.Height = size.Height;


            image.CropCoordX1 = null;
            image.CropCoordY1 = null;
            image.CropCoordX2 = null;
            image.CropCoordY2 = null;

            image.OriginalWidth = size.Width;
            image.OriginalHeight = size.Height;
            image.OriginalSize = fileLength;

            image.ThumbnailWidth = ThumbnailSize.Width;
            image.ThumbnailHeight = ThumbnailSize.Height;
            image.ThumbnailSize = thumbnailImageLength;

            image.ImageAlign = null;

            image.IsUploaded = null;
            image.IsThumbnailUploaded = null;
            image.IsOriginalUploaded = null;

            return image;
        }

        private void CreatePngThumbnail(Stream sourceStream, Stream destinationStream, Size size)
        {
            using (var image = Image.FromStream(sourceStream))
            {
                Image destination = image;

                var diff = (destination.Width - destination.Height) / 2.0;
                if (diff > 0)
                {
                    var x1 = Convert.ToInt32(Math.Floor(diff));
                    var y1 = 0;
                    var x2 = destination.Height;
                    var y2 = destination.Height;
                    var rect = new Rectangle(x1, y1, x2, y2);
                    destination = ImageHelper.Crop(destination, rect);
                }
                else if (diff < 0)
                {
                    diff = Math.Abs(diff);

                    var x1 = 0;
                    var y1 = Convert.ToInt32(Math.Floor(diff));
                    var x2 = destination.Width;
                    var y2 = destination.Width;
                    var rect = new Rectangle(x1, y1, x2, y2);
                    destination = ImageHelper.Crop(destination, rect);
                }

                destination = ImageHelper.Resize(destination, size);

                destination.Save(destinationStream, ImageFormat.Png);
            }
        }
        
        private int GetVersion(MediaImage image)
        {
            var versionsCount = repository.AsQueryable<MediaImage>().Count(i => i.Original != null && i.Original.Id == image.Id);
            return versionsCount;
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

        private MemoryStream DownloadFileStream(string fileUrl)
        {
            byte[] imageData;
            using (var wc = new WebClient())
            {
                imageData = wc.DownloadData(fileUrl);
            }
            return new MemoryStream(imageData);
        }

        private Size GetSize(Stream fileStream)
        {
            try
            {
                var size = GetImageSize(fileStream);
                return size;
            }
            catch (ImagingException ex)
            {
                var message = MediaGlobalization.MultiFileUpload_ImageFormatNotSuported;
                const string logMessage = "Failed to get image size.";
                throw new ValidationException(() => message, logMessage, ex);
            }
        }

        private void StartTasksForImage(
            MediaImage mediaImage,
            Stream fileStream,
            MemoryStream thumbnailFileStream,
            bool shouldNotUploadOriginal = false,
            bool waitForUploadResult = false)
        {
            if (waitForUploadResult)
            {
                StartTasksForImageSync(mediaImage, fileStream, thumbnailFileStream, shouldNotUploadOriginal);
            }
            else
            {
                StartTasksForImageAsync(mediaImage, fileStream, thumbnailFileStream, shouldNotUploadOriginal);
            }
        }

        private void StartTasksForImageSync(
            MediaImage mediaImage, 
            Stream fileStream, 
            MemoryStream thumbnailFileStream, 
            bool shouldNotUploadOriginal = false)
        {
            mediaFileService.UploadMediaFileToStorageSync(
                fileStream,
                mediaImage.FileUri, 
                mediaImage, 
                img =>
                {
                    if (img != null)
                    {
                        img.IsUploaded = true;
                    }
                },
                img =>
                {
                    if (img != null)
                    {
                        img.IsUploaded = false;
                    }
                },
                true);

            if (!shouldNotUploadOriginal)
            {
                mediaFileService.UploadMediaFileToStorageSync(
                    fileStream,
                    mediaImage.OriginalUri,
                    mediaImage,
                    img =>
                    {
                        if (img != null)
                        {
                            img.IsOriginalUploaded = true;
                        }
                    },
                    img =>
                    {
                        if (img != null)
                        {
                            img.IsOriginalUploaded = false;
                        }
                    },
                    true);
            }

            mediaFileService.UploadMediaFileToStorageSync(
                thumbnailFileStream,
                mediaImage.ThumbnailUri, 
                mediaImage, 
                img =>
                {
                    if (img != null)
                    {
                        img.IsThumbnailUploaded = true;
                    }
                },
                img =>
                {
                    if (img != null)
                    {
                        img.IsThumbnailUploaded = false;
                    }
                },
                true);

            OnAfterUploadCompleted(mediaImage, shouldNotUploadOriginal);
        }
        
        private void StartTasksForImageAsync(
            MediaImage mediaImage, 
            Stream fileStream, 
            MemoryStream thumbnailFileStream, 
            bool shouldNotUploadOriginal = false)
        {
            var publicImageUpload = mediaFileService.UploadMediaFileToStorageAsync<MediaImage>(
                fileStream,
                mediaImage.FileUri,
                mediaImage.Id,
                (img, session) =>
                {
                    if (img != null)
                    {
                        img.IsUploaded = true;
                    }
                },
                img =>
                {
                    if (img != null)
                    {
                        img.IsUploaded = false;
                    }
                },
                true);

            var publicThumbnailUpload = mediaFileService.UploadMediaFileToStorageAsync<MediaImage>(
                thumbnailFileStream,
                mediaImage.ThumbnailUri,
                mediaImage.Id,
                (img, session) =>
                {
                    if (img != null)
                    {
                        img.IsThumbnailUploaded = true;
                    }
                },
                img =>
                {
                    if (img != null)
                    {
                        img.IsThumbnailUploaded = false;
                    }
                },
                true);

            var allTasks = new List<Task> { publicImageUpload, publicThumbnailUpload };

            Task publicOriginalUpload = null;
            if (!shouldNotUploadOriginal)
            {
                publicOriginalUpload = mediaFileService.UploadMediaFileToStorageAsync<MediaImage>(
                fileStream,
                mediaImage.OriginalUri,
                mediaImage.Id,
                (img, session) =>
                {
                    if (img != null)
                    {
                        img.IsOriginalUploaded = true;
                    }
                },
                img =>
                {
                    if (img != null)
                    {
                        img.IsOriginalUploaded = false;
                    }
                },
                true);
                allTasks.Add(publicOriginalUpload);
            }

            allTasks.ForEach(task => task.ContinueWith((t) => { Log.Error("Error observed while executing parallel task during image upload.", t.Exception); }, TaskContinuationOptions.OnlyOnFaulted));

            Task.Factory.ContinueWhenAll(
                allTasks.ToArray(),
                result =>
                    {
                        try
                        {
                            ExecuteActionOnThreadSeparatedSessionWithNoConcurrencyTracking(
                                session =>
                                    {
                                        var media = session.Get<MediaImage>(mediaImage.Id);
                                        if (media != null)
                                        {
                                            OnAfterUploadCompleted(media, shouldNotUploadOriginal);
                                        }
                                    });
                        }
                        catch (Exception ex)
                        {
                            Log.Error("Failed to finalize upload.", ex);
                        }
                    });

            publicImageUpload.Start();
            if (publicOriginalUpload != null)
            {
                publicOriginalUpload.Start();
            }
            publicThumbnailUpload.Start();
        }

        private void OnAfterUploadCompleted(MediaImage media, bool shouldNotUploadOriginal)
        {
            var isUploaded = (media.IsUploaded.HasValue && media.IsUploaded.Value) || (media.IsThumbnailUploaded.HasValue && media.IsThumbnailUploaded.Value)
                                 || ((media.IsOriginalUploaded.HasValue && media.IsOriginalUploaded.Value) && shouldNotUploadOriginal);
            if (media.IsCanceled && isUploaded)
            {
                RemoveImageWithFiles(media.Id, media.Version, false, shouldNotUploadOriginal);
            }
        }

        private static string RemoveInvalidHtmlSymbols(string fileName)
        {
            var invalidFileNameChars = Path.GetInvalidFileNameChars().ToList();
            invalidFileNameChars.AddRange(new[] { '+', ' ' });
            return HttpUtility.UrlEncode(invalidFileNameChars.Aggregate(fileName, (current, invalidFileNameChar) => current.Replace(invalidFileNameChar, '_')));
        }

        #endregion
    }
}