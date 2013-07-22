using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Blog.BlogPosts.BlogPost.Properties
{
    [Route("/blog-post-properties/{BlogPostId}", Verbs = "GET")]
    public class GetBlogPostPropertiesRequest : RequestBase<GetBlogPostPropertiesModel>, IReturn<GetBlogPostPropertiesResponse>
    {
    }
}