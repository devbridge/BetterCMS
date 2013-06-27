using System.Collections.Generic;
using System.Runtime.Serialization;

namespace BetterCms.Module.WebApi.Models.MediaManager.GetMediaTree
{
    [DataContract]
    public class MediaTreeContainer
    {
        /// <summary>
        /// Gets or sets the tree of image medias.
        /// </summary>
        /// <value>
        /// The tree of image medias.
        /// </value>
        [DataMember(Order = 10, Name = "imagesTree")]
        public IList<MediaModel> ImagesTree { get; set; }

        /// <summary>
        /// Gets or sets the tree of video medias.
        /// </summary>
        /// <value>
        /// The tree of video medias.
        /// </value>
        [DataMember(Order = 10, Name = "videosTree")]
        public IList<MediaModel> VideosTree { get; set; }

        /// <summary>
        /// Gets or sets the tree of file medias.
        /// </summary>
        /// <value>
        /// The tree of file medias.
        /// </value>
        [DataMember(Order = 10, Name = "filesTree")]
        public IList<MediaModel> FilesTree { get; set; }
    }
}