using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.MediaManager.Folders.Folder
{
    [DataContract]
    [Serializable]
    public class GetFolderResponse : ResponseBase<FolderModel>
    {
    }
}