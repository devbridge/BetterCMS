using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;
using BetterCms.Module.Api.Operations.Pages.Sitemaps.Sitemap;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Pages.Sitemaps
{
    /// <summary>
    /// Request for sitemap creation.
    /// </summary>
    [Route("/sitemaps", Verbs = "POST")]
    [DataContract]
    public class PostSitemapRequest : RequestBase<SaveSitemapModel>, IReturn<PostSitemapResponse>
    {
    }
}