using System;
using System.Runtime.Serialization;

namespace BetterCms.Module.WebApi.Models.Blog.GetBlogPostPropertiesById
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
        [DataMember(Order = 510, Name = "blogPostId", IsRequired = true)]
        public Guid BlogPostId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to fetch tags.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to fetch tags; otherwise, <c>false</c>.
        /// </value>
        [DataMember(Order = 520, Name = "fetchTags")]
        public bool FetchTags { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to fetch category.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to fetch category; otherwise, <c>false</c>.
        /// </value>
        [DataMember(Order = 530, Name = "fetchCategory")]
        public bool FetchCategory { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to fetch metadata.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to fetch metadata; otherwise, <c>false</c>.
        /// </value>
        [DataMember(Order = 540, Name = "fetchMetadata")]
        public bool FetchMetadata { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to fetch layout.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to fetch layout; otherwise, <c>false</c>.
        /// </value>
        [DataMember(Order = 550, Name = "fetchLayout")]
        public bool FetchLayout { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to fetch author.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to fetch author; otherwise, <c>false</c>.
        /// </value>
        [DataMember(Order = 560, Name = "fetchAuthor")]
        public bool FetchAuthor { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to fetch images.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to fetch images; otherwise, <c>false</c>.
        /// </value>
        [DataMember(Order = 570, Name = "fetchImages")]
        public bool FetchImages { get; set; }
    }
}