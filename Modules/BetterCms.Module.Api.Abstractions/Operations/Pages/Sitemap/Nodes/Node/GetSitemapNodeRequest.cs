using System;
using System.Runtime.Serialization;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Pages.Sitemap.Nodes.Node
{
    [Obsolete("Use everything from BetterCms.Module.Api.Operations.Pages.Sitemaps name space.")]
    [Route("/sitemap-node/{NodeId}", Verbs = "GET")]
    [DataContract]
    [Serializable]
    public class GetSitemapNodeRequest : IReturn<SitemapNodeModel>
    {
        [DataMember]
        public Guid NodeId { get; set; }
    }
}