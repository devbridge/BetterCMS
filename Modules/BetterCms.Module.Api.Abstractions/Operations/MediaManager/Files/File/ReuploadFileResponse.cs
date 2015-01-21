using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.MediaManager.Files.File
{
    /// <summary>
    /// Re-loading file response.
    /// </summary>
    [DataContract]
    [Serializable]
    public class ReuploadFileResponse : SaveResponseBase
    {
    }
}
