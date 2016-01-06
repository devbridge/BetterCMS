using System.Linq;

using BetterModules.Core.DataAccess;
using BetterCms.Module.Api.Helpers;
using BetterCms.Module.Api.Infrastructure;
using BetterCms.Module.Api.Operations.MediaManager.Images.Image;
using BetterCms.Module.Api.Operations.Root.Categories.Category;
using BetterCms.Module.MediaManager.Models;
using BetterCms.Module.MediaManager.Services;
using BetterCms.Module.Root.Services;

using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Api.Operations.MediaManager.Images
{
    /// <summary>
    /// Default images service contract implementation for REST.
    /// </summary>
    public class ImagesService : Service, IImagesService
    {
        /// <summary>
        /// The repository.
        /// </summary>
        private readonly IRepository repository;

        /// <summary>
        /// The file URL resolver.
        /// </summary>
        private readonly IMediaFileUrlResolver fileUrlResolver;

        /// <summary>
        /// The image service.
        /// </summary>
        private readonly IImageService imageService;

        private readonly ICategoryService categoryService;
        /// <summary>
        /// Initializes a new instance of the <see cref="ImagesService" /> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="fileUrlResolver">The file URL resolver.</param>
        /// <param name="imageService">The image service.</param>
        public ImagesService(IRepository repository, IMediaFileUrlResolver fileUrlResolver, IImageService imageService, IUploadImageService uploadImageService, ICategoryService categoryService)
        {
            this.repository = repository;
            this.fileUrlResolver = fileUrlResolver;
            this.imageService = imageService;
            this.categoryService = categoryService;
            Upload = uploadImageService;
        }

        /// <summary>
        /// Gets the upload image service.
        /// </summary>
        public IUploadImageService Upload { get; private set; }

        /// <summary>
        /// Gets images list.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>
        ///   <c>GetImagesResponse</c> with images list.
        /// </returns>
        public GetImagesResponse Get(GetImagesRequest request)
        {
            request.Data.SetDefaultOrder("Title");

            var query = repository
                .AsQueryable<Media>()
                .Where(m => m.Original == null && m.Type == Module.MediaManager.Models.MediaType.Image)
                .Where(f => !(f is MediaImage) || (!((MediaImage)f).IsTemporary && ((MediaImage)f).IsUploaded == true));

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

            if (!request.Data.IncludeImages)
            {
                query = query.Where(media => media.ContentType != Module.MediaManager.Models.MediaContentType.File);
            }

            if (!request.Data.IncludeArchived)
            {
                query = query.Where(m => !m.IsArchived);
            }

            query = query.ApplyMediaTagsFilter(request.Data)
                         .ApplyCategoriesFilter(categoryService, request.Data);

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
                            Description = media.Description,
                            Caption = media is MediaImage ? ((MediaImage)media).Caption : null,
                            MediaContentType = media is MediaFolder 
                                                    ? (MediaContentType)((int)MediaContentType.Folder)
                                                    : (MediaContentType)((int)MediaContentType.File),
                            FileExtension = media is MediaImage ? ((MediaImage)media).OriginalFileExtension : null,
                            FileSize = media is MediaImage ? ((MediaImage)media).Size : (long?)null,
                            ImageUrl = media is MediaImage ? ((MediaImage)media).PublicUrl : null,
                            ThumbnailUrl = media is MediaImage ? ((MediaImage)media).PublicThumbnailUrl : null,
                            IsArchived = media.IsArchived
                        })
                        .ToDataListResponse(request);

            foreach (var model in listResponse.Items)
            {
                model.ImageUrl = fileUrlResolver.EnsureFullPathUrl(model.ImageUrl);
                model.ThumbnailUrl = fileUrlResolver.EnsureFullPathUrl(model.ThumbnailUrl);


                if (request.Data.IncludeCategories)
                {
                    model.Categories = (from media in repository.AsQueryable<MediaFile>()
                        from category in media.Categories
                                        where media.Id == model.Id && !category.IsDeleted
                        select
                            new CategoryNodeModel
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
                }
            }

            return new GetImagesResponse
                       {
                           Data = listResponse
                       };
        }

        /// <summary>
        /// Creates a new image.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>
        ///   <c>PostImagesResponse</c> with a new image id.
        /// </returns>
        public PostImageResponse Post(PostImageRequest request)
        {
            var result =
                imageService.Put(
                    new PutImageRequest
                    {
                        Data = request.Data,
                        User = request.User
                    });

            return new PostImageResponse { Data = result.Data };
        }
    }
}