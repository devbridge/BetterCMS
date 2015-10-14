using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;
using BetterCms.Module.Api.Operations.Pages.Redirects.Redirect;

namespace BetterCms.Module.Api.Operations.Pages.Redirects
{
    /// <summary>
    /// Request for redirect creation.
    /// </summary>
    [DataContract]
    [Serializable]
    public class PostRedirectRequest : RequestBase<SaveRedirectModel>
    {
    }
}
