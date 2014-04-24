using System.Runtime.Serialization;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Root.Tags
{
    [Route("/tags", Verbs = "POST")]
    [DataContract]
    public class PostTagsRequest : TagModel, IReturn<PostTagsResponse>
    {
    }
}