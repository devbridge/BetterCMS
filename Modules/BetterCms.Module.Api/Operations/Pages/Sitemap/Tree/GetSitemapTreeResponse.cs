using System.Runtime.Serialization;

namespace BetterCms.Module.Api.Operations.Pages.Sitemap.Tree
{
    [DataContract]
    public class GetSitemapTreeResponse : ResponseBase<System.Collections.Generic.List<SitemapTreeNodeModel>>
    {
    }
}