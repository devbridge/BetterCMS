using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Pages.Pages.Page.Properties
{
    /// <summary>
    /// Page save response.
    /// </summary>
    [DataContract]
    public class PutPagePropertiesResponse : ResponseBase<Guid?>
    {
    }
}