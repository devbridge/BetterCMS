using System;
using System.Linq;

using BetterModules.Core.DataAccess;
using BetterModules.Core.DataAccess.DataContext;
using BetterCms.Core.DataContracts.Enums;
using BetterCms.Core.Security;

using BetterCms.Module.Api.Operations.Pages.Pages.Page.Contents;
using BetterCms.Module.Api.Operations.Pages.Pages.Page.Contents.Content;
using BetterCms.Module.Api.Operations.Pages.Pages.Page.Exists;
using BetterCms.Module.Api.Operations.Pages.Pages.Page.Properties;
using BetterCms.Module.Api.Operations.Pages.Pages.Page.Translations;

using BetterCms.Module.MediaManager.Services;
using BetterCms.Module.Pages.Models;
using BetterCms.Module.Pages.Services;
using BetterCms.Module.Root.Mvc.Helpers;

using ServiceStack.ServiceInterface;

using ITagService = BetterCms.Module.Pages.Services.ITagService;

namespace BetterCms.Module.Api.Operations.Pages.Pages.Page
{
    /// <summary>
    /// Default page service for pages API.
    /// </summary>
    public class PageService : Service, IPageService
    {
        /// <summary>
        /// The page properties service.
        /// </summary>
        private readonly IPagePropertiesService pagePropertiesService;

        /// <summary>
        /// The page exists service.
        /// </summary>
        private readonly IPageExistsService pageExistsService;

        /// <summary>
        /// The page contents service.
        /// </summary>
        private readonly IPageContentsService pageContentsService;

        /// <summary>
        /// The page content service.
        /// </summary>
        private readonly IPageContentService pageContentService;

        /// <summary>
        /// The page translations service.
        /// </summary>
        private readonly IPageTranslationsService pageTranslationsService;

        /// <summary>
        /// The repository.
        /// </summary>
        private readonly IRepository repository;

        /// <summary>
        /// The URL service.
        /// </summary>
        private readonly IUrlService urlService;

        /// <summary>
        /// The file URL resolver.
        /// </summary>
        private readonly IMediaFileUrlResolver fileUrlResolver;

        /// <summary>
        /// Initializes a new instance of the <see cref="PageService" /> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="pagePropertiesService">The page properties service.</param>
        /// <param name="pageExistsService">The page exists service.</param>
        /// <param name="pageContentsService">The page contents service.</param>
        /// <param name="pageContentService">The page content service.</param>
        /// <param name="urlService">The URL service.</param>
        /// <param name="fileUrlResolver">The file URL resolver.</param>
        /// <param name="pageTranslationsService">The page translations service.</param>
        /// <param name="masterPageService">The master page service.</param>
        /// <param name="pageService">The page service.</param>
        /// <param name="sitemapService">The sitemap service.</param>
        /// <param name="tagService">The tag service.</param>
        /// <param name="accessControlService">The access control service.</param>
        public PageService(
            IRepository repository,
            IUnitOfWork unitOfWork,
            IPagePropertiesService pagePropertiesService,
            IPageExistsService pageExistsService,
            IPageContentsService pageContentsService,
            IPageContentService pageContentService,
            IUrlService urlService,
            IMediaFileUrlResolver fileUrlResolver,
            IPageTranslationsService pageTranslationsService,
            IMasterPageService masterPageService,
            Module.Pages.Services.IPageService pageService,
            ISitemapService sitemapService,
            ITagService tagService,
            IAccessControlService accessControlService)
        {
            this.pageContentsService = pageContentsService;
            this.pageContentService = pageContentService;
            this.pagePropertiesService = pagePropertiesService;
            this.pageExistsService = pageExistsService;
            this.repository = repository;
            this.urlService = urlService;
            this.fileUrlResolver = fileUrlResolver;
            this.pageTranslationsService = pageTranslationsService;
        }

        /// <summary>
        /// Gets the properties.
        /// </summary>
        /// <value>
        /// The properties.
        /// </value>
        IPagePropertiesService IPageService.Properties
        {
            get
            {
                return pagePropertiesService;
            }
        }

        /// <summary>
        /// Gets the contents.
        /// </summary>
        /// <value>
        /// The contents.
        /// </value>
        IPageContentsService IPageService.Contents
        {
            get
            {
                return pageContentsService;
            }
        }

        /// <summary>
        /// Gets the content.
        /// </summary>
        /// <value>
        /// The content.
        /// </value>
        IPageContentService IPageService.Content
        {
            get
            {
                return pageContentService;
            }
        }

        /// <summary>
        /// Gets the translations.
        /// </summary>
        /// <value>
        /// The translations.
        /// </value>
        IPageTranslationsService IPageService.Translations
        {
            get
            {
                return pageTranslationsService;
            }
        }

        /// <summary>
        /// Gets the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns><c>GetPageResponse</c> with page properties.</returns>
        public GetPageResponse Get(GetPageRequest request)
        {
            var query = repository.AsQueryable<PageProperties>();

            if (request.PageId.HasValue)
            {
                query = query.Where(page => page.Id == request.PageId.Value);
            }
            else
            {
                var url = urlService.FixUrl(request.PageUrl);
                query = query.Where(page => page.PageUrlHash == url.UrlHash());
            }

            var model = query
                .Select(page => new PageModel
                    {
                        Id = page.Id,
                        Version = page.Version,
                        CreatedBy = page.CreatedByUser,
                        CreatedOn = page.CreatedOn,
                        LastModifiedBy = page.ModifiedByUser,
                        LastModifiedOn = page.ModifiedOn,

                        PageUrl = page.PageUrl,
                        Title = page.Title,
                        Description = page.Description,
                        IsPublished = page.Status == PageStatus.Published,
                        PublishedOn = page.PublishedOn,
                        LayoutId = page.Layout != null && !page.Layout.IsDeleted ? page.Layout.Id : (Guid?)null,
                        MasterPageId = page.MasterPage != null && !page.MasterPage.IsDeleted ? page.MasterPage.Id : (Guid?)null,
                        MainImageId = page.Image != null && !page.Image.IsDeleted ? page.Image.Id : (Guid?)null,
                        MainImageUrl = page.Image != null && !page.Image.IsDeleted ? page.Image.PublicUrl : null,
                        MainImageThumbnauilUrl = page.Image != null && !page.Image.IsDeleted ? page.Image.PublicThumbnailUrl : null,
                        MainImageThumbnailUrl = page.Image != null && !page.Image.IsDeleted ? page.Image.PublicThumbnailUrl : null,
                        MainImageCaption = page.Image != null && !page.Image.IsDeleted ? page.Image.Caption : null,
                        FeaturedImageId = page.FeaturedImage != null && !page.FeaturedImage.IsDeleted ? page.FeaturedImage.Id : (Guid?)null,
                        FeaturedImageUrl = page.FeaturedImage != null && !page.FeaturedImage.IsDeleted ? page.FeaturedImage.PublicUrl : null,
                        FeaturedImageThumbnailUrl = page.FeaturedImage != null && !page.FeaturedImage.IsDeleted ? page.FeaturedImage.PublicThumbnailUrl : null,
                        FeaturedImageCaption = page.FeaturedImage != null && !page.FeaturedImage.IsDeleted ? page.FeaturedImage.Caption : null,
                        SecondaryImageId = page.SecondaryImage != null && !page.SecondaryImage.IsDeleted ? page.SecondaryImage.Id : (Guid?)null,
                        SecondaryImageUrl = page.SecondaryImage != null && !page.SecondaryImage.IsDeleted ? page.SecondaryImage.PublicUrl : null,
                        SecondaryImageThumbnailUrl = page.SecondaryImage != null && !page.SecondaryImage.IsDeleted ? page.SecondaryImage.PublicThumbnailUrl : null,
                        SecondaryImageCaption = page.SecondaryImage != null && !page.SecondaryImage.IsDeleted ? page.SecondaryImage.Caption : null,
                        IsArchived = page.IsArchived,
                        IsMasterPage = page.IsMasterPage,
                        LanguageId = page.Language != null ? page.Language.Id : (Guid?)null,
                        LanguageCode = page.Language != null ? page.Language.Code : null,
                        LanguageGroupIdentifier = page.LanguageGroupIdentifier
                    })
                .FirstOne();

            model.MainImageUrl = fileUrlResolver.EnsureFullPathUrl(model.MainImageUrl);
            model.MainImageThumbnauilUrl = fileUrlResolver.EnsureFullPathUrl(model.MainImageThumbnauilUrl);
            model.MainImageThumbnailUrl = fileUrlResolver.EnsureFullPathUrl(model.MainImageThumbnailUrl);

            model.FeaturedImageUrl = fileUrlResolver.EnsureFullPathUrl(model.FeaturedImageUrl);
            model.FeaturedImageThumbnailUrl = fileUrlResolver.EnsureFullPathUrl(model.FeaturedImageThumbnailUrl);

            model.SecondaryImageUrl = fileUrlResolver.EnsureFullPathUrl(model.SecondaryImageUrl);
            model.SecondaryImageThumbnailUrl = fileUrlResolver.EnsureFullPathUrl(model.SecondaryImageThumbnailUrl);


            model.Categories = (from pagePr in repository.AsQueryable<PageProperties>()
                                from category in pagePr.Categories
                                where pagePr.Id == model.Id
                                select new CategoryModel
                                {
                                    Id = category.Category.Id,
                                    Version = category.Category.Version,
                                    CreatedBy = category.Category.CreatedByUser,
                                    CreatedOn = category.Category.CreatedOn,
                                    LastModifiedBy = category.Category.ModifiedByUser,
                                    LastModifiedOn = category.Category.ModifiedOn,
                                    Name = category.Category.Name
                                }).ToList();

            return new GetPageResponse { Data = model };
        }

        /// <summary>
        /// Exists the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns><c>PageExistsResponse</c> with page data.</returns>
        PageExistsResponse IPageService.Exists(PageExistsRequest request)
        {
            return pageExistsService.Get(request);
        }
    }
}