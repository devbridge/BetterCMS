using System.Runtime.Serialization;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Root.Tags.Tag
{
    [Route("/tag", Verbs = "PUT")]
    [DataContract]
    public class PutTagRequest : TagModel, IReturn<PutTagResponse>
    {
    }
}