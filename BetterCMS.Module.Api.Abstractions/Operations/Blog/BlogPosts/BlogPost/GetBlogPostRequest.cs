using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Blog.BlogPosts.BlogPost
{
    [Route("/blog-posts/{BlogPostId}", Verbs = "GET")]
    [DataContract]
    public class GetBlogPostRequest : IReturn<GetBlogPostResponse>
    {
        [DataMember]
        public System.Guid BlogPostId { get; set; }
    }
}