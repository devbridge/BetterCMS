using System.Collections.Generic;
using System.Runtime.Serialization;

using BetterCms.Module.MediaManager.Models;

namespace BetterCms.Module.WebApi.Models.MediaManager.GetMediaTree
{
    [DataContract]
    public class MediaModel : ModelBase
    {
        /// <summary>
        /// Gets or sets the media title.
        /// </summary>
        /// <value>
        /// The media title.
        /// </value>
        [DataMember(Order = 10, Name = "title")]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the type of the media content.
        /// </summary>
        /// <value>
        /// The type of the media content.
        /// </value>
        [DataMember(Order = 20, Name = "mediaContentType")]
        public MediaContentType MediaContentType { get; set; }

        /// <summary>
        /// Gets or sets the URL.
        /// </summary>
        /// <value>
        /// The URL.
        /// </value>
        [DataMember(Order = 50, Name = "url")]
        public string url { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether media is archived.
        /// </summary>
        /// <value>
        /// <c>true</c> if media is archived; otherwise, <c>false</c>.
        /// </value>
        [DataMember(Order = 60, Name = "isArchived")]
        public bool IsArchived { get; set; }

        /// <summary>
        /// Gets or sets the list of media children.
        /// </summary>
        /// <value>
        /// The list of media children.
        /// </value>
        [DataMember(Order = 70, Name = "children")]
        public IList<MediaModel> Children { get; set; }
    }
}