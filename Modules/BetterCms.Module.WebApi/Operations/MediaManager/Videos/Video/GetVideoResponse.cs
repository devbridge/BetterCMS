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
        private System.Collections.Generic.IList<TagModel> Tags { get; set; }
    }
}