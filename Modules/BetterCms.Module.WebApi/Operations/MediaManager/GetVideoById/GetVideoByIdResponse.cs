using System.Runtime.Serialization;

namespace BetterCms.Module.Api.Operations.MediaManager.GetVideoById
{
    [DataContract]
    public class GetVideoByIdResponse : ResponseBase<MediaModel>
    {
        /// <summary>
        /// Gets or sets the list of video tags.
        /// </summary>
        /// <value>
        /// The list of video tags.
        /// </value>
        [DataMember(Order = 10, Name = "tags")]
        private System.Collections.Generic.IList<TagModel> Tags { get; set; }
    }
}