using System;
using System.Runtime.Serialization;

namespace BetterCms.Module.Api.Operations.Blog.BlogPosts.BlogPost.Properties
{
    [DataContract]
    [Serializable]
    public class MetadataModel
    {
        /// <summary>
        /// Gets or sets the page meta title.
        /// </summary>
        /// <value>
        /// The page meta title.
        /// </value>
        [DataMember]
        public string MetaTitle { get; set; }

        /// <summary>
        /// Gets or sets the page meta keywords.
        /// </summary>
        /// <value>
        /// The page meta keywords.
        /// </value>
        [DataMember]
        public string MetaKeywords { get; set; }

        /// <summary>
        /// Gets or sets the page meta description.
        /// </summary>
        /// <value>
        /// The page meta description.
        /// </value>
        [DataMember]
        public string MetaDescription { get; set; }
    }
}