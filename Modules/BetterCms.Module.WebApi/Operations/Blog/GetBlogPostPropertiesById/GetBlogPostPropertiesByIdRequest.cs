using System;
using System.Runtime.Serialization;

namespace BetterCms.Module.Api.Operations.Blog.GetBlogPostPropertiesById
{
    [DataContract]
    public class GetBlogPostPropertiesByIdRequest : RequestBase
    {
        /// <summary>
        /// Gets or sets the blog post id.
        /// </summary>
        /// <value>
        /// The blog post id.
        /// </value>
        [DataMember(Order = 10, Name = "blogPostId", IsRequired = true)]
        public Guid BlogPostId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to include HTML content.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to include HTML content; otherwise, <c>false</c>.
        /// </value>
        [DataMember(Order = 20, Name = "includeHtmlContent")]
        public bool IncludeHtmlContent { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to include tags.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to include tags; otherwise, <c>false</c>.
        /// </value>
        [DataMember(Order = 30, Name = "includeTags")]
        public bool IncludeTags { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to include category.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to include category; otherwise, <c>false</c>.
        /// </value>
        [DataMember(Order = 40, Name = "includeCategory")]
        public bool IncludeCategory { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to include layout.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to include layout; otherwise, <c>false</c>.
        /// </value>
        [DataMember(Order = 50, Name = "includeLayout")]
        public bool IncludeLayout { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to include author.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to include author; otherwise, <c>false</c>.
        /// </value>
        [DataMember(Order = 60, Name = "includeAuthor")]
        public bool IncludeAuthor { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to include image.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to include image; otherwise, <c>false</c>.
        /// </value>
        [DataMember(Order = 70, Name = "includeImages")]
        public bool IncludeImages { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to include meta data.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to include meta data; otherwise, <c>false</c>.
        /// </value>
        [DataMember(Order = 80, Name = "includeMetaData")]
        public bool IncludeMetaData { get; set; }
    }
}