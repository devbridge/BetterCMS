using System.Runtime.Serialization;

using BetterCms.Module.WebApi.Models.Enums;

namespace BetterCms.Module.WebApi.Models.MediaManager.GetImages
{
    [DataContract]
    public class GetImagesRequest : ListRequestBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetImagesRequest" /> class.
        /// </summary>
        public GetImagesRequest()
        {
            IncludeFolders = true;
            IncludeImages = true;

            TagsConnector = FilterConnector.And;
        }

        /// <summary>
        /// Gets or sets the folder id.
        /// </summary>
        /// <value>
        /// The folder id.
        /// </value>
        [DataMember(Order = 10, Name = "folderId")]
        public System.Guid? FolderId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to include images.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to include images; otherwise, <c>false</c>.
        /// </value>
        [DataMember(Order = 20, Name = "includeImages")]
        public bool IncludeImages { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to include archived medias.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to include archived medias; otherwise, <c>false</c>.
        /// </value>
        [DataMember(Order = 30, Name = "includeArchived")]
        public bool IncludeArchived { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to include folders.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to include folders; otherwise, <c>false</c>.
        /// </value>
        [DataMember(Order = 40, Name = "includeFolders")]
        public bool IncludeFolders { get; set; }

        /// <summary>
        /// Gets or sets the image tags for filtering.
        /// </summary>
        /// <value>
        /// The image tags for filtering.
        /// </value>
        [DataMember(Order = 50, Name = "imageTags")]
        public System.Collections.Generic.List<string> ImageTags { get; set; }

        /// <summary>
        /// Gets or sets the tags filter connector.
        /// </summary>
        /// <value>
        /// The tags filter connector.
        /// </value>
        [DataMember(Order = 60, Name = "tagsConnector")]
        public FilterConnector TagsConnector { get; set; }
    }
}
