using System;
using System.Runtime.Serialization;

namespace BetterCms.Module.Api.Operations.Pages.Redirects.Redirect
{
    [DataContract]
    [Serializable]
    public class GetRedirectRequest
    {
        [DataMember]
        public Guid RedirectId { get; set; }
    }
}