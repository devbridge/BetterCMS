using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

using BetterCms.Configuration;
using BetterCms.Core.DataAccess;
using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Core.Exceptions;
using BetterCms.Core.Services.Storage;
using BetterCms.Core.Web;
using BetterCms.Module.MediaManager.Models;
using BetterCms.Module.Root.Mvc;

using NHibernate;

namespace BetterCms.Module.MediaManager.Services
{
    public class DefaultMediaFileService : IMediaFileService
    {
        private readonly IStorageService storageService;
        
        private readonly IRepository repository;

        private readonly IUnitOfWork unitOfWork;

        private readonly ICmsConfiguration configuration;

        private readonly IHttpContextAccessor httpContextAccessor;

        private ISessionFactoryProvider sessionFactoryProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultMediaFileService" /> class.
        /// </summary>
        /// <param name="storageService">The storage service.</param>
        /// <param name="repository">The repository.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="configuration">The configuration.</param>
        /// <param name="httpContextAccessor">The HTTP context accessor.</param>
        /// <param name="sessionFactoryProvider">The session factory provider.</param>
        public DefaultMediaFileService(IStorageService storageService, IRepository repository, IUnitOfWork unitOfWork,
            ICmsConfiguration configuration, IHttpContextAccessor httpContextAccessor, ISessionFactoryProvider sessionFactoryProvider)
        {
            this.sessionFactoryProvider = sessionFactoryProvider;
            this.httpContextAccessor = httpContextAccessor;
            this.configuration = configuration;
            this.unitOfWork = unitOfWork;
            this.storageService = storageService;
            this.repository = repository;
        }

        public virtual void RemoveFile(Guid fileId, int version)
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
                if (file.IsUploaded)
                {
                    Task removeFile = 
                            new Task(() =>
                            {
                                storageService.RemoveObject(file.FileUri);
                            })
                            .ContinueWith(task =>
                            {
                                storageService.RemoveObjectBucket(file.FileUri);
                            });

                    removeFile.Start();
                }
            }
            finally
            {
                repository.Delete<MediaFile>(fileId, version);
                unitOfWork.Commit();   
            }
        }

        public virtual string GetFileSizeText(long sizeInBytes)
        {
            string[] sizes = { "bytes", "KB", "MB", "GB" };
            double fileSize = sizeInBytes;
            int order = 0;
            while (fileSize >= 1024 && order + 1 < sizes.Length)
            {
                order++;
                fileSize = fileSize / 1024;
            }

            return string.Format("{0:0.##} {1}", fileSize, sizes[order]);
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
            file.IsUploaded = false;

            unitOfWork.BeginTransaction();
            repository.Save(file);
            unitOfWork.Commit();

            Task fileUploadTask = UploadMediaFileToStorage<MediaFile>(fileStream, file.FileUri, file.Id, media => { media.IsUploaded = true; });
            fileUploadTask.ContinueWith(
                task =>
                    {
                        // During uploading progress Cancel action can by executed. Need to remove uploaded files from the storage.
                        ExecuteActionOnThreadSeparatedSessionWithNoConcurrencyTracking(
                            session =>
                                {
                                    var media = session.Get<MediaFile>(file.Id);
                                    if (media.IsCanceled && media.IsUploaded)
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
                GetContentRoot(configuration.Storage.PublicContentUrlRoot),
                Path.Combine(type.ToString().ToLower(), folderName, fileName));

            return new Uri(fullPath).AbsoluteUri;
        }

        /// <summary>
        /// Creates a task to upload a file to the storage.
        /// </summary>
        /// <param name="sourceStream">The source stream.</param>
        /// <param name="fileUri">The file URI.</param>
        /// <param name="mediaId">The media id.</param>
        /// <param name="updateMediaAfterUpload">An action to update a specific field for the media after image upload.</param>
        /// <returns>
        /// Upload file task.
        /// </returns>
        public Task UploadMediaFileToStorage<TMedia>(Stream sourceStream, Uri fileUri, Guid mediaId, Action<TMedia> updateMediaAfterUpload) where TMedia : MediaFile
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

                    storageService.UploadObject(upload);
                });

            task
             .ContinueWith(
                t =>
                {
                    stream.Close();
                    stream.Dispose();
                })
             .ContinueWith(
                t =>
                {
                    ExecuteActionOnThreadSeparatedSessionWithNoConcurrencyTracking(session =>
                    {
                        var media = session.Get<TMedia>(mediaId);
                        updateMediaAfterUpload(media);
                        session.SaveOrUpdate(media);
                        session.Flush();
                    });
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
    }
}