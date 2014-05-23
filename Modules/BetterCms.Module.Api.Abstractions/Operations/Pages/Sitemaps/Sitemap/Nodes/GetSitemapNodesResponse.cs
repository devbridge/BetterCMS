using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Pages.Sitemaps.Sitemap.Nodes
{
    [Serializable]
    [DataContract]
    public class GetSitemapNodesResponse : ListResponseBase<SitemapNodeModel>
    {
    }
}