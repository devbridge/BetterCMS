using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Pages.Redirects.Redirect
{
    /// <summary>
    /// Redirect delete request for REST.
    /// </summary>
    [Route("/redirects/{RedirectId}", Verbs = "DELETE")]
    [DataContract]
    [Serializable]
    public class DeleteRedirectRequest : DeleteRequestBase, IReturn<DeleteRedirectResponse>
    {
        /// <summary>
        /// Gets or sets the redirect identifier.
        /// </summary>
        /// <value>
        /// The redirect identifier.
        /// </value>
        [DataMember]
        public Guid RedirectId { get; set; }
    }
}