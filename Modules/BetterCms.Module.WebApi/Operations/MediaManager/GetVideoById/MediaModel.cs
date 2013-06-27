using System.Runtime.Serialization;

namespace BetterCms.Module.Api.Operations.MediaManager.GetVideoById
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
        /// Gets or sets a value indicating whether media is archived.
        /// </summary>
        /// <value>
        /// <c>true</c> if media is archived; otherwise, <c>false</c>.
        /// </value>
        [DataMember(Order = 20, Name = "isArchived")]
        public bool IsArchived { get; set; }

        /// <summary>
        /// Gets or sets the folder id.
        /// </summary>
        /// <value>
        /// The folder id.
        /// </value>
        [DataMember(Order = 30, Name = "folderId")]
        public System.Guid? FolderId { get; set; }

        /// <summary>
        /// Gets or sets the name of the folder.
        /// </summary>
        /// <value>
        /// The name of the folder.
        /// </value>
        [DataMember(Order = 40, Name = "folderName")]
        public string FolderName { get; set; }

        /// <summary>
        /// Gets or sets the date, when media was published on.
        /// </summary>
        /// <value>
        /// The published on.
        /// </value>
        [DataMember(Order = 50, Name = "publishedOn")]
        public System.DateTime PublishedOn { get; set; }
    }
}