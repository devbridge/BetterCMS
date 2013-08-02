using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Pages.Sitemap.Nodes.Node
{
    [DataContract]
    public class GetSitemapNodeResponse : ResponseBase<SitemapNodeModel>
    {
    }
}