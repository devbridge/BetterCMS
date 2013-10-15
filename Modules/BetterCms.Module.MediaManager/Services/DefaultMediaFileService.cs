using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

using BetterCms.Configuration;
using BetterCms.Core;
using BetterCms.Core.DataAccess;
using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Core.Exceptions;
using BetterCms.Core.Security;
using BetterCms.Core.Services;
using BetterCms.Core.Services.Storage;
using BetterCms.Core.Web;
using BetterCms.Module.MediaManager.Controllers;
using BetterCms.Module.MediaManager.Models;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;

using Common.Logging;

using NHibernate;

namespace BetterCms.Module.MediaManager.Services
{
    internal class DefaultMediaFileService : IMediaFileService
    {
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
            var file = repository.AsQueryable<MediaFile>()
                          .Where(f => f.Id == fileId)
                          .Select(f => new
                                           {
                                               IsUploaded = f.IsUploaded,
                                               FileUri = f.FileUri
                                           })
                          .FirstOrDefault();

            if (file == null)
            {
                throw new CmsException(string.Format("A file was not found by given id={0}", fileId));
            }

            try
            {
                if (file.IsUploaded.HasValue && file.IsUploaded.Value)
                {
                    Task removeFile = 
                            new Task(() =>
                            {
                                storageService.RemoveObject(file.FileUri);
                            })
                            .ContinueWith(task =>
                            {
                                storageService.RemoveFolder(file.FileUri);
                            });

                    removeFile.Start();
                }
            }
            finally
            {
                if (doNotCheckVersion)
                {
                    var media = repository.AsQueryable<MediaFile>().FirstOrDefault(f => f.Id == fileId);
                    repository.Delete(media);
                }
                else
                {
                    repository.Delete<MediaFile>(fileId, version);
                }
                unitOfWork.Commit();   
            }
        }

        public virtual MediaFile UploadFile(MediaType type, Guid rootFolderId, string fileName, long fileLength, Stream fileStream)
        {
            string folderName = CreateRandomFolderName();
            MediaFile file = new MediaFile();
            if (!rootFolderId.HasDefaultValue())
            {
                file.Folder = repository.AsProxy<MediaFolder>(rootFolderId);
            }
            file.Title = Path.GetFileName(fileName);
            file.Type = type;
            file.OriginalFileName = fileName;
            file.OriginalFileExtension = Path.GetExtension(fileName);           
            file.Size = fileLength;
            file.FileUri = GetFileUri(type, folderName, fileName);
            file.PublicUrl = GetPublicFileUrl(type, folderName, fileName);
            file.IsTemporary = true;
            file.IsCanceled = false;
            file.IsUploaded = null;
            if (configuration.Security.AccessControlEnabled)
            {
                file.AddRule(new AccessRule() { AccessLevel = AccessLevel.ReadWrite, Identity = securityService.CurrentPrincipalName });
            }

            unitOfWork.BeginTransaction();
            repository.Save(file);
            unitOfWork.Commit();

            Task fileUploadTask = UploadMediaFileToStorage<MediaFile>(fileStream, file.FileUri, file.Id, media => { media.IsUploaded = true; }, media => { media.IsUploaded = false; }, false);
            fileUploadTask.ContinueWith(
                task =>
                    {
                        // During uploading progress Cancel action can by executed. Need to remove uploaded files from the storage.
                        ExecuteActionOnThreadSeparatedSessionWithNoConcurrencyTracking(
                            session =>
                                {
                                    var media = session.Get<MediaFile>(file.Id);
                                    if (media.IsCanceled && media.IsUploaded.HasValue && media.IsUploaded.Value)
                                    {
                                        RemoveFile(media.Id, media.Version);
                                    }
                                });
                    });

            fileUploadTask.Start();

            return file;
        }

        public virtual string CreateRandomFolderName()
        {
            return Guid.NewGuid().ToString().Replace("-", string.Empty);
        }

        public virtual Uri GetFileUri(MediaType type, string folderName, string fileName)
        {
            return new Uri(Path.Combine(GetContentRoot(configuration.Storage.ContentRoot), type.ToString().ToLower(), folderName, fileName));
        }

        public virtual string GetPublicFileUrl(MediaType type, string folderName, string fileName)
        {
            string fullPath = Path.Combine(
                GetContentPublicRoot(configuration.Storage.PublicContentUrlRoot),
                Path.Combine(type.ToString().ToLower(), folderName, fileName));

            string absoluteUri = new Uri(fullPath).AbsoluteUri;

            return HttpUtility.UrlDecode(absoluteUri);
        }

        /// <summary>
        /// Creates a task to upload a file to the storage.
        /// </summary>
        /// <typeparam name="TMedia">The type of the media.</typeparam>
        /// <param name="sourceStream">The source stream.</param>
        /// <param name="fileUri">The file URI.</param>
        /// <param name="mediaId">The media id.</param>
        /// <param name="updateMediaAfterUpload">An action to update a specific field for the media after file upload.</param>
        /// <param name="updateMediaAfterFail">An action to update a specific field for the media after file upload fails.</param>
        /// <param name="ignoreAccessControl">if set to <c>true</c> ignore access control.</param>
        /// <returns>
        /// Upload file task.
        /// </returns>
        public Task UploadMediaFileToStorage<TMedia>(Stream sourceStream, Uri fileUri, Guid mediaId, Action<TMedia> updateMediaAfterUpload, Action<TMedia> updateMediaAfterFail, bool ignoreAccessControl) where TMedia : MediaFile
        {
            var stream = new MemoryStream();

            sourceStream.Seek(0, SeekOrigin.Begin);
            sourceStream.CopyTo(stream);

            var task = new Task(
                () =>
                {
                    var upload = new UploadRequest();
                    upload.CreateDirectory = true;
                    upload.Uri = fileUri;
                    upload.InputStream = stream;
                    upload.IgnoreAccessControl = ignoreAccessControl;

                    storageService.UploadObject(upload);
                });

            task
             .ContinueWith(
                t =>
                {
                    if (t.Exception == null)
                    {
                        ExecuteActionOnThreadSeparatedSessionWithNoConcurrencyTracking(session =>
                        {
                            var media = session.Get<TMedia>(mediaId);
                            updateMediaAfterUpload(media);
                            session.SaveOrUpdate(media);
                            session.Flush();
                        });
                    }
                    else
                    {
                        ExecuteActionOnThreadSeparatedSessionWithNoConcurrencyTracking(session =>
                        {
                            var media = session.Get<TMedia>(mediaId);
                            updateMediaAfterFail(media);
                            session.SaveOrUpdate(media);
                            session.Flush();
                        });

                        // Log exception
                        LogManager.GetCurrentClassLogger().Error("Failed to upload file.", t.Exception.Flatten());
                    }
                })
             .ContinueWith(
                t =>
                {
                    stream.Close();
                    stream.Dispose();
                });

            return task;
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
    }
}