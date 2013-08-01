using System.Collections.Generic;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Pages.Sitemap.Tree
{
    [DataContract]
    public class SitemapTreeNodeModel : ModelBase
    {
        /// <summary>
        /// Gets or sets the parent node id.
        /// </summary>
        /// <value>
        /// The parent node id.
        /// </value>
        [DataMember]
        public System.Guid? ParentId { get; set; }

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
        /// The ndoe URL.
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
        /// Gets or sets the children nodes.
        /// </summary>
        /// <value>
        /// The children nodes.
        /// </value>
        [DataMember]
        public IList<SitemapTreeNodeModel> ChildrenNodes { get; set; }
    }
}