using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.MediaManager.Folders.Folder
{
    /// <summary>
    /// Request for folder update or creation.
    /// </summary>
    [Route("/folders", Verbs = "POST")]
    [DataContract]
    [Serializable]
    public class PostFolderRequest : RequestBase<Folder.SaveFolderModel>, IReturn<PostFolderResponse>
    {
    }
}