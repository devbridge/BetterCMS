using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Pages.Redirects.Redirect
{
    [Route("/redirects/{RedirectId}", Verbs = "GET")]
    [DataContract]
    public class GetRedirectRequest : IReturn<GetRedirectResponse>
    {
        [DataMember]
        public System.Guid RedirectId { get; set; }
    }
}