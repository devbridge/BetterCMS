using System.Runtime.Serialization;

namespace BetterCms.Module.WebApi.Models.MediaManager.GetMediaById
{
    [DataContract]
    public class GetMediaByIdRequest : RequestBase
    {
        /// <summary>
        /// Gets or sets the media id.
        /// </summary>
        /// <value>
        /// The media id.
        /// </value>
        [DataMember(Order = 10, Name = "mediaId")]
        public System.Guid MediaId { get; set; }
    }
}