using System.Runtime.Serialization;

using BetterCms.Module.MediaManager.Models;

namespace BetterCms.Module.WebApi.Models.MediaManager.GetImages
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
        /// Gets or sets the file extension.
        /// </summary>
        /// <value>
        /// The file extension.
        /// </value>
        [DataMember(Order = 30, Name = "fileExtension")]
        public string FileExtension { get; set; }

        /// <summary>
        /// Gets or sets the size of the file.
        /// </summary>
        /// <value>
        /// The size of the file.
        /// </value>
        [DataMember(Order = 40, Name = "fileSize")]
        public long FileSize { get; set; }

        /// <summary>
        /// Gets or sets the URL.
        /// </summary>
        /// <value>
        /// The URL.
        /// </value>
        [DataMember(Order = 50, Name = "imageUrl")]
        public string ImageUrl { get; set; }

        /// <summary>
        /// Gets or sets the thumbnail URL.
        /// </summary>
        /// <value>
        /// The thumbnail URL.
        /// </value>
        [DataMember(Order = 60, Name = "thumbnailUrl")]
        public string ThumbnailUrl { get; set; }

        /// <summary>
        /// Gets or sets the image caption.
        /// </summary>
        /// <value>
        /// The image caption.
        /// </value>
        [DataMember(Order = 70, Name = "caption")]
        public string Caption { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether media is archived.
        /// </summary>
        /// <value>
        /// <c>true</c> if media is archived; otherwise, <c>false</c>.
        /// </value>
        [DataMember(Order = 80, Name = "isArchived")]
        public bool IsArchived { get; set; }
    }
}