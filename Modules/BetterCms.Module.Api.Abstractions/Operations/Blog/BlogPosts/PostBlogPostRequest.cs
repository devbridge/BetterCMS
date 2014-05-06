using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;
using BetterCms.Module.Api.Operations.Blog.BlogPosts.BlogPost;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Blog.BlogPosts
{
    /// <summary>
    /// Request for blog post creation.
    /// </summary>
    [Route("/blog-posts", Verbs = "POST")]
    [DataContract]
    public class PostBlogPostRequest : RequestBase<SaveBlogPostModel>, IReturn<PostBlogPostResponse>
    {
    }
}
