using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Pages.Contents.Content.BlogPostContent
{
    [Route("/contents/blog-post/{ContentId}", Verbs = "GET")]
    public class GetBlogPostContentRequest : RequestBase, IReturn<GetBlogPostContentResponse>
    {
        /// <summary>
        /// Gets or sets the content id.
        /// </summary>
        /// <value>
        /// The content id.
        /// </value>
        public System.Guid ContentId { get; set; }
    }
}