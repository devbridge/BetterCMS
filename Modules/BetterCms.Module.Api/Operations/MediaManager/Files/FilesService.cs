using System;
using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.DataAccess;
using BetterCms.Core.Security;
using BetterCms.Module.Api.Helpers;
using BetterCms.Module.Api.Infrastructure;
using BetterCms.Module.Api.Operations.Root;
using BetterCms.Module.MediaManager.Models;
using BetterCms.Module.MediaManager.Services;

using ServiceStack.ServiceInterface;

using AccessLevel = BetterCms.Module.Api.Operations.Root.AccessLevel;

namespace BetterCms.Module.Api.Operations.MediaManager.Files
{
    public class FilesService : Service, IFilesService
    {
        private readonly IRepository repository;

        private readonly IMediaFileService fileService;

        private readonly IMediaFileUrlResolver fileUrlResolver;
        
        private readonly IAccessControlService accessControlService;

        public FilesService(IRepository repository, IMediaFileService fileService,
            IMediaFileUrlResolver fileUrlResolver, IAccessControlService accessControlService)
        {
            this.repository = repository;
            this.fileService = fileService;
            this.fileUrlResolver = fileUrlResolver;
            this.accessControlService = accessControlService;
        }

        public GetFilesResponse Get(GetFilesRequest request)
        {
            request.Data.SetDefaultOrder("Title");

            var query = repository.AsQueryable<Media>()
                .Where(m => m.Original == null && m.Type == MediaType.File)
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

            query = query.ApplyMediaTagsFilter(request.Data);

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

            var listResponse = query.Select(media => new MediaModel
                        {
                            Id = media.Id,
                            Version = media.Version,
                            CreatedBy = media.CreatedByUser,
                            CreatedOn = media.CreatedOn,
                            LastModifiedBy = media.ModifiedByUser,
                            LastModifiedOn = media.ModifiedOn,

                            Title = media.Title,
                            MediaContentType = media is MediaFolder 
                                                    ? (MediaContentType)((int)MediaContentType.Folder) 
                                                    : (MediaContentType)((int)MediaContentType.File),
                            FileExtension = media is MediaFile ? ((MediaFile)media).OriginalFileExtension : null,
                            FileSize = media is MediaFile ? ((MediaFile)media).Size : (long?)null,
                            FileUrl = media is MediaFile ? ((MediaFile)media).PublicUrl : null,
                            IsArchived = media.IsArchived,
                            ThumbnailId = media.Image != null && !media.Image.IsDeleted ? media.Image.Id : (Guid?)null,
                            ThumbnailCaption = media.Image != null && !media.Image.IsDeleted ? media.Image.Caption : null,
                            ThumbnailUrl = media.Image != null && !media.Image.IsDeleted ? media.Image.PublicThumbnailUrl : null
                        })
                        .ToDataListResponse(request);

            var ids = new List<Guid>();

            listResponse.Items.ToList().ForEach(media =>
                {
                    if (media.MediaContentType == MediaContentType.File)
                    {
                        media.FileUrl = fileService.GetDownloadFileUrl(MediaType.File, media.Id, media.FileUrl);
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
                select new AccessRuleModelEx
                {
                    AccessRule = new AccessRuleModel
                    {
                        AccessLevel = (AccessLevel)(int)accessRule.AccessLevel,
                        Identity = accessRule.Identity,
                        IsForRole = accessRule.IsForRole
                    },
                    FileId = file.Id
                }).ToList()
                .ForEach(
                    rule => listResponse
                            .Items
                            .Where(file => file.Id == rule.FileId)
                            .ToList()
                            .ForEach(
                                file =>
                                {
                                    if (file.AccessRules == null)
                                    {
                                        file.AccessRules = new List<AccessRuleModel>();
                                    }
                                    file.AccessRules.Add(rule.AccessRule);
                                })
                    );
            }

            return new GetFilesResponse
                       {
                           Data = listResponse
                       };
        }

        private class AccessRuleModelEx
        {
            public AccessRuleModel AccessRule { get; set; }

            public Guid FileId { get; set; }
        }
    }
}