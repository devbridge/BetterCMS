using System;
using System.Runtime.Serialization;

namespace BetterCms.Module.Api.Operations.Pages.Sitemaps.Sitemap.Nodes.Node
{
    /// <summary>
    /// Model for sitemap node getting.
    /// </summary>
    [Serializable]
    [DataContract]
    public class GetNodeModel
    {
        /// <summary>
        /// Gets or sets a value indicating whether to include translations.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to include translations; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool IncludeTranslations { get; set; }
    }
}
