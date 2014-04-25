using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Root.Tags
{
    /// <summary>
    /// Request for tag update or creation.
    /// </summary>
    [Route("/tags", Verbs = "POST")]
    [DataContract]
    public class PostTagsRequest : RequestBase<TagModel>, IReturn<PostTagsResponse>
    {
    }
}