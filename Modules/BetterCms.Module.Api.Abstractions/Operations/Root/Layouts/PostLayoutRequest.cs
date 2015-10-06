using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;
using BetterCms.Module.Api.Operations.Root.Layouts.Layout;

namespace BetterCms.Module.Api.Operations.Root.Layouts
{
    /// <summary>
    /// Request for layout creation.
    /// </summary>
    [DataContract]
    [Serializable]
    public class PostLayoutRequest : RequestBase<SaveLayoutModel>
    {
    }
}