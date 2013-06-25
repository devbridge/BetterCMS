using System.Runtime.Serialization;

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
        }

        /// <summary>
        /// Gets or sets a value indicating whether to include images.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to include images; otherwise, <c>false</c>.
        /// </value>
        [DataMember(Order = 10, Name = "includeImages")]
        public bool IncludeImages { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to include folders.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to include folders; otherwise, <c>false</c>.
        /// </value>
        [DataMember(Order = 20, Name = "includeFolders")]
        public bool IncludeFolders { get; set; }

        /// <summary>
        /// Gets or sets the folder id.
        /// </summary>
        /// <value>
        /// The folder id.
        /// </value>
        [DataMember(Order = 30, Name = "folderId")]
        public System.Guid? FolderId { get; set; }
    }
}
