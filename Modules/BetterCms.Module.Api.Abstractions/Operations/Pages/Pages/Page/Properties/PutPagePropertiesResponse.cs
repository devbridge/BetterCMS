using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Pages.Pages.Page.Properties
{
    /// <summary>
    /// Response after page properties saving.
    /// </summary>
    [DataContract]
    public class PutPagePropertiesResponse : ResponseBase<Guid?>
    {
    }
}