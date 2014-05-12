using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Blog.BlogPosts.BlogPost.Properties
{
    /// <summary>
    /// Request for blog post update.
    /// </summary>
    [Route("/blog-post-properties/{BlogPostId}", Verbs = "PUT")]
    [DataContract]
    public class PutBlogPostPropertiesRequest : RequestBase<SaveBlogPostPropertiesModel>, IReturn<PutBlogPostPropertiesResponse>
    {
        /// <summary>
        /// Gets or sets the blog post identifier.
        /// </summary>
        /// <value>
        /// The blog post identifier.
        /// </value>
        [DataMember]
        public System.Guid? BlogPostId { get; set; }
    }
}
