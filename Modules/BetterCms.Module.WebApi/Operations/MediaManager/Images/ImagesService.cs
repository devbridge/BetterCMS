using System.Linq;

using BetterCms.Core.DataAccess;
using BetterCms.Module.Api.Helpers;
using BetterCms.Module.MediaManager.Models;

using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Api.Operations.MediaManager.Images
{
    public class ImagesService : Service, IImagesService
    {
        private readonly IRepository repository;

        public ImagesService(IRepository repository)
        {
            this.repository = repository;
        }

        public GetImagesResponse Get(GetImagesRequest request)
        {
            // TODO: throw new validation exception if request.IncludeImages == false && request.IncludeFolders == false

            request.SetDefaultOrder("Title");

            var query = repository
                .AsQueryable<Media>()
                .Where(m => m.Original == null && m.Folder.Id == request.FolderId);

            if (!request.IncludeArchived)
            {
                query = query.Where(m => !m.IsArchived);
            }

            if (request.IncludeFolders && request.IncludeImages)
            {
                query = query.Where(media => (media is MediaImage || media is MediaFolder));
            }
            else if (!request.IncludeFolders)
            {
                query = query.Where(media => media is MediaImage);
            }
            else
            {
                query = query.Where(media => media is MediaFolder);
            }

            // TODO: filter by tags !!!

            var listResponse = query.Select(media =>
                    new MediaModel
                        {
                            Id = media.Id,
                            Version = media.Version,
                            CreatedBy = media.CreatedByUser,
                            CreatedOn = media.CreatedOn,
                            LastModifiedBy = media.ModifiedByUser,
                            LastModifiedOn = media.ModifiedOn,

                            Title = media.Title,
                            Caption = media is MediaImage ? ((MediaImage)media).Caption : null,
                            MediaContentType = media is MediaFolder ? MediaContentType.Folder : MediaContentType.File,
                            FileExtension = media is MediaImage ? ((MediaImage)media).OriginalFileExtension : null,
                            FileSize = media is MediaImage ? ((MediaImage)media).Size : (long?)null,
                            ImageUrl = media is MediaImage ? ((MediaImage)media).PublicUrl : null,
                            ThumbnailUrl = media is MediaImage ? ((MediaImage)media).PublicThumbnailUrl : null

                        }).ToDataListResponse(request);

            return new GetImagesResponse
                       {
                           Data = listResponse
                       };
        }
    }
}