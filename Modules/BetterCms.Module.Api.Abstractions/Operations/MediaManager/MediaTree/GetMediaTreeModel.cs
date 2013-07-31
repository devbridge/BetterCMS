using System.Runtime.Serialization;

namespace BetterCms.Module.Api.Operations.MediaManager.MediaTree
{
    [DataContract]
    public class GetMediaTreeModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetMediaTreeModel" /> class.
        /// </summary>
        public GetMediaTreeModel()
        {
            IncludeImagesTree = true;
            IncludeFilesTree = true;
        }

        /// <summary>
        /// Gets or sets a value indicating whether to include images tree.
        /// </summary>
        /// <value>
        ///   <c>true</c> if include images tree; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool IncludeImagesTree { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to include the files of images tree.
        /// </summary>
        /// <value>
        ///   <c>true</c> if include the files of images tree; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool IncludeImages { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to include files tree.
        /// </summary>
        /// <value>
        ///   <c>true</c> if include files tree; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool IncludeFilesTree { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to include the files of files tree.
        /// </summary>
        /// <value>
        ///   <c>true</c> if include the files of files tree; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool IncludeFiles { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to include archived medias to response.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to include archived items to response; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool IncludeArchived { get; set; }
    }
}