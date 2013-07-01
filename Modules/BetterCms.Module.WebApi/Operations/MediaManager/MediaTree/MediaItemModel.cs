using System.Collections.Generic;
using System.Runtime.Serialization;

using BetterCms.Module.MediaManager.Models;

namespace BetterCms.Module.Api.Operations.MediaManager.MediaTree
{
    [DataContract]
    public class MediaItemModel : ModelBase
    {
        /// <summary>
        /// Gets or sets the media title.
        /// </summary>
        /// <value>
        /// The media title.
        /// </value>
        [DataMember]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the type of the media content.
        /// </summary>
        /// <value>
        /// The type of the media content.
        /// </value>
        [DataMember]
        public MediaContentType MediaContentType { get; set; }

        /// <summary>
        /// Gets or sets the URL.
        /// </summary>
        /// <value>
        /// The URL.
        /// </value>
        [DataMember]
        public string url { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether media is archived.
        /// </summary>
        /// <value>
        /// <c>true</c> if media is archived; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool IsArchived { get; set; }

        /// <summary>
        /// Gets or sets the list of media children.
        /// </summary>
        /// <value>
        /// The list of media children.
        /// </value>
        [DataMember]
        public IList<MediaItemModel> Children { get; set; }
    }
}