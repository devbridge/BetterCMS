using System.Runtime.Serialization;

namespace BetterCms.Module.Api.Operations.MediaManager.GetFileById
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
        [DataMember(Order = 50, Name = "fileUrl")]
        public string FileUrl { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether media is archived.
        /// </summary>
        /// <value>
        /// <c>true</c> if media is archived; otherwise, <c>false</c>.
        /// </value>
        [DataMember(Order = 60, Name = "isArchived")]
        public bool IsArchived { get; set; }

        /// <summary>
        /// Gets or sets the folder id.
        /// </summary>
        /// <value>
        /// The folder id.
        /// </value>
        [DataMember(Order = 70, Name = "folderId")]
        public System.Guid? FolderId { get; set; }

        /// <summary>
        /// Gets or sets the name of the folder.
        /// </summary>
        /// <value>
        /// The name of the folder.
        /// </value>
        [DataMember(Order = 80, Name = "folderName")]
        public string FolderName { get; set; }

        /// <summary>
        /// Gets or sets the date, when media was published on.
        /// </summary>
        /// <value>
        /// The published on.
        /// </value>
        [DataMember(Order = 90, Name = "publishedOn")]
        public System.DateTime PublishedOn { get; set; }

        /// <summary>
        /// Gets or sets the name of the original file.
        /// </summary>
        /// <value>
        /// The name of the original file.
        /// </value>
        [DataMember(Order = 100, Name = "originalFileName")]
        public virtual string OriginalFileName { get; set; }

        /// <summary>
        /// Gets or sets the original file extension.
        /// </summary>
        /// <value>
        /// The original file extension.
        /// </value>
        [DataMember(Order = 110, Name = "originalFileExtension")]
        public virtual string OriginalFileExtension { get; set; }
    }
}