using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Pages.Sitemaps.Sitemap.Nodes.Node
{
    [Serializable]
    [DataContract]
    public class GetNodeRequest : RequestBase<GetNodeModel>
    {
        [DataMember]
        public Guid SitemapId { get; set; }

        [DataMember]
        public Guid NodeId { get; set; }
    }
}