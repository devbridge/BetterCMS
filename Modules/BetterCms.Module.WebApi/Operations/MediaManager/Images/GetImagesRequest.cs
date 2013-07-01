using System.Runtime.Serialization;

using BetterCms.Module.Api.Operations.Enums;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.MediaManager.Images
{
    [Route("/images", Verbs = "GET")]
    public class GetImagesRequest : ListRequestBase, IReturn<GetImagesResponse>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetImagesRequest" /> class.
        /// </summary>
        public GetImagesRequest()
        {
            IncludeFolders = true;
            IncludeImages = true;

            FilterByTagsConnector = FilterConnector.And;
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
        [DataMember(Order = 50, Name = "filterByTags")]
        public System.Collections.Generic.List<string> FilterByTags { get; set; }

        /// <summary>
        /// Gets or sets the tags filter connector.
        /// </summary>
        /// <value>
        /// The tags filter connector.
        /// </value>
        [DataMember(Order = 60, Name = "filterByTagsConnector")]
        public FilterConnector FilterByTagsConnector { get; set; }
    }
}
