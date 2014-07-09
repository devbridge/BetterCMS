using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Pages.Sitemaps.Sitemap
{
    /// <summary>
    /// Request to get sitemap data.
    /// </summary>
    [Route("/sitemaps/{SitemapId}", Verbs = "GET")]
    [Serializable]
    [DataContract]
    public class GetSitemapRequest : RequestBase<GetSitemapModel>, IReturn<GetSitemapResponse>
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