using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;
using BetterCms.Module.Api.Operations.Pages.Sitemaps.Sitemap.Nodes.Node;

namespace BetterCms.Module.Api.Operations.Pages.Sitemaps.Sitemap
{
    /// <summary>
    /// Sitemap node data model.
    /// </summary>
    [Serializable]
    [DataContract]
    public class SaveSitemapNodeModel : SaveModelBase
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        [DataMember]
        public Guid? Id { get; set; }

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
        /// Gets or sets the translations.
        /// </summary>
        /// <value>
        /// The translations.
        /// </value>
        [DataMember]
        public IList<SaveSitemapNodeTranslation> Translations { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to use page title as node title.
        /// </summary>
        /// <value>
        /// <c>true</c> if to use page title as node title; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool UsePageTitleAsNodeTitle { get; set; }

        /// <summary>
        /// Gets or sets the nodes.
        /// </summary>
        /// <value>
        /// The nodes.
        /// </value>
        [DataMember]
        public IList<SaveSitemapNodeModel> Nodes { get; set; }
    }
}