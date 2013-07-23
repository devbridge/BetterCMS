using System.Runtime.Serialization;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Root.Tags
{
    [Route("/tags", Verbs = "GET, POST")]
    [DataContract]
    public class GetTagsRequest : RequestBase<DataOptions>, IReturn<GetTagsResponse>
    {
    }
}