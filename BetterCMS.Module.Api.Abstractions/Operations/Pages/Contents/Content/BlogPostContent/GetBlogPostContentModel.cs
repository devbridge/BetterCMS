using System.Runtime.Serialization;

namespace BetterCms.Module.Api.Operations.Pages.Contents.Content.BlogPostContent
{
    [DataContract]
    public class GetBlogPostContentModel
    {
        /// <summary>
        /// Gets or sets the content id.
        /// </summary>
        /// <value>
        /// The content id.
        /// </value>
        [DataMember]
        public System.Guid ContentId { get; set; }
    }
}