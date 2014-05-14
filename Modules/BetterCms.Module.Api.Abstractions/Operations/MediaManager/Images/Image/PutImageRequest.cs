using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.MediaManager.Images.Image
{
    /// <summary>
    /// Request for tag update or creation.
    /// </summary>
    [Route("/images/{ImageId}", Verbs = "PUT")]
    [DataContract]
    [Serializable]
    public class PutImageRequest : RequestBase<SaveImageModel>, IReturn<PutImageResponse>
    {
        /// <summary>
        /// Gets or sets the image identifier.
        /// </summary>
        /// <value>
        /// The image identifier.
        /// </value>
        [DataMember]
        public Guid ImageId { get; set; }
    }
}