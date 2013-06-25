using System.Runtime.Serialization;

namespace BetterCms.Module.WebApi.Models.MediaManager.GetImageById
{
    [DataContract]
    public class GetImageByIdRequest : RequestBase
    {
        /// <summary>
        /// Gets or sets the image id.
        /// </summary>
        /// <value>
        /// The image id.
        /// </value>
        [DataMember(Order = 10, Name = "imageId")]
        public System.Guid ImageId { get; set; }
    }
}