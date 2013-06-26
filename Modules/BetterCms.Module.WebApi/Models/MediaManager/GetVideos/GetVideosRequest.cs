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
        /// Gets or sets a value indicating whether to include archived medias.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to include archived medias; otherwise, <c>false</c>.
        /// </value>
        [DataMember(Order = 30, Name = "includeArchived")]
        public bool IncludeArchived { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether media is archived.
        /// </summary>
        /// <value>
        /// <c>true</c> if media is archived; otherwise, <c>false</c>.
        /// </value>
        [DataMember(Order = 40, Name = "isArchived")]
        public bool IsArchived { get; set; }
    }
}
