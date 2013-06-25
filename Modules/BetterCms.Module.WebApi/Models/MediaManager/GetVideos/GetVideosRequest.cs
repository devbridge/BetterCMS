using System.Runtime.Serialization;

namespace BetterCms.Module.WebApi.Models.MediaManager.GetVideos
{
    [DataContract]
    public class GetVideosRequest : ListRequestBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetVideosRequest" /> class.
        /// </summary>
        public GetVideosRequest()
        {
            IncludeFolders = true;
            IncludeVideos = true;
        }

        /// <summary>
        /// Gets or sets a value indicating whether to include videos.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to include videos; otherwise, <c>false</c>.
        /// </value>
        [DataMember(Order = 10, Name = "includeVideos")]
        public bool IncludeVideos { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to include folders.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to include folders; otherwise, <c>false</c>.
        /// </value>
        [DataMember(Order = 20, Name = "includeFolders")]
        public bool IncludeFolders { get; set; }

        /// <summary>
        /// Gets or sets the folder id.
        /// </summary>
        /// <value>
        /// The folder id.
        /// </value>
        [DataMember(Order = 30, Name = "folderId")]
        public System.Guid? FolderId { get; set; }
    }
}
