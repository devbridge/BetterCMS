using System.Runtime.Serialization;

namespace BetterCms.Module.Api.Operations.Blog.GetBlogPostById
{
    [DataContract]
    public class GetBlogPostByIdRequest : RequestBase
    {
        /// <summary>
        /// Gets or sets the blog post id.
        /// </summary>
        /// <value>
        /// The blog post id.
        /// </value>
        [DataMember(Order = 10, Name = "blogPostId")]
        public System.Guid BlogPostId { get; set; }
    }
}