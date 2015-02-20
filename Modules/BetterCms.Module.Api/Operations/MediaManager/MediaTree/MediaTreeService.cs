using System;
using System.Collections.Generic;
using System.Linq;

using BetterModules.Core.DataAccess;
using BetterCms.Core.Security;
using BetterCms.Module.Api.Infrastructure;
using BetterCms.Module.Api.Operations.Root;
using BetterCms.Module.MediaManager.Models;
using BetterCms.Module.MediaManager.Services;

using ServiceStack.ServiceInterface;

using AccessLevel = BetterCms.Module.Api.Operations.Root.AccessLevel;

namespace BetterCms.Module.Api.Operations.MediaManager.MediaTree
{
    public class MediaTreeService : Service, IMediaTreeService
    {
        private readonly IRepository repository;
        
        private readonly IMediaFileService fileService;

        private readonly IMediaFileUrlResolver fileUrlResolver;
        
        private readonly IAccessControlService accessControlService;

        public MediaTreeService(IRepository repository, IMediaFileService fileService,
            IMediaFileUrlResolver fileUrlResolver, IAccessControlService accessControlService)
        {
            this.repository = repository;
            this.fileService = fileService;
            this.fileUrlResolver = fileUrlResolver;
            this.accessControlService = accessControlService;
        }

        public GetMediaTreeResponse Get(GetMediaTreeRequest request)
        {
            var response = new GetMediaTreeResponse
                               {
                                   Data = new MediaTreeModel()
                               };

            if (request.Data.IncludeFilesTree)
            {
                IEnumerable<Guid> deniedPages = null;
                if (request.User != null && !string.IsNullOrWhiteSpace(request.User.Name))
                {
                    var principal = new ApiPrincipal(request.User);
                    deniedPages = accessControlService.GetPrincipalDeniedObjects<MediaFile>(principal, false);
                }

                response.Data.FilesTree = LoadMediaTree<MediaFile>(Module.MediaManager.Models.MediaType.File, deniedPages, request.Data.IncludeArchived, request.Data.IncludeFiles, request.Data.IncludeAccessRules);
            }
            if (request.Data.IncludeImagesTree)
            {
                response.Data.ImagesTree = LoadMediaTree<MediaImage>(Module.MediaManager.Models.MediaType.Image, null, request.Data.IncludeArchived, request.Data.IncludeImages, false);
            }

            return response;
        }

        private IList<MediaItemModel> LoadMediaTree<TEntity>(Module.MediaManager.Models.MediaType mediaType, IEnumerable<Guid> deniedPages,
            bool includeArchived, bool loadFiles, bool includeAccessRules)
            where TEntity: Media
        {
            var query = repository
                    .AsQueryable<Media>()
                    .OrderBy(m => m.Title)
                    .Where(f => f.Type == mediaType && f.Original == null);

            if (mediaType == Module.MediaManager.Models.MediaType.Image)
            {
                query = query.Where(f => !(f is MediaImage) || (!((MediaImage)f).IsTemporary && ((MediaImage)f).IsUploaded == true));
            }
            else
            {
                query = query.Where(f => !(f is MediaFile) || (!((MediaFile)f).IsTemporary && ((MediaFile)f).IsUploaded == true));
            }

            if (!includeArchived)
            {
                query = query.Where(f => !f.IsArchived);
            }

            if (loadFiles)
            {
                query = query.Where(f => (f is MediaFolder || f is TEntity));
            }
            else
            {
                query = query.Where(f => f is MediaFolder);
            }

            if (deniedPages != null)
            {
                foreach (var deniedPageId in deniedPages)
                {
                    var id = deniedPageId;
                    query = query.Where(f => f.Id != id);
                }
            }

            var mediaItems = query
                .Select(media => new MediaItemModel
                                 {
                                     Id = media.Id,
                                     Version = media.Version,
                                     CreatedBy = media.CreatedByUser,
                                     CreatedOn = media.CreatedOn,
                                     LastModifiedBy = media.ModifiedByUser,
                                     LastModifiedOn = media.ModifiedOn,

                                     ParentFolderId = media.Folder != null && !media.Folder.IsDeleted ? media.Folder.Id : (Guid?)null,
                                     Title = media.Title,
                                     Description = media.Description,
                                     MediaContentType = media is MediaFolder 
                                                            ? (MediaContentType)((int)MediaContentType.Folder) 
                                                            : (MediaContentType)((int)MediaContentType.File),
                                     Url = (media is MediaFile || media is MediaImage) ? fileUrlResolver.EnsureFullPathUrl(((MediaFile)media).PublicUrl) : null,
                                     IsArchived = media.IsArchived
                                 }).ToList();

            var ids = new List<Guid>();
            mediaItems.ForEach(media =>
                                   {
                                       if (media.MediaContentType == MediaContentType.File)
                                       {
                                           media.Url = fileService.GetDownloadFileUrl(mediaType, media.Id, media.Url);
                                           ids.Add(media.Id);
                                       }
                                   });

            if (includeAccessRules)
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
                            })
                        .ToList()
                        .ForEach(rule => mediaItems
                            .Where(file => file.Id == rule.FileId)
                            .ToList()
                            .ForEach(file =>
                                {
                                    if (file.AccessRules == null)
                                    {
                                        file.AccessRules = new List<AccessRuleModel>();
                                    }
                                    file.AccessRules.Add(rule.AccessRule);
                                }));
            }

            return GetChildren(mediaItems, null);
        }

        private List<MediaItemModel> GetChildren(List<MediaItemModel> allItems, Guid? parentId)
        {
            var childItems = allItems.Where(item => item.ParentFolderId == parentId && item.Id != parentId).ToList();

            foreach (var item in childItems)
            {
                item.Children = GetChildren(allItems, item.Id);
            }

            return childItems;
        }

        private class AccessRuleModelEx
        {
            public AccessRuleModel AccessRule { get; set; }

            public Guid FileId { get; set; }
        }
    }
}