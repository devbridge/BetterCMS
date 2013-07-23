using System.Runtime.Serialization;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Pages.Redirects
{
    [Route("/redirects", Verbs = "GET")]
    [DataContract]
    public class GetRedirectsRequest : RequestBase<DataOptions>, IReturn<GetRedirectsResponse>
    {
    }
}