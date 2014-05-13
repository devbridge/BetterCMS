using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;
using BetterCms.Module.Api.Operations.Pages.Sitemaps.Sitemap.Nodes.Node;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Pages.Sitemaps.Sitemap
{
    /// <summary>
    /// Request to save sitemap.
    /// </summary>
    [Route("/sitemaps/{SitemapId}", Verbs = "PUT")]
    [Serializable]
    [DataContract]
    public class PutSitemapRequest : RequestBase<SaveSitemapModel>, IReturn<PutSitemapResponse>
    {
        /// <summary>
        /// Gets or sets the sitemap identifier.
        /// </summary>
        /// <value>
        /// The sitemap identifier.
        /// </value>
        [DataMember]
        public Guid? SitemapId { get; set; }
    }
}