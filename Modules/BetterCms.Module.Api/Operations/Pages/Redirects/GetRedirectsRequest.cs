using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Pages.Redirects
{
    [Route("/redirects", Verbs = "GET")]
    public class GetRedirectsRequest : RequestBase<DataOptions>, IReturn<GetRedirectsResponse>
    {
    }
}