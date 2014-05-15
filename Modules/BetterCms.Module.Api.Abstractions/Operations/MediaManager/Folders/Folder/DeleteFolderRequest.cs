using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.MediaManager.Folders.Folder
{
    /// <summary>
    /// Request for folder update or creation.
    /// </summary>
    [Route("/folders/{FolderId}", Verbs = "DELETE")]
    [DataContract]
    [Serializable]
    public class DeleteFolderRequest : DeleteRequestBase, IReturn<DeleteFolderResponse>
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        [DataMember]
        public Guid FolderId { get; set; }
    }
}