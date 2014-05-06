using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Root.Tags
{
    /// <summary>
    /// Request for tag creation.
    /// </summary>
    [Route("/tags", Verbs = "POST")]
    [DataContract]
    public class PostTagRequest : RequestBase<TagModel>, IReturn<PostTagResponse>
    {
    }
}