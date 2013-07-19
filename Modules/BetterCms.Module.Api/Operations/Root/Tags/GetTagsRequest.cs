using System.Runtime.Serialization;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Root.Tags
{
    [Route("/tags", Verbs = "GET, POST")]
    [DataContract]
    public class GetTagsRequest : ListRequestBase, IReturn<GetTagsResponse>
    {
    }
}