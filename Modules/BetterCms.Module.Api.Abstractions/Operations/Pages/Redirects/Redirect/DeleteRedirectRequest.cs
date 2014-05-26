using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Pages.Redirects.Redirect
{
    /// <summary>
    /// Redirect delete request for REST.
    /// </summary>
    [Route("/redirects/{Id}", Verbs = "DELETE")]
    [DataContract]
    [Serializable]
    public class DeleteRedirectRequest : DeleteRequestBase, IReturn<DeleteRedirectResponse>
    {
    }
}