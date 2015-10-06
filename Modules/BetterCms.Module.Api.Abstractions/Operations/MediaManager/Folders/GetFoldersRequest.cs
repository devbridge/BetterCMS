using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.MediaManager.Folders
{
    /// <summary>
    /// Folder list request.
    /// </summary>
    [DataContract]
    [Serializable]
    public class GetFoldersRequest : RequestBase<GetFolderModel>
    {
    }
}
