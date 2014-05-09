using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Pages.Pages
{
    /// <summary>
    /// Page creation response.
    /// </summary>
    [DataContract]
    public class PostPageResponse : ResponseBase<Guid?>
    {
    }
}
