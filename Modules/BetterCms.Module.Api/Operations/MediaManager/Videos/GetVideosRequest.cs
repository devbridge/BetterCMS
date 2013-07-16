using BetterCms.Module.Api.Operations.Enums;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.MediaManager.Videos
{
    [Route("/videos", Verbs = "GET")]
    public class GetVideosRequest : ListRequestBase, IReturn<GetVideosResponse>, IFilterByTags
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetVideosRequest" /> class.
        /// </summary>
        public GetVideosRequest()
        {
            IncludeFolders = true;
            IncludeVideos = true;

            FilterByTagsConnector = FilterConnector.And;
        }

        /// <summary>
        /// Gets or sets the folder id.
        /// </summary>
        /// <value>
        /// The folder id.
        /// </value>
        public System.Guid? FolderId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to include videos.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to include videos; otherwise, <c>false</c>.
        /// </value>
        public bool IncludeVideos { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to include archived medias.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to include archived medias; otherwise, <c>false</c>.
        /// </value>
        public bool IncludeArchived { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to include folders.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to include folders; otherwise, <c>false</c>.
        /// </value>
        public bool IncludeFolders { get; set; }

        /// <summary>
        /// Gets or sets the video tags for filtering.
        /// </summary>
        /// <value>
        /// The video tags for filtering.
        /// </value>
        public System.Collections.Generic.List<string> FilterByTags { get; set; }

        /// <summary>
        /// Gets or sets the tags filter connector.
        /// </summary>
        /// <value>
        /// The tags filter connector.
        /// </value>
        public FilterConnector FilterByTagsConnector { get; set; }
    }
}
