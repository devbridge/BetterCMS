using System.Collections.Generic;
using System.Runtime.Serialization;

namespace BetterCms.Module.WebApi.Models.MediaManager.GetMediaTree
{
    [DataContract]
    public class MediaTreeContainer
    {
        /// <summary>
        /// Gets or sets the list of image medias.
        /// </summary>
        /// <value>
        /// The image medias.
        /// </value>
        [DataMember(Order = 10, Name = "imageMedias")]
        public IList<MediaModel> ImageMedias { get; set; }

        /// <summary>
        /// Gets or sets the list of video medias.
        /// </summary>
        /// <value>
        /// The video medias.
        /// </value>
        [DataMember(Order = 10, Name = "videoMedias")]
        public IList<MediaModel> VideoMedias { get; set; }

        /// <summary>
        /// Gets or sets the list of file medias.
        /// </summary>
        /// <value>
        /// The file medias.
        /// </value>
        [DataMember(Order = 10, Name = "fileMedias")]
        public IList<MediaModel> Files { get; set; }
    }
}