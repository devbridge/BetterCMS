using System.Collections.Generic;
using System.Runtime.Serialization;

using BetterCms.Module.WebApi.Models.Enums;

namespace BetterCms.Module.WebApi.Models.Blog.GetBlogPosts
{
    [DataContract]
    public class GetBlogPostsRequest : RequestBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetBlogPostsRequest" /> class.
        /// </summary>
        public GetBlogPostsRequest()
        {
            TagsConnector = FilterConnector.And;
        }

        /// <summary>
        /// Gets or sets a value indicating whether to include unpublished blog posts.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to include unpublished blog posts; otherwise, <c>false</c>.
        /// </value>
        [DataMember(Order = 10, Name = "includeUnpublished")]
        public bool IncludeUnpublished { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to include not active blog posts.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to include not active blog posts; otherwise, <c>false</c>.
        /// </value>
        [DataMember(Order = 20, Name = "includeNotActive")]
        public bool IncludeNotActive { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to include archived blog posts.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to include archived blog posts; otherwise, <c>false</c>.
        /// </value>
        [DataMember(Order = 30, Name = "includeArchived")]
        public bool IncludeArchived { get; set; }

        /// <summary>
        /// Gets or sets the tags.
        /// </summary>
        /// <value>
        /// The tags.
        /// </value>
        [DataMember(Order = 40, Name = "blogPostTags")]
        public List<string> BlogPostTags { get; set; }

        /// <summary>
        /// Gets or sets the tags filter connector.
        /// </summary>
        /// <value>
        /// The tags filter connector.
        /// </value>
        [DataMember(Order = 50, Name = "tagsConnector")]
        public FilterConnector TagsConnector { get; set; }
    }
}