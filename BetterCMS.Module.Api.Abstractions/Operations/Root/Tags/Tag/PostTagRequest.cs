using System.Runtime.Serialization;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Root.Tags.Tag
{
    [Route("/tag", Verbs = "POST")]
    [DataContract]
    public class PostTagRequest : TagModel, IReturn<PostTagResponse>
    {
    }
}