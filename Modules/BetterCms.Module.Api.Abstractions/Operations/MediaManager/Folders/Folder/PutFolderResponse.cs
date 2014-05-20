using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.MediaManager.Folders.Folder
{
    /// <summary>
    /// Response after folder saving.
    /// </summary>
    [DataContract]
    [Serializable]
    public class PutFolderResponse : SaveResponseBase
    {
    }
}