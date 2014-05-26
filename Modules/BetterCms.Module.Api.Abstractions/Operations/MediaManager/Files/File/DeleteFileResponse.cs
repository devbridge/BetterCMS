using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.MediaManager.Files.File
{
    /// <summary>
    /// Response for file delete operation.
    /// </summary>
    [DataContract]
    [Serializable]
    public class DeleteFileResponse : DeleteResponseBase
    {
    }
}