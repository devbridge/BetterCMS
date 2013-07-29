using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;
using BetterCms.Module.Api.Infrastructure.Enums;

namespace BetterCms.Module.Api.Operations.MediaManager.Files
{
    [DataContract]
    public class GetFilesModel : DataOptions, IFilterByTags
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetFilesModel" /> class.
        /// </summary>
        public GetFilesModel()
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
        [DataMember]
        public System.Guid? FolderId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to include files.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to include files; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool IncludeFiles { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to include archived medias.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to include archived medias; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool IncludeArchived { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to include folders.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to include folders; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool IncludeFolders { get; set; }

        /// <summary>
        /// Gets or sets the file tags for filtering.
        /// </summary>
        /// <value>
        /// The file tags for filtering.
        /// </value>
        [DataMember]
        public System.Collections.Generic.List<string> FilterByTags { get; set; }

        /// <summary>
        /// Gets or sets the tags filter connector.
        /// </summary>
        /// <value>
        /// The tags filter connector.
        /// </value>
        [DataMember]
        public FilterConnector FilterByTagsConnector { get; set; }
    }
}