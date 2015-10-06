using System.Runtime.Serialization;

namespace BetterCms.Module.Api.Operations.Blog.BlogPosts.BlogPost
{
    [DataContract]
    [System.Serializable]
    public class GetBlogPostRequest
    {
        [DataMember]
        public System.Guid BlogPostId { get; set; }
    }
}