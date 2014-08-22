using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;
using BetterCms.Module.Api.Operations.Pages.Redirects.Redirect;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Pages.Redirects
{
    /// <summary>
    /// Request for redirect creation.
    /// </summary>
    [Route("/redirects/", Verbs = "POST")]
    [DataContract]
    [Serializable]
    public class PostRedirectRequest : RequestBase<SaveRedirectModel>, IReturn<PostRedirectResponse>
    {
    }
}
