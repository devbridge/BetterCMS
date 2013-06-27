using System.Runtime.Serialization;

namespace BetterCms.Module.Api.Operations.MediaManager.GetMediaTree
{
    [DataContract]
    public class GetMediaTreeRequest : RequestBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetMediaTreeRequest" /> class.
        /// </summary>
        public GetMediaTreeRequest()
        {
            IncludeImagesTree = true;
            IncludeVideosTree = true;
            IncludeFilesTree = true;
        }

        /// <summary>
        /// Gets or sets a value indicating whether to include images tree.
        /// </summary>
        /// <value>
        ///   <c>true</c> if include images tree; otherwise, <c>false</c>.
        /// </value>
        [DataMember(Order = 10, Name = "includeImagesTree")]
        public bool IncludeImagesTree { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to include the files of images tree.
        /// </summary>
        /// <value>
        ///   <c>true</c> if include the files of images tree; otherwise, <c>false</c>.
        /// </value>
        [DataMember(Order = 20, Name = "includeImages")]
        public bool IncludeImages { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to include videos tree.
        /// </summary>
        /// <value>
        ///   <c>true</c> if include videos tree; otherwise, <c>false</c>.
        /// </value>
        [DataMember(Order = 30, Name = "includeVideosTree")]
        public bool IncludeVideosTree { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to include the files of videos tree.
        /// </summary>
        /// <value>
        ///   <c>true</c> if include the files of videos  tree; otherwise, <c>false</c>.
        /// </value>
        [DataMember(Order = 40, Name = "includeVideos")]
        public bool IncludeVideos { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to include files tree.
        /// </summary>
        /// <value>
        ///   <c>true</c> if include files tree; otherwise, <c>false</c>.
        /// </value>
        [DataMember(Order = 50, Name = "includeFilesTree")]
        public bool IncludeFilesTree { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to include the files of files tree.
        /// </summary>
        /// <value>
        ///   <c>true</c> if include the files of files tree; otherwise, <c>false</c>.
        /// </value>
        [DataMember(Order = 60, Name = "includeFiles")]
        public bool IncludeFiles { get; set; }
    }
}