using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.DataAccess;
using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Module.MediaManager.Models;

using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Api.Operations.MediaManager.Files.File
{
    public class FileService : Service, IFileService
    {
        private readonly IRepository repository;

        public FileService(IRepository repository)
        {
            this.repository = repository;
        }

        public GetFileResponse Get(GetFileRequest request)
        {
            var model = repository
                .AsQueryable<MediaFile>(media => media.Id == request.Data.FileId && media.Type == MediaType.File)
                .Select(media => new FileModel
                    {
                        Id = media.Id,
                        Version = media.Version,
                        CreatedBy = media.CreatedByUser,
                        CreatedOn = media.CreatedOn,
                        LastModifiedBy = media.ModifiedByUser,
                        LastModifiedOn = media.ModifiedOn,

                        Title = media.Title,
                        FileExtension = media.OriginalFileExtension,
                        FileSize = media.Size,
                        FileUrl = media.PublicUrl,
                        IsArchived = media.IsArchived,
                        FolderId = media.Folder.Id,
                        FolderName = media.Folder.Title,
                        PublishedOn = media.PublishedOn,
                        OriginalFileName = media.OriginalFileName,
                        OriginalFileExtension = media.OriginalFileExtension,
                        ThumbnailCaption = media.Image.Caption,
                        ThumbnailUrl = media.Image.PublicThumbnailUrl,
                        ThumbnailId = media.Image.Id
                    })
                .FirstOne();

            IList<TagModel> tags;
            if (request.Data.IncludeTags)
            {
                tags =
                    repository.AsQueryable<MediaTag>(mediaTag => mediaTag.Media.Id == request.Data.FileId)
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