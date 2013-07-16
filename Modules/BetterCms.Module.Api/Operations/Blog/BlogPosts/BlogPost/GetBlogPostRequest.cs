using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Blog.BlogPosts.BlogPost
{
    [Route("/blog-posts/{BlogPostId}", Verbs = "GET")]
    public class GetBlogPostRequest : RequestBase, IReturn<GetBlogPostResponse>
    {
        /// <summary>
        /// Gets or sets the blog post id.
        /// </summary>
        /// <value>
        /// The blog post id.
        /// </value>
        public System.Guid BlogPostId { get; set; }
    }
}