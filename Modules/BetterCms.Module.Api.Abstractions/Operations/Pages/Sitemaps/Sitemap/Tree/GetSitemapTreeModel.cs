using System;
using System.Runtime.Serialization;

namespace BetterCms.Module.Api.Operations.Pages.Sitemaps.Sitemap.Tree
{
    [Serializable]
    [DataContract]
    public class GetSitemapTreeModel
    {
        /// <summary>
        /// Gets or sets the node id.
        /// </summary>
        /// <value>
        /// The node id.
        /// </value>
        [DataMember]
        public Guid? NodeId { get; set; }

        /// <summary>
        /// Gets or sets the language identifier.
        /// </summary>
        /// <value>
        /// The language identifier.
        /// </value>
        [DataMember]
        public Guid? LanguageId { get; set; }
    }
}