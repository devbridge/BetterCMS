using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Root.Layouts
{
    /// <summary>
    /// Layout creation response.
    /// </summary>
    [DataContract]
    [Serializable]
    public class PostLayoutResponse : SaveResponseBase
    {
    }
}