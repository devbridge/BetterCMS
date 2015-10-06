using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Root.Layouts.Layout
{
    /// <summary>
    /// Request for layout update or creation.
    /// </summary>
    [DataContract]
    [Serializable]
    public class PutLayoutRequest : PutRequestBase<SaveLayoutModel>
    {
    }
}