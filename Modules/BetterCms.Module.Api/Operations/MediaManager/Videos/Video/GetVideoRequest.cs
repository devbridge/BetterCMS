using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.MediaManager.Videos.Video
{
    public class GetVideoModel
    {
        /// <summary>
        /// Gets or sets the video id.
        /// </summary>
        /// <value>
        /// The video id.
        /// </value>
        public System.Guid VideoId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to include tags to response.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to include tags to response; otherwise, <c>false</c>.
        /// </value>
        public bool IncludeTags { get; set; }
    }

    [Route("/videos/{VideoId}", Verbs = "GET")]
    public class GetVideoRequest : RequestBase<GetVideoModel>, IReturn<GetVideoResponse>
    {
    }
}