using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Pages.Sitemap.Nodes
{
    [DataContract]
    public class GetSitemapNodesResponse :  ListResponseBase<SitemapNodeModel>
    {
    }
}