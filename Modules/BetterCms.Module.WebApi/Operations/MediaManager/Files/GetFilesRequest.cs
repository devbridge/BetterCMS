using BetterCms.Module.Api.Operations.Enums;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.MediaManager.Files
{
    [Route("/files", Verbs = "GET")]
    public class GetFilesRequest : ListRequestBase, IReturn<GetFilesResponse>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetFilesRequest" /> class.
        /// </summary>
        public GetFilesRequest()
        {
            IncludeFolders = true;
            IncludeFiles = true;

            FilterByTagsConnector = FilterConnector.And;
        }

        /// <summary>
        /// Gets or sets the folder id.
        /// </summary>
        /// <value>
        /// The folder id.
        /// </value>
        public System.Guid? FolderId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to include files.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to include files; otherwise, <c>false</c>.
        /// </value>
        public bool IncludeFiles { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to include archived medias.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to include archived medias; otherwise, <c>false</c>.
        /// </value>
        public bool IncludeArchived { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to include folders.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to include folders; otherwise, <c>false</c>.
        /// </value>
        public bool IncludeFolders { get; set; }

        /// <summary>
        /// Gets or sets the file tags for filtering.
        /// </summary>
        /// <value>
        /// The file tags for filtering.
        /// </value>
        public System.Collections.Generic.List<string> FilterByTags { get; set; }

        /// <summary>
        /// Gets or sets the tags filter connector.
        /// </summary>
        /// <value>
        /// The tags filter connector.
        /// </value>
        public FilterConnector FilterByTagsConnector { get; set; }
    }
}
