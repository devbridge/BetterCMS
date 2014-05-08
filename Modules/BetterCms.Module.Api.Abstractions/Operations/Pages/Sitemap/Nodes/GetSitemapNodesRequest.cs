using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Pages.Sitemap.Nodes
{
    [Obsolete("Use everything from BetterCms.Module.Api.Operations.Pages.Sitemaps name space.")]
    [Route("/sitemap-nodes/{SitemapId}", Verbs = "GET")]
    [DataContract]
    public class GetSitemapNodesRequest : RequestBase<DataOptions>, IReturn<GetSitemapNodesResponse>
    {
        [DataMember]
        public System.Guid SitemapId { get; set; }
    }
}