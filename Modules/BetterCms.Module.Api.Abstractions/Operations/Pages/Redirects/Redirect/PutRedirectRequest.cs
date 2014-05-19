using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Pages.Redirects.Redirect
{
    /// <summary>
    /// Request for redirect update.
    /// </summary>
    [Route("/redirects/{RedirectId}", Verbs = "PUT")]
    [DataContract]
    [Serializable]
    public class PutRedirectRequest : RequestBase<SaveRedirectModel>, IReturn<PutRedirectResponse>
    {
        /// <summary>
        /// Gets or sets the redirect identifier.
        /// </summary>
        /// <value>
        /// The redirect identifier.
        /// </value>
        [DataMember]
        public Guid? RedirectId { get; set; }
    }
}
