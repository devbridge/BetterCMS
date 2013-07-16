namespace BetterCms.Module.Api.Operations.MediaManager.Videos.Video
{
    public class GetVideoResponse : ResponseBase<VideoModel>
    {
        /// <summary>
        /// Gets or sets the list of video tags.
        /// </summary>
        /// <value>
        /// The list of video tags.
        /// </value>
        public System.Collections.Generic.IList<TagModel> Tags { get; set; }
    }
}