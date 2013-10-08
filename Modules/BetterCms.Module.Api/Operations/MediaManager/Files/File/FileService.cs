using System;
using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.DataAccess;
using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Module.MediaManager.Models;
using BetterCms.Module.MediaManager.Services;

using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Api.Operations.MediaManager.Files.File
{
    public class FileService : Service, IFileService
    {
        private readonly IRepository repository;

        private readonly IMediaFileService fileService;

        private readonly IMediaFileUrlResolver fileUrlResolver;

        public FileService(IRepository repository, IMediaFileService fileService, IMediaFileUrlResolver fileUrlResolver)
        {
            this.repository = repository;
            this.fileService = fileService;
            this.fileUrlResolver = fileUrlResolver;
        }

        public GetFileResponse Get(GetFileRequest request)
        {
            var model = repository
                .AsQueryable<MediaFile>()
                .Where(media => media.Id == request.FileId && media.Type == MediaType.File)
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
                        ThumbnailUrl = media.Image != null && !media.Image.IsDeleted ? media.Image.PublicThumbnailUrl : null
                    })
                .FirstOne();

            model.FileUrl = fileService.GetDownloadFileUrl(MediaType.File, model.Id, model.FileUrl);
            model.ThumbnailUrl = fileUrlResolver.EnsureFullPathUrl(model.ThumbnailUrl);

            IList<TagModel> tags;
            if (request.Data.IncludeTags)
            {
                tags =
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
                              .ToList();
            }
            else
            {
                tags = null;
            }

            return new GetFileResponse
                       {
                           Data = model,
                           Tags = tags
                       };
        }
    }
}