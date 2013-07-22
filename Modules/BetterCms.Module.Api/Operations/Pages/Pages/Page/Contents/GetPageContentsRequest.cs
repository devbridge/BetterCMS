using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Pages.Pages.Page.Contents
{
    [Route("/pages/{PageId}/contents")]
    public class GetPageContentsRequest : RequestBase<GetPageContentsModel>, IReturn<GetPageContentsResponse>
    {
    }
}