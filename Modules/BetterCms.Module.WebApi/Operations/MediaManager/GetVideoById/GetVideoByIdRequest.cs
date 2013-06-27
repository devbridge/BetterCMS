using System.Runtime.Serialization;

namespace BetterCms.Module.Api.Operations.MediaManager.GetVideoById
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