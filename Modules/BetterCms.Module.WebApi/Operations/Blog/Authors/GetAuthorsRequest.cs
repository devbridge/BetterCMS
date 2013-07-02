using System.Runtime.Serialization;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Blog.Authors
{
    [DataContract]
    [Route("/authors", Verbs = "GET")]
    public class GetAuthorsRequest : RequestBase, IReturn<GetAuthorsResponse>
    {
    }
}