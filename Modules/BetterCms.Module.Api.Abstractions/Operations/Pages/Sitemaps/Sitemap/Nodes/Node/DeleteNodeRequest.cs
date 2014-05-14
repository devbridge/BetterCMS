using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Pages.Sitemaps.Sitemap.Nodes.Node
{
    /// <summary>
    /// Request to delete sitemap node.
    /// </summary>
    [Route("/sitemaps/{SitemapId}/nodes/{NodeId}", Verbs = "DELETE")]
    [Serializable]
    [DataContract]
    public class DeleteNodeRequest : DeleteRequestBase, IReturn<DeleteNodeResponse>
    {
        /// <summary>
        /// Gets or sets the sitemap identifier.
        /// </summary>
        /// <value>
        /// The sitemap identifier.
        /// </value>
        [DataMember]
        public Guid SitemapId { get; set; }

        /// <summary>
        /// Gets or sets the node identifier.
        /// </summary>
        /// <value>
        /// The node identifier.
        /// </value>
        [DataMember]
        public Guid NodeId { get; set; }
    }
}