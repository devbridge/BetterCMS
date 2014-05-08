using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Pages.Sitemaps.Sitemap
{
    /// <summary>
    /// Request to save sitemap.
    /// </summary>
    [Route("/sitemaps/{SitemapId}", Verbs = "PUT")]
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