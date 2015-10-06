using System;
using System.Runtime.Serialization;

namespace BetterCms.Module.Api.Operations.Pages.Sitemap.Nodes.Node
{
    [Obsolete("Use everything from BetterCms.Module.Api.Operations.Pages.Sitemaps name space.")]
    [DataContract]
    [Serializable]
    public class GetSitemapNodeRequest
    {
        [DataMember]
        public Guid NodeId { get; set; }
    }
}