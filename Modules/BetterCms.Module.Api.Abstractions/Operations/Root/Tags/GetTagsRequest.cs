using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Root.Tags
{
    [Route("/tags", Verbs = "GET, POST")]
    [DataContract]
    public class GetTagsRequest : RequestBase<DataOptions>, IReturn<GetTagsResponse>
    {
    }
}