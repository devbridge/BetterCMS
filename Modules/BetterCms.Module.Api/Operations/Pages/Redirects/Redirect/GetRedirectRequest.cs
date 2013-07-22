using System.Runtime.Serialization;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Pages.Redirects.Redirect
{
    [Route("/redirects/{RedirectId}", Verbs = "GET")]
    [DataContract]
    public class GetRedirectRequest : RequestBase<GetRedirectModel>, IReturn<GetRedirectResponse>
    {
    }
}