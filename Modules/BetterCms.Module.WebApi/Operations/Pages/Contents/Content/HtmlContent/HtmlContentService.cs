using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Api.Operations.Pages.Contents.Content.HtmlContent
{
    public class HtmlContentService : Service, IHtmlContentService
    {
        public GetHtmlContentResponse Get(GetHtmlContentRequest request)
        {
            // TODO: need implementation
            return new GetHtmlContentResponse
                       {
                           Data = new HtmlContentModel
                                      {
                                          Id = request.ContentId
                                      }
                       };
        }
    }
}