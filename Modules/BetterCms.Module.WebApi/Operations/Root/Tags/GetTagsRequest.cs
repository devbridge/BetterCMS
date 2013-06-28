using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Root.Tags
{
    [Route("/tags", Verbs = "GET")]
    public class GetTagsRequest : ListRequestBase, IReturn<GetTagsResponse>
    {
    }
}