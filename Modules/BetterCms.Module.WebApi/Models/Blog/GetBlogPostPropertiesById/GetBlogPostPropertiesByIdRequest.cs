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
        [DataMember(Order = 10, Name = "blogPostId", IsRequired = true)]
        public Guid BlogPostId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to fetch HTML content.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to fetch HTML content; otherwise, <c>false</c>.
        /// </value>
        [DataMember(Order = 20, Name = "fetchHtmlContent")]
        public bool FetchHtmlContent { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to fetch tags.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to fetch tags; otherwise, <c>false</c>.
        /// </value>
        [DataMember(Order = 30, Name = "fetchTags")]
        public bool FetchTags { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to fetch category.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to fetch category; otherwise, <c>false</c>.
        /// </value>
        [DataMember(Order = 40, Name = "fetchCategory")]
        public bool FetchCategory { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to fetch layout.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to fetch layout; otherwise, <c>false</c>.
        /// </value>
        [DataMember(Order = 50, Name = "fetchLayout")]
        public bool FetchLayout { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to fetch author.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to fetch author; otherwise, <c>false</c>.
        /// </value>
        [DataMember(Order = 60, Name = "fetchAuthor")]
        public bool FetchAuthor { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to fetch image.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to fetch image; otherwise, <c>false</c>.
        /// </value>
        [DataMember(Order = 70, Name = "fetchImages")]
        public bool FetchImages { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to fetch meta data.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to fetch meta data; otherwise, <c>false</c>.
        /// </value>
        [DataMember(Order = 80, Name = "fetchMetaData")]
        public bool FetchMetaData { get; set; }
    }
}