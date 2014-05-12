using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;
using BetterCms.Module.Api.Operations.Pages.Pages.Page;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Pages.Sitemaps.Sitemap
{
    /// <summary>
    /// Request to get sitemap data.
    /// </summary>
    [Route("/sitemaps/{SitemapId}", Verbs = "GET")]
    [Serializable]
    [DataContract]
    public class GetSitemapRequest : RequestBase<GetSitemapModel>, IReturn<GetPageResponse>
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