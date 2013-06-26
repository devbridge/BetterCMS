using System.Runtime.Serialization;

namespace BetterCms.Module.WebApi.Models.MediaManager.GetImageById
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
        /// Gets or sets the image caption.
        /// </summary>
        /// <value>
        /// The image caption.
        /// </value>
        [DataMember(Order = 20, Name = "caption")]
        public string Caption { get; set; }

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
        /// Gets or sets the image width.
        /// </summary>
        /// <value>
        /// The image width.
        /// </value>
        [DataMember(Order = 60, Name = "width")]
        public virtual int Width { get; set; }

        /// <summary>
        /// Gets or sets the image height.
        /// </summary>
        /// <value>
        /// The image height.
        /// </value>
        [DataMember(Order = 70, Name = "height")]
        public virtual int Height { get; set; }

        /// <summary>
        /// Gets or sets the thumbnail URL.
        /// </summary>
        /// <value>
        /// The thumbnail URL.
        /// </value>
        [DataMember(Order = 80, Name = "thumbnailUrl")]
        public string ThumbnailUrl { get; set; }

        /// <summary>
        /// Gets or sets the width of the thumbnail image.
        /// </summary>
        /// <value>
        /// The width of the thumbnail image.
        /// </value>
        [DataMember(Order = 90, Name = "thumbnailWidth")]
        public virtual int ThumbnailWidth { get; set; }

        /// <summary>
        /// Gets or sets the height of the thumbnail image.
        /// </summary>
        /// <value>
        /// The height of the thumbnail image.
        /// </value>
        [DataMember(Order = 100, Name = "thumbnailHeight")]
        public virtual int ThumbnailHeight { get; set; }

        /// <summary>
        /// Gets or sets the size of the thumbnail image.
        /// </summary>
        /// <value>
        /// The size of the thumbnail image.
        /// </value>
        [DataMember(Order = 110, Name = "thumbnailSize")]
        public virtual long ThumbnailSize { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether media is archived.
        /// </summary>
        /// <value>
        /// <c>true</c> if media is archived; otherwise, <c>false</c>.
        /// </value>
        [DataMember(Order = 120, Name = "isArchived")]
        public bool IsArchived { get; set; }

        /// <summary>
        /// Gets or sets the folder id.
        /// </summary>
        /// <value>
        /// The folder id.
        /// </value>
        [DataMember(Order = 130, Name = "folderId")]
        public System.Guid? FolderId { get; set; }

        /// <summary>
        /// Gets or sets the name of the folder.
        /// </summary>
        /// <value>
        /// The name of the folder.
        /// </value>
        [DataMember(Order = 140, Name = "folderName")]
        public string FolderName { get; set; }

        /// <summary>
        /// Gets or sets the date, when media was published on.
        /// </summary>
        /// <value>
        /// The published on.
        /// </value>
        [DataMember(Order = 150, Name = "publishedOn")]
        public System.DateTime PublishedOn { get; set; }

        /// <summary>
        /// Gets or sets the name of the original file.
        /// </summary>
        /// <value>
        /// The name of the original file.
        /// </value>
        [DataMember(Order = 160, Name = "originalFileName")]
        public virtual string OriginalFileName { get; set; }

        /// <summary>
        /// Gets or sets the original image file extension.
        /// </summary>
        /// <value>
        /// The original image file extension.
        /// </value>
        [DataMember(Order = 170, Name = "originalFileExtension")]
        public virtual string OriginalFileExtension { get; set; }

        /// <summary>
        /// Gets or sets the width of the original image.
        /// </summary>
        /// <value>
        /// The width of the original image.
        /// </value>
        [DataMember(Order = 180, Name = "originalWidth")]
        public virtual int OriginalWidth { get; set; }

        /// <summary>
        /// Gets or sets the height of the original image.
        /// </summary>
        /// <value>
        /// The height of the original image.
        /// </value>
        [DataMember(Order = 190, Name = "originalHeight")]
        public virtual int OriginalHeight { get; set; }

        /// <summary>
        /// Gets or sets the size of the original image.
        /// </summary>
        /// <value>
        /// The size of the original image.
        /// </value>
        [DataMember(Order = 200, Name = "originalSize")]
        public virtual long OriginalSize { get; set; }

        /// <summary>
        /// Gets or sets the original image URL.
        /// </summary>
        /// <value>
        /// The original image URL.
        /// </value>
        [DataMember(Order = 210, Name = "originalUrl")]
        public virtual string OriginalUrl { get; set; }
    }
}