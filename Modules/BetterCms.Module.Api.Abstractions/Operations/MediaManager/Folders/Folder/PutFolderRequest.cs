using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.MediaManager.Folders.Folder
{
    /// <summary>
    /// Request for tag update or creation.
    /// </summary>
    [Route("/folders/{FolderId}", Verbs = "PUT")]
    [DataContract]
    [Serializable]
    public class PutFolderRequest : RequestBase<SaveFolderModel>, IReturn<PutFolderResponse>
    {
        /// <summary>
        /// Gets or sets the folder identifier.
        /// </summary>
        /// <value>
        /// The folder identifier.
        /// </value>
        [DataMember]
        public Guid? FolderId { get; set; }
    }
}