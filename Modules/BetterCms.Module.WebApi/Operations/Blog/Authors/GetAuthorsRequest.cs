using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Blog.Authors
{
    [Route("/authors", Verbs = "GET")]
    public class GetAuthorsRequest : RequestBase, IReturn<GetAuthorsResponse>
    {
    }
}