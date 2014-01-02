using System;
using System.Linq;

using BetterCms.Core.DataAccess;
using BetterCms.Module.Api.Helpers;
using BetterCms.Module.Api.Infrastructure;
using BetterCms.Module.MediaManager.Models;
using BetterCms.Module.MediaManager.Services;

using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Api.Operations.MediaManager.Files
{
    public class FilesService : Service, IFilesService
    {
        private readonly IRepository repository;

        private readonly IMediaFileService fileService;

        private readonly IMediaFileUrlResolver fileUrlResolver;

        public FilesService(IRepository repository, IMediaFileService fileService, IMediaFileUrlResolver fileUrlResolver)
        {
            this.repository = repository;
            this.fileService = fileService;
            this.fileUrlResolver = fileUrlResolver;
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

            listResponse.Items.ToList().ForEach(media =>
                {
                    if (media.MediaContentType == MediaContentType.File)
                    {
                        media.FileUrl = fileService.GetDownloadFileUrl(MediaType.File, media.Id, media.FileUrl);
                    }
                    media.FileUrl = fileUrlResolver.EnsureFullPathUrl(media.FileUrl);
                    media.ThumbnailUrl = fileUrlResolver.EnsureFullPathUrl(media.ThumbnailUrl);
                });

            return new GetFilesResponse
                       {
                           Data = listResponse
                       };
        }
    }
}