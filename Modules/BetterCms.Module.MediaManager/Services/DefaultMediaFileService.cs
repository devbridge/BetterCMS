using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

using BetterCms.Configuration;

using BetterCms.Core.Exceptions;
using BetterCms.Core.Security;
using BetterCms.Core.Services;
using BetterCms.Core.Services.Storage;

using BetterCms.Module.MediaManager.Models;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;

using Common.Logging;

using BetterModules.Core.DataAccess;
using BetterModules.Core.DataAccess.DataContext;
using BetterModules.Core.DataAccess.DataContext.Fetching;
using BetterModules.Core.Exceptions.DataTier;
using BetterModules.Core.Web.Web;

using NHibernate;

namespace BetterCms.Module.MediaManager.Services
{
    internal class DefaultMediaFileService : IMediaFileService
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private readonly IStorageService storageService;

        private readonly IRepository repository;

        private readonly IUnitOfWork unitOfWork;

        private readonly ICmsConfiguration configuration;

        private readonly IHttpContextAccessor httpContextAccessor;

        private readonly ISessionFactoryProvider sessionFactoryProvider;

        private readonly IMediaFileUrlResolver mediaFileUrlResolver;

        private readonly ISecurityService securityService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultMediaFileService" /> class.
        /// </summary>
        /// <param name="storageService">The storage service.</param>
        /// <param name="repository">The repository.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="configuration">The configuration.</param>
        /// <param name="httpContextAccessor">The HTTP context accessor.</param>
        /// <param name="sessionFactoryProvider">The session factory provider.</param>
        /// <param name="mediaFileUrlResolver">The media file URL resolver.</param>
        /// <param name="securityService">The security service.</param>
        public DefaultMediaFileService(IStorageService storageService, IRepository repository, IUnitOfWork unitOfWork,
            ICmsConfiguration configuration, IHttpContextAccessor httpContextAccessor, ISessionFactoryProvider sessionFactoryProvider,
            IMediaFileUrlResolver mediaFileUrlResolver, ISecurityService securityService)
        {
            this.sessionFactoryProvider = sessionFactoryProvider;
            this.httpContextAccessor = httpContextAccessor;
            this.configuration = configuration;
            this.unitOfWork = unitOfWork;
            this.storageService = storageService;
            this.repository = repository;
            this.mediaFileUrlResolver = mediaFileUrlResolver;
            this.securityService = securityService;
        }

        public virtual void RemoveFile(Guid fileId, int version, bool doNotCheckVersion = false)
        {
            var file = EagerFetch.FetchMany(repository
                    .AsQueryable<MediaFile>(f => f.Id == fileId), f => f.AccessRules)
                .FirstOrDefault();

            if (file == null)
            {
                throw new CmsException(string.Format("A file was not found by given id={0}", fileId));
            }

            try
            {
                if (file.IsUploaded.HasValue && file.IsUploaded.Value)
                {
                    Task.Factory
                        .StartNew(() => { })
                        .ContinueWith(task =>
                        {
                            storageService.RemoveObject(file.FileUri);
                        })
                        .ContinueWith(task =>
                        {
                            storageService.RemoveFolder(file.FileUri);
                        });
                }
            }
            finally
            {
                if (!doNotCheckVersion)
                {
                    file.Version = version;
                }
                file.AccessRules.ToList().ForEach(file.RemoveRule);
                repository.Delete(file);
                unitOfWork.Commit();
            }
        }

        public void DeleteFileByMovingToTrash(Guid fileId)
        {
            var filesToTrash = repository
                .AsQueryable<MediaFile>(f => f.Id == fileId || f.Original.Id == fileId)
                .Fetch(f => f.AccessRules)
                .Fetch(f => f.Categories)
                .Fetch(f => f.MediaTags)
                .ToList();

            var mainFile = filesToTrash.FirstOrDefault(t => t.Id == fileId);

            if (mainFile == null)
            {
                throw new CmsException(string.Format("A file was not found by given id={0}", fileId));
            }

            TrashFiles(filesToTrash, false);
            
            Events.MediaManagerEvents.Instance.OnMediaFileDeleted(mainFile);
        }

        public void DeleteFolderByMovingToTrash(Guid folderId)
        {
            var folderToTrash = repository
                .AsQueryable<MediaFolder>(f => f.Id == folderId)
                .Fetch(f => f.Medias)
                // Without ToList() only first one media item would be fetched
                .ToList()
                .FirstOrDefault();

            if (folderToTrash == null)
            {
                throw new CmsException(string.Format("A folder was not found by given id={0}", folderId));
            }

            TrashFiles(new List<MediaFolder> { folderToTrash }, true);

            Events.MediaManagerEvents.Instance.OnMediaFolderDeleted(folderToTrash);
        }

        private void TrashFiles(IEnumerable<Media> filesToTrash, bool deleteFoldersAndSubMedias)
        {
            unitOfWork.BeginTransaction();
            var trashFolder = Path.Combine(GetContentRoot(configuration.Storage.ContentRoot), "trash");
            // in case of exception store moved files and restore them
            var movedFilesDic = new Dictionary<Uri, Uri>();
            try
            {
                foreach (var media in filesToTrash)
                {
                    if (media is MediaFile)
                    {
                        var mediaFile = (MediaFile)media;
                        foreach (var category in mediaFile.Categories)
                        {
                            unitOfWork.Session.Delete(category);
                        }
                        while (mediaFile.AccessRules.Any())
                        {
                            var rule = mediaFile.AccessRules.First();
                            mediaFile.RemoveRule(rule);
                            unitOfWork.Session.Delete(rule);
                        }
                        foreach (var tag in mediaFile.MediaTags)
                        {
                            unitOfWork.Session.Delete(tag);
                        }
                        var sourceUri = mediaFile.FileUri;
                        var destinationUri = new Uri(Path.Combine(trashFolder, GetFolderWithFileName(mediaFile.FileUri, mediaFile.Type)));

                        // when only file properties are updated record on database is duplicated but file is not duplicated
                        if (!movedFilesDic.ContainsValue(sourceUri))
                        {
                            storageService.CopyObject(sourceUri, destinationUri);
                            movedFilesDic.Add(destinationUri, sourceUri);
                        }

                        mediaFile.FileUri = destinationUri;
                        mediaFile.PublicUrl = GenerateTrashPublicUrl(mediaFile.FileUri, mediaFile.Type);

                        if (mediaFile is MediaImage)
                        {
                            var mediaImage = (MediaImage)mediaFile;
                            if (mediaImage.IsThumbnailUploaded.GetValueOrDefault())
                            {
                                var sourceImageUri = mediaImage.ThumbnailUri;
                                var destinationImageUri = new Uri(Path.Combine(trashFolder, GetFolderWithFileName(mediaImage.ThumbnailUri, mediaImage.Type)));
                                
                                // thumbnail image can be duplicated so we only need to update reference
                                if (!movedFilesDic.ContainsValue(sourceImageUri))
                                {
                                    storageService.CopyObject(sourceImageUri, destinationImageUri);
                                    movedFilesDic.Add(destinationImageUri, sourceImageUri);
                                }

                                mediaImage.ThumbnailUri = destinationImageUri;
                                mediaImage.PublicThumbnailUrl = GenerateTrashPublicUrl(mediaImage.ThumbnailUri, mediaImage.Type);
                            }
                            if (mediaImage.IsOriginalUploaded.GetValueOrDefault())
                            {
                                var sourceImageUri = mediaImage.OriginalUri;
                                var destinationImageUri = new Uri(Path.Combine(trashFolder, GetFolderWithFileName(mediaImage.OriginalUri, mediaImage.Type)));

                                // original image can be duplicated so we only need to update reference
                                if (!movedFilesDic.ContainsValue(sourceImageUri))
                                {
                                    storageService.CopyObject(sourceImageUri, destinationImageUri);
                                    movedFilesDic.Add(destinationImageUri, sourceImageUri);
                                }

                                mediaImage.OriginalUri = destinationImageUri;
                                mediaImage.PublicOriginallUrl = GenerateTrashPublicUrl(mediaImage.OriginalUri, mediaImage.Type);
                            }
                        }
                    }
                    else if (deleteFoldersAndSubMedias && media is MediaFolder)
                    {
                        var mediafolder = (MediaFolder)media;
                        if (mediafolder.Medias != null && mediafolder.Medias.Count > 0)
                        {
                            TrashFiles(mediafolder.Medias, true);
                        }
                    }
                    unitOfWork.Session.Delete(media);
                }
            }
            catch (Exception e)
            {
                // Restore copied files.
                try
                {
                    foreach (var movedFile in movedFilesDic.Keys)
                    {
                        storageService.RemoveObject(movedFile);
                    }
                }
                catch (Exception exception)
                {
                    LogFailedRemoveFiles(movedFilesDic.Keys, exception);
                }
                throw;
            }
            unitOfWork.Commit();
            try
            {
                foreach (var movedFile in movedFilesDic.Values)
                {
                    storageService.RemoveObject(movedFile);
                }
            }
            catch (Exception e)
            {
                LogFailedRemoveFiles(movedFilesDic.Values, e);
            }
        }

        private void LogFailedRemoveFiles(IEnumerable<Uri> movedFiles, Exception exception)
        {
            Log.Error("Failed to remove files: " + string.Join(", ", movedFiles), exception);
            throw exception;
        }

        private string GenerateTrashPublicUrl(Uri fileUri, MediaType mediaType)
        {
            return Path.Combine(
                GetContentPublicRoot("~/trash"),
                GetFolderWithFileName(fileUri, mediaType))
                .Replace('\\', '/');
        }

        private string GetFolderWithFileName(Uri fileUri, MediaType mediaType)
        {
            return Path.Combine(mediaType.ToString().ToLower(), GetElementFromEnd(fileUri.Segments, 1), GetElementFromEnd(fileUri.Segments, 0));
        }

        /// <summary>
        /// Gets element from the end of list.
        /// </summary>
        /// <typeparam name="TSource">Type of list item.</typeparam>
        /// <param name="list">List to get element from.</param>
        /// <param name="elementFromEnd">Element number counted from end of list.</param>
        /// <returns>Element of list.</returns>
        private static TSource GetElementFromEnd<TSource>(IList<TSource> list, int elementFromEnd)
        {
            ++elementFromEnd;
            if (list.Count <= elementFromEnd || elementFromEnd < 0)
            {
                throw new ArgumentOutOfRangeException(string.Format("Failed to get element number {0} from array size of {1}.", elementFromEnd, list.Count));
            }
            return list[list.Count - elementFromEnd];
        }

        public virtual MediaFile UploadFile(MediaType type, Guid rootFolderId, string fileName, long fileLength, Stream fileStream, bool isTemporary = true, 
            string title = "", string description = "")
        {
            string folderName = CreateRandomFolderName();
            MediaFile file = new MediaFile();
            if (!rootFolderId.HasDefaultValue())
            {
                file.Folder = repository.AsProxy<MediaFolder>(rootFolderId);
            }
            file.Title = !string.IsNullOrEmpty(title) ? title : Path.GetFileName(fileName);
            if (!string.IsNullOrEmpty(description))
            {
                file.Description = description;
            }
            file.Type = type;
            file.OriginalFileName = fileName;
            file.OriginalFileExtension = Path.GetExtension(fileName);
            file.Size = fileLength;
            file.FileUri = GetFileUri(type, folderName, RemoveInvalidHtmlSymbols(fileName));
            file.PublicUrl = GetPublicFileUrl(type, folderName, RemoveInvalidHtmlSymbols(fileName));
            file.IsTemporary = isTemporary;
            file.IsCanceled = false;
            file.IsUploaded = null;
            if (configuration.Security.AccessControlEnabled)
            {
                file.AddRule(new AccessRule { AccessLevel = AccessLevel.ReadWrite, Identity = securityService.CurrentPrincipalName });
            }

            unitOfWork.BeginTransaction();
            repository.Save(file);
            unitOfWork.Commit();

            Task fileUploadTask = UploadMediaFileToStorageAsync<MediaFile>(fileStream, file.FileUri, file.Id, (media, session) => { media.IsUploaded = true; }, media => { media.IsUploaded = false; }, false);
            fileUploadTask.ContinueWith(
                task =>
                {
                    try
                    {
                        // During uploading progress Cancel action can by executed. Need to remove uploaded files from the storage.
                        ExecuteActionOnThreadSeparatedSessionWithNoConcurrencyTracking(
                            session =>
                            {
                                var media = session.Get<MediaFile>(file.Id);
                                if (media != null)
                                {
                                    if (media.IsCanceled && media.IsUploaded.HasValue && media.IsUploaded.Value)
                                    {
                                        RemoveFile(media.Id, media.Version);
                                    }
                                }
                            });
                    }
                    catch (Exception ex)
                    {
                        Log.Error("Failed to cancel file upload.", ex);
                    }
                });

            fileUploadTask.ContinueWith((t) => { Log.Error("Error observed while executing parallel task in file uploading.", t.Exception); }, TaskContinuationOptions.OnlyOnFaulted);

            fileUploadTask.Start();

            return file;
        }

        public MediaFile UploadFileWithStream(
            MediaType type,
            Guid rootFolderId,
            string fileName,
            long fileLength,
            Stream fileStream,
            bool waitForUploadResult = false,
            string title = "",
            string description = "",
            Guid? reuploadMediaId = null)
        {
            string folderName = CreateRandomFolderName();
            MediaFile tempFile;
            bool createHistoryItem;

            if (reuploadMediaId.HasValue && !reuploadMediaId.Equals(Guid.Empty))
            {
                var originalEntity = repository.AsQueryable<MediaFile>().FirstOrDefault(f => f.Id == reuploadMediaId);
                if (originalEntity == null)
                {
                    throw new EntityNotFoundException("File with specified ID could not be found");
                }
                tempFile = (MediaFile)originalEntity.Clone();
                createHistoryItem = true;
            }
            else
            {
                tempFile = new MediaFile();

                if (!rootFolderId.HasDefaultValue())
                {
                    tempFile.Folder = repository.AsProxy<MediaFolder>(rootFolderId);
                }
                if (!string.IsNullOrEmpty(description))
                {
                    tempFile.Description = description;
                }
                tempFile.Type = type;
                tempFile.Title = !string.IsNullOrEmpty(title) ? title : Path.GetFileName(fileName);

                if (configuration.Security.AccessControlEnabled)
                {
                    tempFile.AddRule(new AccessRule { AccessLevel = AccessLevel.ReadWrite, Identity = securityService.CurrentPrincipalName });
                }
                createHistoryItem = false;
            }

            tempFile.FileUri = GetFileUri(type, folderName, RemoveInvalidHtmlSymbols(fileName));
            tempFile.PublicUrl = GetPublicFileUrl(type, folderName, RemoveInvalidHtmlSymbols(fileName));

            tempFile.OriginalFileName = fileName;
            tempFile.OriginalFileExtension = Path.GetExtension(fileName);
            tempFile.Size = fileLength;

            tempFile.IsTemporary = true;
            tempFile.IsCanceled = false;
            tempFile.IsUploaded = null;

            unitOfWork.BeginTransaction();
            repository.Save(tempFile);

            if (!waitForUploadResult)
            {
                unitOfWork.Commit();
            }

            if (waitForUploadResult)
            {
                UploadMediaFileToStorageSync(fileStream, tempFile.FileUri, tempFile, media => { media.IsUploaded = true; media.IsTemporary = false; }, media => { media.IsUploaded = false; }, false);

                if (createHistoryItem)
                {
                    SaveOriginalMediaFile(reuploadMediaId.Value, tempFile, unitOfWork.Session);
                }

                repository.Save(tempFile);
                unitOfWork.Commit();
                Events.MediaManagerEvents.Instance.OnMediaFileUpdated(tempFile);
            }
            else
            {
                Action<MediaFile, ISession> action = delegate(MediaFile mediaFile, ISession session)
                {
                    mediaFile.IsUploaded = true;
                    mediaFile.IsTemporary = false;

                    if (createHistoryItem)
                    {
                        SaveOriginalMediaFile(reuploadMediaId.Value, mediaFile, session);
                    }
                };

                Task fileUploadTask = UploadMediaFileToStorageAsync<MediaFile>(fileStream, tempFile.FileUri, tempFile.Id, action, media => { media.IsUploaded = false; }, false);
                fileUploadTask.ContinueWith(
                    task =>
                    {
                        // During uploading progress Cancel action can by executed. Need to remove uploaded files from the storage.
                        ExecuteActionOnThreadSeparatedSessionWithNoConcurrencyTracking(
                            session =>
                            {
                                var media = session.Get<MediaFile>(tempFile.Id);
                                if (media != null)
                                {
                                    if (media.IsCanceled && media.IsUploaded.HasValue && media.IsUploaded.Value)
                                    {
                                        RemoveFile(media.Id, media.Version);
                                    }
                                }
                            });
                    });

                fileUploadTask.Start();
            }

            return tempFile;
        }

        public void SwapOriginalMediaWithVersion(MediaFile originalEntity, MediaFile newVersion, ISession session = null)
        {
            var swapEntity = newVersion.Clone();
            if (session != null)
            {
                session.Evict(swapEntity);
            }

            // swap
            newVersion.CopyDataTo(swapEntity, false);
            originalEntity.CopyDataTo(newVersion, false);
            swapEntity.CopyDataTo(originalEntity, false);

            newVersion.Original = originalEntity;

            originalEntity.Categories = originalEntity.Categories ?? new List<MediaCategory>();
            newVersion.Categories = newVersion.Categories ?? new List<MediaCategory>();

            var catResult = SwapEntityCollections(originalEntity.Categories, newVersion.Categories, originalEntity, newVersion);
            originalEntity.Categories = catResult.Item1;
            newVersion.Categories = catResult.Item2;

            originalEntity.MediaTags = originalEntity.MediaTags ?? new List<MediaTag>();
            newVersion.MediaTags = newVersion.MediaTags ?? new List<MediaTag>();

            var tagResult = SwapEntityCollections(originalEntity.MediaTags, newVersion.MediaTags, originalEntity, newVersion);
            originalEntity.MediaTags = tagResult.Item1;
            newVersion.MediaTags = tagResult.Item2;
        }

        private System.Tuple<IList<TEntity>, IList<TEntity>> SwapEntityCollections<TEntity>(IList<TEntity> collection1, 
            IList<TEntity> collection2, MediaFile entity1, MediaFile entity2)
            where TEntity : IMediaProvider
        {
            var list1 = new List<TEntity>(collection1);
            var list2 = new List<TEntity>(collection2);

            list1.ForEach(e => collection1.Remove(e));
            list2.ForEach(e => collection2.Remove(e));

            collection1 = new List<TEntity>();
            collection2 = new List<TEntity>();

            list2.ForEach(e =>
            {
                e.Media = entity1;
                collection1.Add(e);
            });
            list1.ForEach(
                e =>
                {
                    e.Media = entity2;
                    collection2.Add(e);
                });

            return new System.Tuple<IList<TEntity>, IList<TEntity>>(collection1, collection2);
        }

        private void SaveOriginalMediaFile(Guid originalId, MediaFile mediaFile, ISession session)
        {
            var originalEntity = session.Get<MediaFile>(originalId);
            SwapOriginalMediaWithVersion(originalEntity, mediaFile, session);
            session.SaveOrUpdate(originalEntity);
        }

        public virtual string CreateRandomFolderName()
        {
            return Guid.NewGuid().ToString().Replace("-", string.Empty);
        }

        public virtual Uri GetFileUri(MediaType type, string folderName, string fileName)
        {
            string contentRoot;
            if (configuration.Security.AccessControlEnabled && type == MediaType.File)
            {
                contentRoot = configuration.Storage.SecuredContentRoot;
            }
            else
            {
                contentRoot = configuration.Storage.ContentRoot;
            }
            return new Uri(Path.Combine(GetContentRoot(contentRoot), type.ToString().ToLower(), folderName, fileName));
        }

        public virtual string GetPublicFileUrl(MediaType type, string folderName, string fileName)
        {
            string fullPath = Path.Combine(
                GetContentPublicRoot(configuration.Storage.PublicContentUrlRoot),
                Path.Combine(type.ToString().ToLower(), folderName, fileName));

            string absoluteUri = new Uri(fullPath).AbsoluteUri;

            return HttpUtility.UrlDecode(absoluteUri);
        }

        public void UploadMediaFileToStorageSync<TMedia>(
            Stream sourceStream,
            Uri fileUri,
            TMedia media,
            Action<TMedia> updateMediaAfterUpload,
            Action<TMedia> updateMediaAfterFail,
            bool ignoreAccessControl) where TMedia : MediaFile
        {
            using (var stream = new MemoryStream())
            {
                sourceStream.Seek(0, SeekOrigin.Begin);
                sourceStream.CopyTo(stream);

                try
                {
                    ExecuteFileUpload(fileUri, stream, ignoreAccessControl);
                    updateMediaAfterUpload(media);
                }
                catch (Exception exc)
                {
                    updateMediaAfterFail(media);

                    // Log exception
                    LogManager.GetCurrentClassLogger().Error("Failed to upload file.", exc);
                }
                finally
                {
                    stream.Close();
                    stream.Dispose();
                }
            }
        }

        public Task UploadMediaFileToStorageAsync<TMedia>(Stream sourceStream,
            Uri fileUri,
            Guid mediaId,
            Action<TMedia, ISession> updateMediaAfterUpload,
            Action<TMedia> updateMediaAfterFail,
            bool ignoreAccessControl)
            where TMedia : MediaFile
        {

            var stream = new MemoryStream();

            sourceStream.Seek(0, SeekOrigin.Begin);
            sourceStream.CopyTo(stream);

            Action<ISession> failedResultAction = session =>
            {
                var media = session.Get<TMedia>(mediaId);
                if (media != null)
                {
                    updateMediaAfterFail(media);
                    session.SaveOrUpdate(media);
                    session.Flush();
                    Events.MediaManagerEvents.Instance.OnMediaFileUpdated(media);
                }
            };

            Action<ISession> completedAction = session =>
            {
                var media = session.Get<TMedia>(mediaId);
                if (media != null)
                {
                    updateMediaAfterUpload(media, session);
                    session.SaveOrUpdate(media);
                    session.Flush();
                    Events.MediaManagerEvents.Instance.OnMediaFileUpdated(media);
                }
            };

            Action finalAction = () =>
            {
                stream.Close();
                stream.Dispose();
            };

            var task = new Task(() => ExecuteFileUpload(fileUri, stream, ignoreAccessControl));
            task
             .ContinueWith(
                t =>
                {
                    if (t.Exception == null)
                    {
                        try
                        {
                            ExecuteActionOnThreadSeparatedSessionWithNoConcurrencyTracking(completedAction);
                        }
                        catch (Exception ex)
                        {
                            LogManager.GetCurrentClassLogger().Error("Failed to complete successful file upload.", ex);
                        }
                    }
                    else
                    {
                        // Log exception
                        LogManager.GetCurrentClassLogger().Error("Failed to upload file.", t.Exception.Flatten());

                        try
                        {
                            ExecuteActionOnThreadSeparatedSessionWithNoConcurrencyTracking(failedResultAction);
                        }
                        catch (Exception ex)
                        {
                            LogManager.GetCurrentClassLogger().Error("Failed to complete failed file upload.", ex);
                        }
                    }
                })
             .ContinueWith(t => finalAction());

            return task;
        }

        private void ExecuteFileUpload(Uri fileUri, Stream stream, bool ignoreAccessControl)
        {
            var upload = new UploadRequest();
            upload.CreateDirectory = true;
            upload.Uri = fileUri;
            upload.InputStream = stream;
            upload.IgnoreAccessControl = ignoreAccessControl;

            storageService.UploadObject(upload);
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
        /// Gets the content root.
        /// </summary>
        /// <returns>Root path.</returns>
        private string GetContentRoot(string rootPath)
        {
            if (configuration.Storage.ServiceType == StorageServiceType.FileSystem && VirtualPathUtility.IsAppRelative(rootPath))
            {
                return httpContextAccessor.MapPath(rootPath);
            }

            return rootPath;
        }

        private string GetContentPublicRoot(string rootPath)
        {
            if (configuration.Storage.ServiceType == StorageServiceType.FileSystem && VirtualPathUtility.IsAppRelative(rootPath))
            {
                return httpContextAccessor.MapPublicPath(rootPath);
            }

            return rootPath;
        }

        public string GetDownloadFileUrl(MediaType type, Guid id, string fileUrl)
        {
            if (type == MediaType.Image || !configuration.Security.AccessControlEnabled || !storageService.SecuredUrlsEnabled)
            {
                return fileUrl;
            }

            return mediaFileUrlResolver.GetMediaFileFullUrl(id, fileUrl);
        }

        public void SaveMediaFile(MediaFile file)
        {
            unitOfWork.BeginTransaction();
            repository.Save(file);
            unitOfWork.Commit();
        }

        private static string RemoveInvalidHtmlSymbols(string fileName)
        {
            var invalidFileNameChars = Path.GetInvalidFileNameChars().ToList();
            invalidFileNameChars.AddRange(new[] { '+', ' ' });
            return HttpUtility.UrlEncode(invalidFileNameChars.Aggregate(fileName, (current, invalidFileNameChar) => current.Replace(invalidFileNameChar, '_')));
        }
    }
}