using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Blog.BlogPosts.BlogPost
{
    /// <summary>
    /// Request for blog post update.
    /// </summary>
    [Route("/blog-posts/{BlogPostId}", Verbs = "PUT")]
    [DataContract]
    public class PutBlogPostRequest : PutRequestBase<SaveBlogPostModel>, IReturn<PutBlogPostResponse>
    {
        /// <summary>
        /// Gets or sets the blog post identifier.
        /// </summary>
        /// <value>
        /// The blog post identifier.
        /// </value>
        [DataMember]
        public System.Guid? BlogPostId
        {
            get
            {
                return Data.Id;
            }

            set
            {
                Data.Id = value.HasValue ? value.Value : System.Guid.Empty;
            }
        }
    }
}
