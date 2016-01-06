using System;
using System.Collections.Generic;
using System.Linq;

using BetterModules.Core.DataAccess;
using BetterCms.Core.Security;
using BetterCms.Module.Api.Helpers;
using BetterCms.Module.Api.Infrastructure;
using BetterCms.Module.Api.Operations.MediaManager.Files.File;
using BetterCms.Module.Api.Operations.Root;
using BetterCms.Module.Api.Operations.Root.Categories.Category;
using BetterCms.Module.MediaManager.Models;
using BetterCms.Module.MediaManager.Services;
using BetterCms.Module.Root.Services;

using ServiceStack.Common.Extensions;
using ServiceStack.ServiceInterface;

using AccessLevel = BetterCms.Module.Api.Operations.Root.AccessLevel;

namespace BetterCms.Module.Api.Operations.MediaManager.Files
{
    public class FilesService : Service, IFilesService
    {
        /// <summary>
        /// The repository.
        /// </summary>
        private readonly IRepository repository;

        /// <summary>
        /// The media file service.
        /// </summary>
        private readonly IMediaFileService mediaFileService;

        /// <summary>
        /// The file URL resolver.
        /// </summary>
        private readonly IMediaFileUrlResolver fileUrlResolver;

        /// <summary>
        /// The access control service.
        /// </summary>
        private readonly IAccessControlService accessControlService;

        /// <summary>
        /// The file service.
        /// </summary>
        private readonly IFileService fileService;

        private readonly ICategoryService categoryService;
        /// <summary>
        /// Initializes a new instance of the <see cref="FilesService"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="mediaFileService">The media file service.</param>
        /// <param name="fileUrlResolver">The file URL resolver.</param>
        /// <param name="accessControlService">The access control service.</param>
        /// <param name="fileService">The file service.</param>
        /// <param name="uploadFileService">The upload file service.</param>
        public FilesService(
            IRepository repository,
            IMediaFileService mediaFileService,
            IMediaFileUrlResolver fileUrlResolver,
            IAccessControlService accessControlService,
            IFileService fileService,
            IUploadFileService uploadFileService,
            ICategoryService categoryService)
        {
            this.repository = repository;
            this.mediaFileService = mediaFileService;
            this.fileUrlResolver = fileUrlResolver;
            this.accessControlService = accessControlService;
            this.fileService = fileService;
            this.categoryService = categoryService;
            Upload = uploadFileService;
        }

        /// <summary>
        /// Gets the upload file service.
        /// </summary>
        public IUploadFileService Upload { get; private set; }

        /// <summary>
        /// Gets files list.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>
        ///   <c>GetFilesResponse</c> with files list.
        /// </returns>
        public GetFilesResponse Get(GetFilesRequest request)
        {
            request.Data.SetDefaultOrder("Title");

            var query =
                repository.AsQueryable<Media>()
                    .Where(m => m.Original == null && m.Type == Module.MediaManager.Models.MediaType.File)
                    .Where(f => !(f is MediaFile) || (!((MediaFile)f).IsTemporary && ((MediaFile)f).IsUploaded == true));

            if (request.Data.FolderId == null)
            {
                query = query.Where(m => m.Folder == null);
            }
            else
            {
                query = query.Where(m => m.Folder.Id == request.Data.FolderId && !m.Folder.IsDeleted);
            }

            if (!request.Data.IncludeFolders)
            {
                query = query.Where(media => media.ContentType != Module.MediaManager.Models.MediaContentType.Folder);
            }

            if (!request.Data.IncludeFiles)
            {
                query = query.Where(media => media.ContentType != Module.MediaManager.Models.MediaContentType.File);
            }

            if (!request.Data.IncludeArchived)
            {
                query = query.Where(m => !m.IsArchived);
            }

            query = query.ApplyMediaTagsFilter(request.Data)
                         .ApplyCategoriesFilter(categoryService, request.Data);

            if (request.User != null && !string.IsNullOrWhiteSpace(request.User.Name))
            {
                var principal = new ApiPrincipal(request.User);
                IEnumerable<Guid> deniedPages = accessControlService.GetPrincipalDeniedObjects<MediaFile>(principal, false);
                foreach (var deniedPageId in deniedPages)
                {
                    var id = deniedPageId;
                    query = query.Where(f => f.Id != id);
                }
            }

            var listResponse =
                query.Select(
                    media =>
                    new MediaModel
                        {
                            Id = media.Id,
                            Version = media.Version,
                            CreatedBy = media.CreatedByUser,
                            CreatedOn = media.CreatedOn,
                            LastModifiedBy = media.ModifiedByUser,
                            LastModifiedOn = media.ModifiedOn,
                            Title = media.Title,
                            Description = media.Description,
                            MediaContentType =
                                media is MediaFolder ? (MediaContentType)((int)MediaContentType.Folder) : (MediaContentType)((int)MediaContentType.File),
                            FileExtension = media is MediaFile ? ((MediaFile)media).OriginalFileExtension : null,
                            FileSize = media is MediaFile ? ((MediaFile)media).Size : (long?)null,
                            FileUrl = media is MediaFile ? ((MediaFile)media).PublicUrl : null,
                            IsArchived = media.IsArchived,
                            ThumbnailId = media.Image != null && !media.Image.IsDeleted ? media.Image.Id : (Guid?)null,
                            ThumbnailCaption = media.Image != null && !media.Image.IsDeleted ? media.Image.Caption : null,
                            ThumbnailUrl = media.Image != null && !media.Image.IsDeleted ? media.Image.PublicThumbnailUrl : null
                        }).ToDataListResponse(request);

            var ids = new List<Guid>();

            listResponse.Items.ToList().ForEach(
                media =>
                    {
                        if (media.MediaContentType == MediaContentType.File)
                        {
                            media.FileUrl = this.mediaFileService.GetDownloadFileUrl(Module.MediaManager.Models.MediaType.File, media.Id, media.FileUrl);
                            ids.Add(media.Id);
                        }
                        media.FileUrl = fileUrlResolver.EnsureFullPathUrl(media.FileUrl);
                        media.ThumbnailUrl = fileUrlResolver.EnsureFullPathUrl(media.ThumbnailUrl);
                    });

            if (request.Data.IncludeAccessRules)
            {
                (from file in repository.AsQueryable<MediaFile>()
                 from accessRule in file.AccessRules
                 where ids.Contains(file.Id)
                 orderby accessRule.IsForRole, accessRule.Identity
                 select
                     new AccessRuleModelEx
                         {
                             AccessRule =
                                 new AccessRuleModel
                                     {
                                         AccessLevel = (AccessLevel)(int)accessRule.AccessLevel,
                                         Identity = accessRule.Identity,
                                         IsForRole = accessRule.IsForRole
                                     },
                             FileId = file.Id
                         }).ToList()
                    .ForEach(
                        rule => listResponse.Items.Where(file => file.Id == rule.FileId).ToList().ForEach(
                            file =>
                                {
                                    if (file.AccessRules == null)
                                    {
                                        file.AccessRules = new List<AccessRuleModel>();
                                    }
                                    file.AccessRules.Add(rule.AccessRule);
                                }));
            }

            if (request.Data.IncludeCategories)
            {
                listResponse.Items.ForEach(
                    item =>
                    {
                        item.Categories = (from media in repository.AsQueryable<MediaFile>()
                                           from category in media.Categories
                                           where media.Id == item.Id && !category.IsDeleted
                                           select new CategoryNodeModel
                                           {
                                               Id = category.Category.Id,
                                               Version = category.Version,
                                               CreatedBy = category.CreatedByUser,
                                               CreatedOn = category.CreatedOn,
                                               LastModifiedBy = category.ModifiedByUser,
                                               LastModifiedOn = category.ModifiedOn,
                                               Name = category.Category.Name,
                                               CategoryTreeId = category.Category.CategoryTree.Id
                                           }).ToList();
                    });
            }

            return new GetFilesResponse { Data = listResponse };
        }

        /// <summary>
        /// Creates a new file.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>
        ///   <c>PostFilesResponse</c> with a new file id.
        /// </returns>
        public PostFileResponse Post(PostFileRequest request)
        {
            var result =
                fileService.Put(
                    new PutFileRequest
                    {
                        Data = request.Data,
                        User = request.User
                    });

            return new PostFileResponse { Data = result.Data };
        }

        private class AccessRuleModelEx
        {
            public AccessRuleModel AccessRule { get; set; }

            public Guid FileId { get; set; }
        }
    }
}