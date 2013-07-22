using System.Runtime.Serialization;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Pages.Sitemap.Tree
{
    [Route("/sitemap-tree", Verbs = "GET")]
    [DataContract]
    public class GetSitemapTreeRequest : RequestBase<GetSitemapTreeModel>, IReturn<GetSitemapTreeResponse>
    {
    }
}