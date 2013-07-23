using System.Runtime.Serialization;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Blog.BlogPosts
{
    [Route("/blog-posts", Verbs = "GET")]
    [DataContract]
    public class GetBlogPostsRequest : RequestBase<GetBlogPostsModel>, IReturn<GetBlogPostsResponse>
    {
    }
}