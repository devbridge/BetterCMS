using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.MediaManager.Images.Image
{
    /// <summary>
    /// Request for image update or creation.
    /// </summary>
    [Route("/images/{ImageId}", Verbs = "DELETE")]
    [DataContract]
    [Serializable]
    public class DeleteImageRequest : DeleteRequestBase, IReturn<DeleteImageResponse>
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        [DataMember]
        public Guid ImageId { get; set; }
    }
}