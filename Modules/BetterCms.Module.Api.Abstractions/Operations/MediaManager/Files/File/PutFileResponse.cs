using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.MediaManager.Files.File
{
    /// <summary>
    /// Response after file saving.
    /// </summary>
    [DataContract]
    [Serializable]
    public class PutFileResponse : SaveResponseBase
    {
    }
}