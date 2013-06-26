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

        /// <summary>
        /// Gets or sets a value indicating whether to include tags to response.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to include tags to response; otherwise, <c>false</c>.
        /// </value>
        [DataMember(Order = 20, Name = "includeTags")]
        public bool IncludeTags { get; set; }
    }
}