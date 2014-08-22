using System.Runtime.Serialization;

namespace BetterCms.Module.Api.Operations.Pages.Pages
{
    [DataContract]
    [System.Serializable]
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

        /// <summary>
        /// Gets or sets a value indicating whether to use no follow.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to use no follow; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool UseNoFollow { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to use no index.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to use no follow; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool UseNoIndex { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to use canonical URL.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to use canonical URL; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool UseCanonicalUrl { get; set; }
    }
}
