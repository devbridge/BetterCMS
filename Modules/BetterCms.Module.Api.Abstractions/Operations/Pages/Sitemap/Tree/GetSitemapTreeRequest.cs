using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Pages.Sitemap.Tree
{
    [Route("/sitemap-tree/{SitemapId}", Verbs = "GET")]
    [DataContract]
    public class GetSitemapTreeRequest : RequestBase<GetSitemapTreeModel>, IReturn<GetSitemapTreeResponse>
    {
        [DataMember]
        public System.Guid SitemapId { get; set; }
    }
}