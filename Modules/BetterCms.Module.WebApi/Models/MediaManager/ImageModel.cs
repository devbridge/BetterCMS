using System.Runtime.Serialization;

namespace BetterCms.Module.WebApi.Models.MediaManager
{
    [DataContract]
    public class ImageModel : FileModel
    {
        /// <summary>
        /// Gets or sets the thumbnail URL.
        /// </summary>
        /// <value>
        /// The thumbnail URL.
        /// </value>
        [DataMember(Order = 420, Name = "thumbnailUrl")]
        public string ThumbnailUrl { get; set; }

        /// <summary>
        /// Gets or sets the tooltip.
        /// </summary>
        /// <value>
        /// The tooltip.
        /// </value>
        [DataMember(Order = 420, Name = "caption")]
        public string Caption { get; set; }
    }
}