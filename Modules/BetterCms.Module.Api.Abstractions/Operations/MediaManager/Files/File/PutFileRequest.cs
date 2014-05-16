using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.MediaManager.Files.File
{
    /// <summary>
    /// Request for tag update or creation.
    /// </summary>
    [Route("/files/{FileId}", Verbs = "PUT")]
    [DataContract]
    [Serializable]
    public class PutFileRequest : RequestBase<SaveFileModel>, IReturn<PutFileResponse>
    {
        /// <summary>
        /// Gets or sets the file identifier.
        /// </summary>
        /// <value>
        /// The file identifier.
        /// </value>
        [DataMember]
        public Guid? FileId { get; set; }
    }
}