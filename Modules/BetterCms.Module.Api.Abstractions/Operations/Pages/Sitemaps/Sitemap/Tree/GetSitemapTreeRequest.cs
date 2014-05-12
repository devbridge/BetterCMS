using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Pages.Sitemaps.Sitemap.Tree
{
    [Route("/sitemaps/{SitemapId}/tree/", Verbs = "GET")]
    [Serializable]
    [DataContract]
    public class GetSitemapTreeRequest : RequestBase<GetSitemapTreeModel>, IReturn<GetSitemapTreeResponse>
    {
        [DataMember]
        public Guid SitemapId { get; set; }
    }
}