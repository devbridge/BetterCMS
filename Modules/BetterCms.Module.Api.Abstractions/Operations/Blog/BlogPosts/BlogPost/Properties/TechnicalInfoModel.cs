using System.Runtime.Serialization;

namespace BetterCms.Module.Api.Operations.Blog.BlogPosts.BlogPost.Properties
{
    [DataContract]
    [System.Serializable]
    public class TechnicalInfoModel
    {
        /// <summary>
        /// Gets or sets the blog post content identifier.
        /// </summary>
        /// <value>
        /// The blog post content identifier.
        /// </value>
        [DataMember]
        public System.Guid? BlogPostContentId { get; set; }

        /// <summary>
        /// Gets or sets the page content identifier.
        /// </summary>
        /// <value>
        /// The page content identifier.
        /// </value>
        [DataMember]
        public System.Guid? PageContentId { get; set; }

        /// <summary>
        /// Gets or sets the region identifier.
        /// </summary>
        /// <value>
        /// The region identifier.
        /// </value>
        [DataMember]
        public System.Guid? RegionId { get; set; }
    }
}
