using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Pages.Redirects.Redirect
{
    /// <summary>
    /// Request for redirect update.
    /// </summary>
    [DataContract]
    [Serializable]
    public class PutRedirectRequest : PutRequestBase<SaveRedirectModel>
    {
    }
}
