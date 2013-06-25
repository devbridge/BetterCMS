using System.Runtime.Serialization;

namespace BetterCms.Module.WebApi.Models.MediaManager.GetVideoById
{
    [DataContract]
    public class GetVideoByIdRequest : RequestBase
    {
        /// <summary>
        /// Gets or sets the video id.
        /// </summary>
        /// <value>
        /// The video id.
        /// </value>
        [DataMember(Order = 10, Name = "videoId")]
        public System.Guid VideoId { get; set; }
    }
}