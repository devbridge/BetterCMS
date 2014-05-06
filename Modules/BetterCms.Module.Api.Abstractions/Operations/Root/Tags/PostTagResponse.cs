using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Root.Tags
{
    /// <summary>
    /// Tag creation response.
    /// </summary>
    [DataContract]
    public class PostTagResponse : ResponseBase<Guid?>
    {
    }
}