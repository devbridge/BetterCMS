using System.Runtime.Serialization;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Blog.BlogPosts.BlogPost
{
    [Route("/blog-posts/{BlogPostId}", Verbs = "GET")]
    [DataContract]
    [System.Serializable]
    public class GetBlogPostRequest : IReturn<GetBlogPostResponse>
    {
        [DataMember]
        public System.Guid BlogPostId { get; set; }
    }
}