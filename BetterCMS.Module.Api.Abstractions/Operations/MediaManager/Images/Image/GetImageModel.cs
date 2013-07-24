using System.Runtime.Serialization;

namespace BetterCms.Module.Api.Operations.MediaManager.Images.Image
{
    [DataContract]
    public class GetImageModel
    {
        /// <summary>
        /// Gets or sets the image id.
        /// </summary>
        /// <value>
        /// The image id.
        /// </value>
        [DataMember]
        public System.Guid ImageId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to include tags to response.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to include tags to response; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool IncludeTags { get; set; }
    }
}