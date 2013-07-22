using System.Runtime.Serialization;

namespace BetterCms.Module.Api.Operations.Blog.BlogPosts.BlogPost
{
    [DataContract]
    public class GetBlogPostModel
    {
        /// <summary>
        /// Gets or sets the blog post id.
        /// </summary>
        /// <value>
        /// The blog post id.
        /// </value>
        [DataMember]
        public System.Guid BlogPostId { get; set; }        
    }
}