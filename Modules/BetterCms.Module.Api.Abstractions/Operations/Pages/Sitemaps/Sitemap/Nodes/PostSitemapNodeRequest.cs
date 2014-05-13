using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;
using BetterCms.Module.Api.Operations.Pages.Sitemaps.Sitemap.Nodes.Node;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Pages.Sitemaps.Sitemap.Nodes
{
    /// <summary>
    /// Request for page creation.
    /// </summary>
    [Route("/sitemaps/{SitemapId}/nodes/", Verbs = "POST")]
    [DataContract]
    [Serializable]
    public class PostSitemapNodeRequest : RequestBase<SaveNodeModel>, IReturn<PostSitemapNodeResponse>
    {
        /// <summary>
        /// Gets or sets the sitemap identifier.
        /// </summary>
        /// <value>
        /// The sitemap identifier.
        /// </value>
        [DataMember]
        public Guid SitemapId { get; set; }
    }
}
