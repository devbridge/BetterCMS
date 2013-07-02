using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Api.Operations.Pages.Pages.Page.RenderedHtml
{
    public class PageRenderedHtmlService : Service, IPageRenderedHtmlService
    {
        public GetPageRenderedHtmlResponse Get(GetPageRenderedHtmlRequest request)
        {
            // TODO: need implementation
            return new GetPageRenderedHtmlResponse
            {
                Data = "TODO: IMPLEMENT PAGE HTML RENDERING"
            };
        }
    }
}