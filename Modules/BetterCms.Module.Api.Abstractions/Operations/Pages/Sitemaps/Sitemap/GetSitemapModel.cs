using System.Runtime.Serialization;

namespace BetterCms.Module.Api.Operations.Pages.Sitemaps.Sitemap
{
    /// <summary>
    /// Model for sitemap getting.
    /// </summary>
    [DataContract]
    public class GetSitemapModel
    {
        /// <summary>
        /// Gets or sets a value indicating whether to include tags.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to include tags; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool IncludeTags { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to include access rules.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to include access rules; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool IncludeAccessRules { get; set; }
    }
}
