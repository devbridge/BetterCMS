using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Root.Tags.Tag
{
    [Route("/tags/{TagId}", Verbs = "GET")]
    [Route("/tags/by-name/{TagName}", Verbs = "GET")]
    [DataContract]
    public class GetTagRequest : RequestBase<GetTagModel>, IReturn<GetTagResponse>
    {
    }
}