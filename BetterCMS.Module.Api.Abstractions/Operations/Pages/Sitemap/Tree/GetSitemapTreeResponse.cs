using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Pages.Sitemap.Tree
{
    [DataContract]
    public class GetSitemapTreeResponse : ResponseBase<System.Collections.Generic.List<SitemapTreeNodeModel>>
    {
    }
}