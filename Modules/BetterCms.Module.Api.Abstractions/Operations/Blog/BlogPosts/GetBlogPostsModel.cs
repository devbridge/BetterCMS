using System.Collections.Generic;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;
using BetterCms.Module.Api.Infrastructure.Enums;

namespace BetterCms.Module.Api.Operations.Blog.BlogPosts
{
    [DataContract]
    public class GetBlogPostsModel : DataOptions, IFilterByTags
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetBlogPostsRequest" /> class.
        /// </summary>
        public GetBlogPostsModel()
        {
            FilterByTagsConnector = FilterConnector.And;
            FilterByTags = new List<string>();
        }

        /// <summary>
        /// Gets or sets a value indicating whether to include unpublished blog posts.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to include unpublished blog posts; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool IncludeUnpublished { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to include archived blog posts.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to include archived blog posts; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool IncludeArchived { get; set; }

        /// <summary>
        /// Gets or sets the tags.
        /// </summary>
        /// <value>
        /// The tags.
        /// </value>
        [DataMember]
        public List<string> FilterByTags { get; set; }

        /// <summary>
        /// Gets or sets the tags filter connector.
        /// </summary>
        /// <value>
        /// The tags filter connector.
        /// </value>
        [DataMember]
        public FilterConnector FilterByTagsConnector { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to include tags collections to results.
        /// </summary>
        /// <value>
        ///   <c>true</c> if  to include tags collections to results; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool IncludeTags { get; set; }
    }
}