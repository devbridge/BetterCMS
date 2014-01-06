using System;
using System.Linq;

using BetterCms.Core.DataAccess;
using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Core.DataContracts.Enums;

using BetterCms.Module.Api.Operations.Pages.Pages.Page.Contents;
using BetterCms.Module.Api.Operations.Pages.Pages.Page.Contents.Content;
using BetterCms.Module.Api.Operations.Pages.Pages.Page.Exists;
using BetterCms.Module.Api.Operations.Pages.Pages.Page.Properties;
using BetterCms.Module.Api.Operations.Pages.Pages.Page.Translations;

using BetterCms.Module.MediaManager.Services;
using BetterCms.Module.Pages.Services;
using BetterCms.Module.Root.Mvc.Helpers;

using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Api.Operations.Pages.Pages.Page
{
    public class PageService : Service, IPageService
    {
        private readonly IPagePropertiesService pagePropertiesService;

        private readonly IPageExistsService pageExistsService;

        private readonly IPageContentsService pageContentsService;

        private readonly IPageContentService pageContentService;
        
        private readonly IPageTranslationsService pageTranslationsService;

        private readonly IRepository repository;

        private readonly IUrlService urlService;

        private readonly IMediaFileUrlResolver fileUrlResolver;

        public PageService(IRepository repository, IPagePropertiesService pagePropertiesService, IPageExistsService pageExistsService, 
            IPageContentsService pageContentsService, IPageContentService pageContentService, IUrlService urlService, IMediaFileUrlResolver fileUrlResolver,
            IPageTranslationsService pageTranslationsService)
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

        public GetPageResponse Get(GetPageRequest request)
        {
            var query = repository.AsQueryable<Module.Pages.Models.PageProperties>();
            
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
                        LayoutId = page.Layout != null && !page.Layout.IsDeleted ? page.Layout.Id : Guid.Empty,
                        CategoryId = page.Category != null && !page.Category.IsDeleted ? page.Category.Id : (Guid?)null,
                        CategoryName = page.Category != null && !page.Category.IsDeleted ? page.Category.Name : null,
                        MainImageId = page.Image != null && !page.Image.IsDeleted ? page.Image.Id : (Guid?)null,
                        MainImageUrl = page.Image != null && !page.Image.IsDeleted ? page.Image.PublicUrl : null,
                        MainImageThumbnauilUrl = page.Image != null && !page.Image.IsDeleted ? page.Image.PublicThumbnailUrl : null,
                        MainImageCaption = page.Image != null && !page.Image.IsDeleted ? page.Image.Caption : null,
                        IsArchived = page.IsArchived,
                        IsMasterPage = page.IsMasterPage,
                        LanguageId = page.Language != null ? page.Language.Id : (Guid?)null,
                        LanguageCode = page.Language != null ? page.Language.Code : null,
                        LanguageGroupIdentifier = page.LanguageGroupIdentifier
                    })
                .FirstOne();

            model.MainImageUrl = fileUrlResolver.EnsureFullPathUrl(model.MainImageUrl);
            model.MainImageThumbnauilUrl = fileUrlResolver.EnsureFullPathUrl(model.MainImageThumbnauilUrl);

            return new GetPageResponse { Data = model };
        }

        IPagePropertiesService IPageService.Properties
        {
            get
            {
                return pagePropertiesService;
            }
        }

        PageExistsResponse IPageService.Exists(PageExistsRequest request)
        {
            return pageExistsService.Get(request);
        }

        IPageContentsService IPageService.Contents
        {
            get
            {
                return pageContentsService;
            }
        }

        IPageContentService IPageService.Content
        {
            get
            {
                return pageContentService;
            }
        }

        IPageTranslationsService IPageService.Translations
        {
            get
            {
                return pageTranslationsService;
            }
        }
    }
}