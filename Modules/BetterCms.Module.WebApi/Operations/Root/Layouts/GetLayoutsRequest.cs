using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Root.Layouts
{
    [Route("/layouts", Verbs = "GET")]
    public class GetLayoutsRequest : ListRequestBase, IReturn<GetLayoutsResponse>
    {
    }
}