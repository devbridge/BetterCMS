using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Root.Tags
{
    /// <summary>
    /// Request for tag creation.
    /// </summary>
    [DataContract]
    [Serializable]
    public class PostTagRequest : RequestBase<SaveTagModel>
    {
    }
}