using System;
using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.Exceptions.Api;

using BetterCms.Module.Api.Operations.Root.Categories.Category;
using BetterCms.Module.MediaManager.Models;
using BetterCms.Module.MediaManager.Models.Extensions;
using BetterCms.Module.MediaManager.Services;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Services;

using BetterModules.Core.DataAccess;
using BetterModules.Core.DataAccess.DataContext;
using BetterModules.Core.Exceptions.DataTier;

using NHibernate.Linq;

using ServiceStack.ServiceInterface;

using ITagService = BetterCms.Module.Pages.Services.ITagService;

namespace BetterCms.Module.Api.Operations.MediaManager.Images.Image
{
    /// <summary>
    /// Default image CRUD service.
    /// </summary>
    public class ImageService : Service, IImageService
    {
        /// <summary>
        /// The repository.
        /// </summary>
        private readonly IRepository repository;

        /// <summary>
        /// The unit of work.
        /// </summary>
        private readonly IUnitOfWork unitOfWork;

        /// <summary>
        /// The file URL resolver.
        /// </summary>
        private readonly IMediaFileUrlResolver fileUrlResolver;

        /// <summary>
        /// The tag service.
        /// </summary>
        private readonly ITagService tagService;

        /// <summary>
        /// The media service.
        /// </summary>
        private readonly IMediaService mediaService;
        
        
        /// <summary>
        /// The category service.
        /// </summary>
        private readonly ICategoryService categoryService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageService" /> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="fileUrlResolver">The file URL resolver.</param>
        /// <param name="tagService">The tag service.</param>
        /// <param name="mediaService">The media service.</param>
        public ImageService(IRepository repository, IUnitOfWork unitOfWork, IMediaFileUrlResolver fileUrlResolver, ITagService tagService, IMediaService mediaService, ICategoryService categoryService)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
            this.fileUrlResolver = fileUrlResolver;
            this.tagService = tagService;
            this.mediaService = mediaService;
            this.categoryService = categoryService;
        }

        /// <summary>
        /// Gets the specified image.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>
        ///   <c>GetImageRequest</c> with an image.
        /// </returns>
        public GetImageResponse Get(GetImageRequest request)
        {
            var model = repository
                .AsQueryable<MediaImage>(media => media.Id == request.ImageId && media.Type == Module.MediaManager.Models.MediaType.Image)
                .Select(media => new
                    {
                        Model = new ImageModel
                            {
                                Id = media.Id,
                                Version = media.Version,
                                CreatedBy = media.CreatedByUser,
                                CreatedOn = media.CreatedOn,
                                LastModifiedBy = media.ModifiedByUser,
                                LastModifiedOn = media.ModifiedOn,

                                Title = media.Title,
                                Description = media.Description,
                                Caption = media.Caption,
                                FileExtension = media.OriginalFileExtension,
                                FileSize = media.Size,
                                ImageUrl = media.PublicUrl,
                                Width = media.Width,
                                Height = media.Height,
                                ThumbnailUrl = media.PublicThumbnailUrl,
                                ThumbnailWidth = media.ThumbnailWidth,
                                ThumbnailHeight = media.ThumbnailHeight,
                                ThumbnailSize = media.ThumbnailSize,
                                IsArchived = media.IsArchived,
                                FolderId = media.Folder != null && !media.Folder.IsDeleted ? media.Folder.Id : (Guid?)null,
                                FolderName = media.Folder != null && !media.Folder.IsDeleted ? media.Folder.Title : null,
                                PublishedOn = media.PublishedOn,
                                OriginalFileName = media.OriginalFileName,
                                OriginalFileExtension = media.OriginalFileExtension,
                                OriginalWidth = media.OriginalWidth,
                                OriginalHeight = media.OriginalHeight,
                                OriginalSize = media.OriginalSize,
                                OriginalUrl = media.PublicOriginallUrl,

                                IsUploaded = media.IsUploaded,
                                IsTemporary = media.IsTemporary,
                                IsCanceled = media.IsCanceled
                            },

                        FileUri = media.FileUri,
                        OriginalUri = media.OriginalUri,
                        ThumbnailUri = media.ThumbnailUri
                    })
                .FirstOne();


            model.Model.FileUri = model.FileUri.ToString();
            model.Model.ThumbnailUri = model.ThumbnailUri.ToString();
            model.Model.OriginalUri = model.OriginalUri.ToString();

            model.Model.ImageUrl = fileUrlResolver.EnsureFullPathUrl(model.Model.ImageUrl);
            model.Model.ThumbnailUrl = fileUrlResolver.EnsureFullPathUrl(model.Model.ThumbnailUrl);
            model.Model.OriginalUrl = fileUrlResolver.EnsureFullPathUrl(model.Model.OriginalUrl);

            IList<TagModel> tags;
            if (request.Data.IncludeTags)
            {
                tags =
                    repository.AsQueryable<MediaTag>(mediaTag => mediaTag.Media.Id == request.ImageId && !mediaTag.Tag.IsDeleted)
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


            if (request.Data.IncludeCategories)
            {
                model.Model.Categories = (from media in repository.AsQueryable<MediaFile>()
                                          from category in media.Categories
                                          where media.Id == model.Model.Id && !category.IsDeleted
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
            }

            return new GetImageResponse
                       {
                           Data = model.Model,
                           Tags = tags
                       };
        }

        /// <summary>
        /// Replaces the image or if it doesn't exist, creates it.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>
        ///   <c>PutImageResponse</c> with a image id.
        /// </returns>
        public PutImageResponse Put(PutImageRequest request)
        {
            IEnumerable<MediaFolder> parentFolderFuture = null;
            if (request.Data.FolderId.HasValue)
            {
                parentFolderFuture = repository.AsQueryable<MediaFolder>()
                    .Where(c => c.Id == request.Data.FolderId.Value && !c.IsDeleted)
                    .ToFuture();
            }

            var mediaImage = repository.AsQueryable<MediaImage>()
                .Where(file => file.Id == request.Id)
                .ToFuture()
                .FirstOrDefault();

            MediaFolder parentFolder = null;
            if (parentFolderFuture != null)
            {
                parentFolder = parentFolderFuture.First();
                if (parentFolder.Type != Module.MediaManager.Models.MediaType.Image)
                {
                    throw new CmsApiValidationException("Folder must be type of an image.");
                }
            }

            var createImage = mediaImage == null;
            if (createImage)
            {
                mediaImage = new MediaImage
                                 {
                                     Id = request.Id.GetValueOrDefault(),
                                     Type = Module.MediaManager.Models.MediaType.Image
                                 };
            }
            else if (request.Data.Version > 0)
            {
                mediaImage.Version = request.Data.Version;
            }

            unitOfWork.BeginTransaction();

            if (!createImage)
            {
                repository.Save(mediaImage.CreateHistoryItem());
            }

            mediaImage.Title = request.Data.Title;
            mediaImage.Description = request.Data.Description;
            mediaImage.Caption = request.Data.Caption;
            mediaImage.Size = request.Data.FileSize;
            mediaImage.PublicUrl = request.Data.ImageUrl;
            mediaImage.Width = request.Data.Width;
            mediaImage.Height = request.Data.Height;
            mediaImage.PublicThumbnailUrl = request.Data.ThumbnailUrl;
            mediaImage.ThumbnailWidth = request.Data.ThumbnailWidth;
            mediaImage.ThumbnailHeight = request.Data.ThumbnailHeight;
            mediaImage.ThumbnailSize = request.Data.ThumbnailSize;
            mediaImage.Folder = parentFolder;
            mediaImage.PublishedOn = request.Data.PublishedOn;
            mediaImage.OriginalFileName = request.Data.OriginalFileName;
            mediaImage.OriginalFileExtension = request.Data.OriginalFileExtension;
            mediaImage.OriginalWidth = request.Data.OriginalWidth;
            mediaImage.OriginalHeight = request.Data.OriginalHeight;
            mediaImage.OriginalSize = request.Data.OriginalSize;
            mediaImage.PublicOriginallUrl = request.Data.OriginalUrl;

            mediaImage.FileUri = new Uri(request.Data.FileUri);
            mediaImage.IsUploaded = request.Data.IsUploaded;
            mediaImage.IsTemporary = request.Data.IsTemporary;
            mediaImage.IsCanceled = request.Data.IsCanceled;
            mediaImage.OriginalUri = new Uri(request.Data.OriginalUri);
            mediaImage.ThumbnailUri = new Uri(request.Data.ThumbnailUri);

            var archivedMedias = new List<Media>();
            var unarchivedMedias = new List<Media>();
            if (mediaImage.IsArchived != request.Data.IsArchived)
            {
                if (request.Data.IsArchived)
                {
                    archivedMedias.Add(mediaImage);
                    mediaService.ArchiveSubMedias(mediaImage, archivedMedias);
                }
                else
                {
                    unarchivedMedias.Add(mediaImage);
                    mediaService.UnarchiveSubMedias(mediaImage, unarchivedMedias);
                }
            }

            mediaImage.IsArchived = request.Data.IsArchived;

            if (request.Data.Categories != null)
            {
                categoryService.CombineEntityCategories<Media, MediaCategory>(mediaImage, request.Data.Categories);
            }

            repository.Save(mediaImage);

            IList<Tag> newTags = null;
            if (request.Data.Tags != null)
            {
                tagService.SaveMediaTags(mediaImage, request.Data.Tags, out newTags);
            }

            unitOfWork.Commit();

            // Fire events.
            Events.RootEvents.Instance.OnTagCreated(newTags);
            if (createImage)
            {
                Events.MediaManagerEvents.Instance.OnMediaFileUploaded(mediaImage);
            }
            else
            {
                Events.MediaManagerEvents.Instance.OnMediaFileUpdated(mediaImage);
            }

            foreach (var archivedMedia in archivedMedias.Distinct())
            {
                Events.MediaManagerEvents.Instance.OnMediaArchived(archivedMedia);
            }

            foreach (var archivedMedia in unarchivedMedias.Distinct())
            {
                Events.MediaManagerEvents.Instance.OnMediaUnarchived(archivedMedia);
            }

            return new PutImageResponse
                       {
                           Data = mediaImage.Id
                       };
        }

        /// <summary>
        /// Deletes the specified image.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>
        ///   <c>DeleteImageResponse</c> with success status.
        /// </returns>
        public DeleteImageResponse Delete(DeleteImageRequest request)
        {
            if (request.Data == null || request.Id.HasDefaultValue())
            {
                return new DeleteImageResponse { Data = false };
            }

            var itemToDelete = repository
                .AsQueryable<MediaImage>()
                .Where(p => p.Id == request.Id)
                .FirstOne();

            if (request.Data.Version > 0 && itemToDelete.Version != request.Data.Version)
            {
                throw new ConcurrentDataException(itemToDelete);
            }

            unitOfWork.BeginTransaction();

            mediaService.DeleteMedia(itemToDelete);

            unitOfWork.Commit();

            Events.MediaManagerEvents.Instance.OnMediaFileDeleted(itemToDelete);

            return new DeleteImageResponse { Data = true };
        }
    }
}