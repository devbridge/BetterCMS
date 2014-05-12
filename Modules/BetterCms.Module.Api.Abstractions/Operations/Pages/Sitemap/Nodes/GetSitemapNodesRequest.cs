using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Pages.Sitemap.Nodes
{
    [Obsolete("Use everything from BetterCms.Module.Api.Operations.Pages.Sitemaps name space.")]
    [Route("/sitemap-nodes/{SitemapId}", Verbs = "GET")]
    [DataContract]
    [Serializable]
    public class GetSitemapNodesRequest : RequestBase<DataOptions>, IReturn<GetSitemapNodesResponse>
    {
        [DataMember]
        public Guid SitemapId { get; set; }
    }
}