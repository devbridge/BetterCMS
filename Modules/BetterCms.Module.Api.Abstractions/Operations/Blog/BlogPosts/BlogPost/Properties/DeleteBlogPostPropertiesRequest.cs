using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Blog.BlogPosts.BlogPost.Properties
{
    /// <summary>
    /// Blog post delete request for REST.
    /// </summary>
    [Route("/blog-post-properties/{BlogPostId}", Verbs = "DELETE")]
    [DataContract]
    public class DeleteBlogPostPropertiesRequest : DeleteRequestBase, IReturn<DeleteBlogPostPropertiesResponse>
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        [DataMember]
        public Guid BlogPostId { get; set; }
    }
}