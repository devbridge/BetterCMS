using System.Linq;

using BetterCms.Core.DataAccess;
using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Core.DataContracts.Enums;

using BetterCms.Module.Api.Operations.Pages.Pages.Page.Contents;
using BetterCms.Module.Api.Operations.Pages.Pages.Page.Exists;
using BetterCms.Module.Api.Operations.Pages.Pages.Page.Properties;
using BetterCms.Module.Api.Operations.Pages.Pages.Page.RenderedHtml;
using BetterCms.Module.Pages.Services;

using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Api.Operations.Pages.Pages.Page
{
    public class PageService : Service, IPageService
    {
        private readonly IPagePropertiesService pagePropertiesService;

        private readonly IPageRenderedHtmlService pageHtmlService;

        private readonly IPageExistsService pageExistsService;

        private readonly IPageContentsService pageContentsService;

        private readonly IRepository repository;

        private readonly IUrlService urlService;

        public PageService(IRepository repository, IPagePropertiesService pagePropertiesService, IPageRenderedHtmlService pageHtmlService,
            IPageExistsService pageExistsService, IPageContentsService pageContentsService, IUrlService urlService)
        {
            this.pageContentsService = pageContentsService;
            this.pagePropertiesService = pagePropertiesService;
            this.pageHtmlService = pageHtmlService;
            this.pageExistsService = pageExistsService;
            this.repository = repository;
            this.urlService = urlService;
        }

        public GetPageResponse Get(GetPageRequest request)
        {
            // TODO: validate request - one and only one of these can be specified: PageUrl / PageId

            var query = repository.AsQueryable<Module.Pages.Models.PageProperties>();
            
            if (request.PageId.HasValue)
            {
                query = query.Where(page => page.Id == request.PageId.Value);
            }
            else
            {
                var url = urlService.FixUrl(request.PageUrl);
                query = query.Where(page => page.PageUrl == url);
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
                        LayoutId = page.Layout.Id,
                        CategoryId = page.Category.Id,
                        CategoryName = page.Category.Name,
                        MainImageId = page.Image.Id,
                        MainImageUrl = page.Image.PublicUrl,
                        MainImageThumbnauilUrl = page.Image.PublicThumbnailUrl,
                        MainImageCaption = page.Image.Caption,
                        IsArchived = page.IsArchived
                    })
                .FirstOne();

            return new GetPageResponse { Data = model };
        }

        IPagePropertiesService IPageService.Properties
        {
            get
            {
                return pagePropertiesService;
            }
        }

        IPageRenderedHtmlService IPageService.Html
        {
            get
            {
                return pageHtmlService;
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
    }
}