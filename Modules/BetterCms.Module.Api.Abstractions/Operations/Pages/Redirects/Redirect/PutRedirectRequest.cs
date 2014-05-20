using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Pages.Redirects.Redirect
{
    /// <summary>
    /// Request for redirect update.
    /// </summary>
    [Route("/redirects/{Id}", Verbs = "PUT")]
    [DataContract]
    [Serializable]
    public class PutRedirectRequest : PutRequestBase<SaveRedirectModel>, IReturn<PutRedirectResponse>
    {
    }
}
