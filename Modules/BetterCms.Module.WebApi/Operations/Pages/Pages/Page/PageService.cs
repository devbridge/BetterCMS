using BetterCms.Module.Api.Operations.Pages.Pages.Page.Contents;
using BetterCms.Module.Api.Operations.Pages.Pages.Page.Exists;
using BetterCms.Module.Api.Operations.Pages.Pages.Page.Properties;
using BetterCms.Module.Api.Operations.Pages.Pages.Page.RenderedHtml;

using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Api.Operations.Pages.Pages.Page
{
    public class PageService : Service, IPageService
    {
        private readonly IPagePropertiesService pagePropertiesService;

        private readonly IPageRenderedHtmlService pageHtmlService;

        private readonly IPageExistsService pageExistsService;

        private readonly IPageContentsService pageContentsService;

        public PageService(IPagePropertiesService pagePropertiesService, IPageRenderedHtmlService pageHtmlService,
            IPageExistsService pageExistsService, IPageContentsService pageContentsService)
        {
            this.pageContentsService = pageContentsService;
            this.pagePropertiesService = pagePropertiesService;
            this.pageHtmlService = pageHtmlService;
            this.pageExistsService = pageExistsService;
        }

        public GetPageResponse Get(GetPageRequest request)
        {
            // TODO: need implementation
            return new GetPageResponse
                       {
                           Data = new PageModel
                                      {
                                          Id = request.PageId,
                                          PageUrl = request.PageUrl
                                      }
                       };
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