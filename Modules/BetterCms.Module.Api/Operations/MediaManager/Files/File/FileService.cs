using System;
using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.DataAccess;
using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Core.Exceptions.Api;
using BetterCms.Core.Exceptions.DataTier;
using BetterCms.Module.Api.Operations.Root;
using BetterCms.Module.MediaManager.Models;
using BetterCms.Module.MediaManager.Models.Extensions;
using BetterCms.Module.MediaManager.Services;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;

using NHibernate.Linq;

using ServiceStack.ServiceInterface;

using ITagService = BetterCms.Module.Pages.Services.ITagService;

namespace BetterCms.Module.Api.Operations.MediaManager.Files.File
{
    public class FileService : Service, IFileService
    {
        /// <summary>
        /// The repository.
        /// </summary>
        private readonly IRepository repository;

        /// <summary>
        /// The file service.
        /// </summary>
        private readonly IMediaFileService fileService;

        /// <summary>
        /// The file URL resolver.
        /// </summary>
        private readonly IMediaFileUrlResolver fileUrlResolver;

        /// <summary>
        /// The unit of work.
        /// </summary>
        private readonly IUnitOfWork unitOfWork;

        /// <summary>
        /// The tag service.
        /// </summary>
        private readonly ITagService tagService;

        /// <summary>
        /// The media service.
        /// </summary>
        private readonly IMediaService mediaService;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileService" /> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="fileService">The file service.</param>
        /// <param name="fileUrlResolver">The file URL resolver.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="tagService">The tag service.</param>
        /// <param name="mediaService">The media service.</param>
        public FileService(IRepository repository, IMediaFileService fileService, IMediaFileUrlResolver fileUrlResolver, IUnitOfWork unitOfWork, ITagService tagService, IMediaService mediaService)
        {
            this.repository = repository;
            this.fileService = fileService;
            this.fileUrlResolver = fileUrlResolver;
            this.unitOfWork = unitOfWork;
            this.tagService = tagService;
            this.mediaService = mediaService;
        }

        /// <summary>
        /// Gets the specified file.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>
        ///   <c>GetFileRequest</c> with an file.
        /// </returns>
        public GetFileResponse Get(GetFileRequest request)
        {
            var model = repository
                .AsQueryable<MediaFile>()
                .Where(media => media.Id == request.FileId && media.Type == Module.MediaManager.Models.MediaType.File)
                .Select(media => new FileModel
                    {
                        Id = media.Id,
                        Version = media.Version,
                        CreatedBy = media.CreatedByUser,
                        CreatedOn = media.CreatedOn,
                        LastModifiedBy = media.ModifiedByUser,
                        LastModifiedOn = media.ModifiedOn,

                        Title = media.Title,
                        Description = media.Description,
                        FileExtension = media.OriginalFileExtension,
                        FileSize = media.Size,
                        FileUrl = fileUrlResolver.EnsureFullPathUrl(media.PublicUrl),
                        IsArchived = media.IsArchived,
                        FolderId = media.Folder != null && !media.Folder.IsDeleted ? media.Folder.Id : (Guid?)null,
                        FolderName = media.Folder != null && !media.Folder.IsDeleted ? media.Folder.Title : null,
                        PublishedOn = media.PublishedOn,
                        OriginalFileName = media.OriginalFileName,
                        OriginalFileExtension = media.OriginalFileExtension,
                        ThumbnailId = media.Image != null && !media.Image.IsDeleted ? media.Image.Id : (Guid?)null,
                        ThumbnailCaption = media.Image != null && !media.Image.IsDeleted ? media.Image.Caption : null,
                        ThumbnailUrl = media.Image != null && !media.Image.IsDeleted ? media.Image.PublicThumbnailUrl : null,

                        FileUri = media.FileUri.ToString(),
                        IsUploaded = media.IsUploaded,
                        IsTemporary = media.IsTemporary,
                        IsCanceled = media.IsCanceled,
                    })
                .FirstOne();

            model.FileUrl = fileService.GetDownloadFileUrl(Module.MediaManager.Models.MediaType.File, model.Id, model.FileUrl);
            model.ThumbnailUrl = fileUrlResolver.EnsureFullPathUrl(model.ThumbnailUrl);

            IEnumerable<TagModel> tagsFuture;
            if (request.Data.IncludeAccessRules)
            {
                tagsFuture =
                    repository.AsQueryable<MediaTag>(mediaTag => mediaTag.Media.Id == request.FileId && !mediaTag.Tag.IsDeleted)                               
                              .OrderBy(mediaTag => mediaTag.Tag.Name)                             
                              .Select(media => new TagModel
                                    {
                                        Id = media.Tag.Id,
                                        Version = media.Tag.Version,
                                        CreatedBy = media.Tag.CreatedByUser,
                                        CreatedOn = media.Tag.CreatedOn,
                                        LastModifiedBy = media.Tag.ModifiedByUser,
                                        LastModifiedOn = media.Tag.ModifiedOn,

                                        Name = media.Tag.Name
                                    })
                              .ToFuture();
            }
            else
            {
                tagsFuture = null;
            }
            
            IEnumerable<AccessRuleModel> accessRulesFuture;
            if (request.Data.IncludeTags)
            {
                accessRulesFuture = (from file in repository.AsQueryable<MediaFile>()
                    from accessRule in file.AccessRules
                    where file.Id == request.FileId
                    orderby accessRule.IsForRole, accessRule.Identity
                    select
                        new AccessRuleModel
                        {
                            AccessLevel = (AccessLevel)(int)accessRule.AccessLevel, 
                            Identity = accessRule.Identity, 
                            IsForRole = accessRule.IsForRole
                        })
                    .ToList();
            }
            else
            {
                accessRulesFuture = null;
            }

            return new GetFileResponse
                   {
                       Data = model,
                       Tags = tagsFuture != null ? tagsFuture.ToList() : null,
                       AccessRules = accessRulesFuture != null ? accessRulesFuture.ToList() : null
                   };
        }

        /// <summary>
        /// Replaces the file or if it doesn't exist, creates it.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>
        ///   <c>PutFileResponse</c> with a file id.
        /// </returns>
        public PutFileResponse Put(PutFileRequest request)
        {
            IEnumerable<MediaFolder> parentFolderFuture = null;
            if (request.Data.FolderId.HasValue)
            {
                parentFolderFuture = repository.AsQueryable<MediaFolder>()
                    .Where(c => c.Id == request.Data.FolderId.Value && !c.IsDeleted)
                    .ToFuture();
            }

            var mediaFile = repository.AsQueryable<MediaFile>()
                .Where(file => file.Id == request.FileId)
                .ToFuture()
                .FirstOrDefault();

            MediaFolder parentFolder = null;
            if (parentFolderFuture != null)
            {
                parentFolder = parentFolderFuture.First();
                if (parentFolder.Type != Module.MediaManager.Models.MediaType.File)
                {
                    throw new CmsApiValidationException("Folder must be type of an file.");
                }
            }

            var createFile = mediaFile == null;
            if (createFile)
            {
                mediaFile = new MediaFile
                {
                    Id = request.FileId.GetValueOrDefault(),
                    Type = Module.MediaManager.Models.MediaType.File
                };
            }
            else if (request.Data.Version > 0)
            {
                mediaFile.Version = request.Data.Version;
            }

            unitOfWork.BeginTransaction();

            if (!createFile)
            {
                repository.Save(mediaFile.CreateHistoryItem());
            }

            mediaFile.Title = request.Data.Title;
            mediaFile.Description = request.Data.Description;
            mediaFile.Size = request.Data.FileSize;
            mediaFile.PublicUrl = request.Data.PublicUrl;
            mediaFile.IsArchived = request.Data.IsArchived;
            mediaFile.Folder = parentFolder;
            mediaFile.PublishedOn = request.Data.PublishedOn;
            mediaFile.OriginalFileName = request.Data.OriginalFileName;
            mediaFile.OriginalFileExtension = request.Data.OriginalFileExtension;

            mediaFile.FileUri = new Uri(request.Data.FileUri);
            mediaFile.IsUploaded = request.Data.IsUploaded;
            mediaFile.IsTemporary = request.Data.IsTemporary;
            mediaFile.IsCanceled = request.Data.IsCanceled;

            var archivedMedias = new List<Media> { mediaFile };
            var unarchivedMedias = new List<Media> { mediaFile };
            if (request.Data.IsArchived)
            {
                mediaService.ArchiveSubMedias(mediaFile, archivedMedias);
            }
            else
            {
                mediaService.UnarchiveSubMedias(mediaFile, unarchivedMedias);
            }

            repository.Save(mediaFile);

            IList<Tag> newTags = null;
            if (request.Data.Tags != null)
            {
                tagService.SaveMediaTags(mediaFile, request.Data.Tags, out newTags);
            }

            unitOfWork.Commit();

            // Fire events.
            Events.RootEvents.Instance.OnTagCreated(newTags);
            if (createFile)
            {
                Events.MediaManagerEvents.Instance.OnMediaFileUploaded(mediaFile);
            }
            else
            {
                Events.MediaManagerEvents.Instance.OnMediaFileUpdated(mediaFile);
            }

            foreach (var archivedMedia in archivedMedias.Distinct())
            {
                Events.MediaManagerEvents.Instance.OnMediaArchived(archivedMedia);
            }

            foreach (var archivedMedia in unarchivedMedias.Distinct())
            {
                Events.MediaManagerEvents.Instance.OnMediaUnarchived(archivedMedia);
            }

            return new PutFileResponse
            {
                Data = mediaFile.Id
            };
        }

        /// <summary>
        /// Deletes the specified file.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>
        ///   <c>DeleteFileResponse</c> with success status.
        /// </returns>
        public DeleteFileResponse Delete(DeleteFileRequest request)
        {
            if (request.Data == null || request.FileId.HasDefaultValue())
            {
                return new DeleteFileResponse { Data = false };
            }

            var itemToDelete = repository
                .AsQueryable<MediaImage>()
                .Where(p => p.Id == request.FileId)
                .FirstOne();

            if (request.Data.Version > 0 && itemToDelete.Version != request.Data.Version)
            {
                throw new ConcurrentDataException(itemToDelete);
            }

            unitOfWork.BeginTransaction();

            mediaService.DeleteMedia(itemToDelete);

            unitOfWork.Commit();

            Events.MediaManagerEvents.Instance.OnMediaFileDeleted(itemToDelete);

            return new DeleteFileResponse { Data = true };
        }
    }
}