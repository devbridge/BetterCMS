using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Pages.Sitemaps.Sitemap.Nodes
{
    /// <summary>
    /// Sitemap node data model.
    /// </summary>
    [Serializable]
    [DataContract]
    public class SitemapNodeModel : ModelBase
    {
        /// <summary>
        /// Gets or sets the parent node id.
        /// </summary>
        /// <value>
        /// The parent node id.
        /// </value>
        [DataMember]
        public Guid? ParentId { get; set; }

        /// <summary>
        /// Gets or sets the node title.
        /// </summary>
        /// <value>
        /// The node title.
        /// </value>
        [DataMember]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the node URL.
        /// </summary>
        /// <value>
        /// The node URL.
        /// </value>
        [DataMember]
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets the node display order.
        /// </summary>
        /// <value>
        /// The node display order.
        /// </value>
        [DataMember]
        public int DisplayOrder { get; set; }

        /// <summary>
        /// Gets or sets the macro.
        /// </summary>
        /// <value>
        /// The macro.
        /// </value>
        [DataMember]
        public string Macro { get; set; }

        /// <summary>
        /// Gets or sets the page identifier.
        /// </summary>
        /// <value>
        /// The page identifier.
        /// </value>
        [DataMember]
        public Guid? PageId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether page is published.
        /// </summary>
        /// <value>
        ///   <c>true</c> if page is published; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool PageIsPublished { get; set; }

        /// <summary>
        /// Gets or sets the page language identifier.
        /// </summary>
        /// <value>
        /// The page language identifier.
        /// </value>
        [DataMember]
        public Guid? PageLanguageId { get; set; }
    }
}