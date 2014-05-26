using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Pages.Sitemap
{
    [Obsolete("Use everything from BetterCms.Module.Api.Operations.Pages.Sitemaps name space.")]
    [Route("/sitemap-trees", Verbs = "GET")]
    [DataContract]
    [Serializable]
    public class GetSitemapsRequest : RequestBase<GetSitemapsModel>, IReturn<GetSitemapsResponse>
    {
    }
}