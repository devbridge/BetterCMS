using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Pages.Pages.Page
{
    /// <summary>
    /// Page save response.
    /// </summary>
    [DataContract]
    public class PutPageResponse : ResponseBase<Guid?>
    {
    }
}