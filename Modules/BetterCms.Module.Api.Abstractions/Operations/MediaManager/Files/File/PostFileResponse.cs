using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.MediaManager.Files.File
{
    /// <summary>
    /// File creation response.
    /// </summary>
    [DataContract]
    [Serializable]
    public class PostFileResponse : SaveResponseBase
    {
    }
}