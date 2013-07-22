using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Blog.BlogPosts
{
    [Route("/blog-posts", Verbs = "GET")]
    public class GetBlogPostsRequest : RequestBase<GetBlogPostsModel>, IReturn<GetBlogPostsResponse>
    {
    }
}