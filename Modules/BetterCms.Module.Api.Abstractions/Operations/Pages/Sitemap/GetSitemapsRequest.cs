using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Pages.Sitemap
{
    [Route("/sitemap-trees", Verbs = "GET")]
    [DataContract]
    public class GetSitemapsRequest : RequestBase<GetSitemapsModel>, IReturn<GetSitemapsResponse>
    {
    }
}