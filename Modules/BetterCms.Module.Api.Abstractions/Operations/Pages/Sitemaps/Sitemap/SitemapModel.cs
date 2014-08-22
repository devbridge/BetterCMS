using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Pages.Sitemaps.Sitemap
{
    /// <summary>
    /// Sitemap model.
    /// </summary>
    [Serializable]
    [DataContract]
    public class SitemapModel : ModelBase
    {
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        [DataMember]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the list of tag names.
        /// </summary>
        /// <value>
        /// The list of tag names.
        /// </value>
        [DataMember]
        public System.Collections.Generic.List<string> Tags { get; set; }
    }
}