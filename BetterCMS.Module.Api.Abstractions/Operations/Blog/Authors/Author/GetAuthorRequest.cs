using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Blog.Authors.Author
{
    [Route("/authors/{AuthorId}", Verbs = "GET")]
    [DataContract]
    public class GetAuthorRequest : RequestBase<GetAuthorModel>, IReturn<GetAuthorResponse>
    {
    }
}