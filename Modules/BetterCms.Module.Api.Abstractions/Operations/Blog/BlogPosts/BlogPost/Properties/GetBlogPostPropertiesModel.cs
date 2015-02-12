using System.Runtime.Serialization;

namespace BetterCms.Module.Api.Operations.Blog.BlogPosts.BlogPost.Properties
{
    [DataContract]
    [System.Serializable]
    public class GetBlogPostPropertiesModel
    {
        /// <summary>
        /// Gets or sets a value indicating whether to include HTML content.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to include HTML content; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool IncludeHtmlContent { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to include tags.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to include tags; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool IncludeTags { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to include categories.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to include category; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool IncludeCategories { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to include language.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to include language; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool IncludeLanguage { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to include layout.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to include layout; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool IncludeLayout { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to include author.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to include author; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool IncludeAuthor { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to include image.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to include image; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool IncludeImages { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to include meta data.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to include meta data; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool IncludeMetaData { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to include access rules.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to include access rules; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool IncludeAccessRules { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to include technical information (content, page content, region ids).
        /// </summary>
        /// <value>
        /// <c>true</c> if include technical information (content, page content, region ids); otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool IncludeTechnicalInfo { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to include child contents options.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to include child contents options; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool IncludeChildContentsOptions { get; set; }
    }
}