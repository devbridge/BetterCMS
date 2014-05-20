using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.MediaManager.Folders.Folder
{
    /// <summary>
    /// Response for folder delete operation.
    /// </summary>
    [DataContract]
    [Serializable]
    public class DeleteFolderResponse : DeleteResponseBase
    {
    }
}