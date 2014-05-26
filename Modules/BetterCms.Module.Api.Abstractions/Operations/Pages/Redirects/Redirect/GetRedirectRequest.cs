using System;
using System.Runtime.Serialization;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Pages.Redirects.Redirect
{
    [Route("/redirects/{RedirectId}", Verbs = "GET")]
    [DataContract]
    [Serializable]
    public class GetRedirectRequest : IReturn<GetRedirectResponse>
    {
        [DataMember]
        public Guid RedirectId { get; set; }
    }
}