using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.MediaManager.Files.File
{
    /// <summary>
    /// Request for file update or creation.
    /// </summary>
    [Route("/files/{FileId}", Verbs = "DELETE")]
    [DataContract]
    [Serializable]
    public class DeleteFileRequest : DeleteRequestBase, IReturn<DeleteFileResponse>
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        [DataMember]
        public Guid FileId { get; set; }
    }
}