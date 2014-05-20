using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Root.Layouts.Layout
{
    /// <summary>
    /// Response after layout saving.
    /// </summary>
    [DataContract]
    [Serializable]
    public class PutLayoutResponse : SaveResponseBase
    {
    }
}