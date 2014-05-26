using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Pages.Sitemaps.Sitemap
{
    /// <summary>
    /// Request to save sitemap.
    /// </summary>
    [Route("/sitemaps/{Id}", Verbs = "PUT")]
    [Serializable]
    [DataContract]
    public class PutSitemapRequest : PutRequestBase<SaveSitemapModel>, IReturn<PutSitemapResponse>
    {
    }
}