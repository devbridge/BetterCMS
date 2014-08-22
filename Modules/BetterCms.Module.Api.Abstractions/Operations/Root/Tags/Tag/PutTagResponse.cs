using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Root.Tags.Tag
{
    /// <summary>
    /// Response after tag saving.
    /// </summary>
    [DataContract]
    [Serializable]
    public class PutTagResponse : SaveResponseBase
    {
    }
}