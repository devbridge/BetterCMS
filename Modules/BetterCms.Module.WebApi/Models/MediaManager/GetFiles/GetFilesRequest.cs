using System.Runtime.Serialization;

namespace BetterCms.Module.WebApi.Models.MediaManager.GetFiles
{
    [DataContract]
    public class GetFilesRequest : ListRequestBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetFilesRequest" /> class.
        /// </summary>
        public GetFilesRequest()
        {
            IncludeFolders = true;
            IncludeFiles = true;
        }

        /// <summary>
        /// Gets or sets a value indicating whether to include files.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to include files; otherwise, <c>false</c>.
        /// </value>
        [DataMember(Order = 10, Name = "includeFiles")]
        public bool IncludeFiles { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to include folders.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to include folders; otherwise, <c>false</c>.
        /// </value>
        [DataMember(Order = 20, Name = "includeFolders")]
        public bool IncludeFolders { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to include archived medias.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to include archived medias; otherwise, <c>false</c>.
        /// </value>
        [DataMember(Order = 30, Name = "includeArchived")]
        public bool IncludeArchived { get; set; }

        /// <summary>
        /// Gets or sets the folder id.
        /// </summary>
        /// <value>
        /// The folder id.
        /// </value>
        [DataMember(Order = 40, Name = "folderId")]
        public System.Guid? FolderId { get; set; }
    }
}
