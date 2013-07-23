using BetterCms.Module.Pages.Services;

using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Api.Operations.Pages.Pages.Page.RenderedHtml
{
    public class PageRenderedHtmlService : Service, IPageRenderedHtmlService
    {
        private readonly IUrlService urlService;

        public PageRenderedHtmlService(IUrlService urlService)
        {
            this.urlService = urlService;
        }

        public GetPageRenderedHtmlResponse Get(GetPageRenderedHtmlRequest request)
        {
            // TODO: validate request - one and only one of these can be specified: PageUrl / PageId
            string data = null;
            if (request.Data.PageId.HasValue)
            {
                data = "TODO: implement rendering by page ID: " + request.Data.PageId.Value.ToString();
            }
            else
            {
                var url = urlService.FixUrl(request.Data.PageUrl);

                data = "TODO: implement rendering by page URL: " + url;
            }

            return new GetPageRenderedHtmlResponse
            {
                Data = data
            };
        }
    }
}