using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.MediaManager.Files.File
{
    /// <summary>
    /// Request for file update or creation.
    /// </summary>
    [DataContract]
    [Serializable]
    public class DeleteFileRequest : DeleteRequestBase
    {
    }
}