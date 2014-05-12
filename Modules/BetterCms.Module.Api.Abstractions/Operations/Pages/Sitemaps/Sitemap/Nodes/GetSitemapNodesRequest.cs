using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Pages.Sitemaps.Sitemap.Nodes
{
    /// <summary>
    /// Request to get sitemap data.
    /// </summary>
    [Route("/sitemaps/{SitemapId}/nodes/", Verbs = "GET")]
    [Serializable]
    [DataContract]
    public class GetSitemapNodesRequest : RequestBase<DataOptions>, IReturn<GetSitemapNodesResponse>
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