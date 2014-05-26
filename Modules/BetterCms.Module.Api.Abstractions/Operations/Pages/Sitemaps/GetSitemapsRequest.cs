using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Pages.Sitemaps
{
    /// <summary>
    /// Request for getting sitemaps list.
    /// </summary>
    [Route("/sitemaps", Verbs = "GET")]
    [DataContract]
    [Serializable]
    public class GetSitemapsRequest : RequestBase<GetSitemapsModel>, IReturn<GetSitemapsResponse>
    {
    }
}