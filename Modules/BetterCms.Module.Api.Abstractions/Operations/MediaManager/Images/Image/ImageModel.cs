using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;
using BetterCms.Module.Api.Operations.Root.Categories.Category;

namespace BetterCms.Module.Api.Operations.MediaManager.Images.Image
{
    [DataContract]
    [Serializable]
    public class ImageModel : ModelBase
    {
        /// <summary>
        /// Gets or sets the media title.
        /// </summary>
        /// <value>
        /// The media title.
        /// </value>
        [DataMember]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the image caption.
        /// </summary>
        /// <value>
        /// The image caption.
        /// </value>
        [DataMember]
        public string Caption { get; set; }

        /// <summary>
        /// Gets or sets the file extension.
        /// </summary>
        /// <value>
        /// The file extension.
        /// </value>
        [DataMember]
        public string FileExtension { get; set; }

        /// <summary>
        /// Gets or sets the size of the file.
        /// </summary>
        /// <value>
        /// The size of the file.
        /// </value>
        [DataMember]
        public long FileSize { get; set; }

        /// <summary>
        /// Gets or sets the URL.
        /// </summary>
        /// <value>
        /// The URL.
        /// </value>
        [DataMember]
        public string ImageUrl { get; set; }

        /// <summary>
        /// Gets or sets the image width.
        /// </summary>
        /// <value>
        /// The image width.
        /// </value>
        [DataMember]
        public virtual int Width { get; set; }

        /// <summary>
        /// Gets or sets the image height.
        /// </summary>
        /// <value>
        /// The image height.
        /// </value>
        [DataMember]
        public virtual int Height { get; set; }

        /// <summary>
        /// Gets or sets the thumbnail URL.
        /// </summary>
        /// <value>
        /// The thumbnail URL.
        /// </value>
        [DataMember]
        public string ThumbnailUrl { get; set; }

        /// <summary>
        /// Gets or sets the width of the thumbnail image.
        /// </summary>
        /// <value>
        /// The width of the thumbnail image.
        /// </value>
        [DataMember]
        public virtual int ThumbnailWidth { get; set; }

        /// <summary>
        /// Gets or sets the height of the thumbnail image.
        /// </summary>
        /// <value>
        /// The height of the thumbnail image.
        /// </value>
        [DataMember]
        public virtual int ThumbnailHeight { get; set; }

        /// <summary>
        /// Gets or sets the size of the thumbnail image.
        /// </summary>
        /// <value>
        /// The size of the thumbnail image.
        /// </value>
        [DataMember]
        public virtual long ThumbnailSize { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether media is archived.
        /// </summary>
        /// <value>
        /// <c>true</c> if media is archived; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool IsArchived { get; set; }

        /// <summary>
        /// Gets or sets the folder id.
        /// </summary>
        /// <value>
        /// The folder id.
        /// </value>
        [DataMember]
        public System.Guid? FolderId { get; set; }

        /// <summary>
        /// Gets or sets the name of the folder.
        /// </summary>
        /// <value>
        /// The name of the folder.
        /// </value>
        [DataMember]
        public string FolderName { get; set; }

        /// <summary>
        /// Gets or sets the date, when media was published on.
        /// </summary>
        /// <value>
        /// The published on.
        /// </value>
        [DataMember]
        public System.DateTime PublishedOn { get; set; }

        /// <summary>
        /// Gets or sets the name of the original file.
        /// </summary>
        /// <value>
        /// The name of the original file.
        /// </value>
        [DataMember]
        public virtual string OriginalFileName { get; set; }

        /// <summary>
        /// Gets or sets the original image file extension.
        /// </summary>
        /// <value>
        /// The original image file extension.
        /// </value>
        [DataMember]
        public virtual string OriginalFileExtension { get; set; }

        /// <summary>
        /// Gets or sets the width of the original image.
        /// </summary>
        /// <value>
        /// The width of the original image.
        /// </value>
        [DataMember]
        public virtual int OriginalWidth { get; set; }

        /// <summary>
        /// Gets or sets the height of the original image.
        /// </summary>
        /// <value>
        /// The height of the original image.
        /// </value>
        [DataMember]
        public virtual int OriginalHeight { get; set; }

        /// <summary>
        /// Gets or sets the size of the original image.
        /// </summary>
        /// <value>
        /// The size of the original image.
        /// </value>
        [DataMember]
        public virtual long OriginalSize { get; set; }

        /// <summary>
        /// Gets or sets the original image URL.
        /// </summary>
        /// <value>
        /// The original image URL.
        /// </value>
        [DataMember]
        public virtual string OriginalUrl { get; set; }

        /// <summary>
        /// Gets or sets the file URI.
        /// </summary>
        /// <value>
        /// The file URI.
        /// </value>
        [DataMember]
        public virtual string FileUri { get; set; }

        /// <summary>
        /// Gets or sets the is uploaded.
        /// </summary>
        /// <value>
        /// The is uploaded.
        /// </value>
        [DataMember]
        public virtual bool? IsUploaded { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is temporary.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is temporary; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public virtual bool IsTemporary { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is canceled.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is canceled; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public virtual bool IsCanceled { get; set; }

        /// <summary>
        /// Gets or sets the original URI.
        /// </summary>
        /// <value>
        /// The original URI.
        /// </value>
        [DataMember]
        public virtual string OriginalUri { get; set; }

        /// <summary>
        /// Gets or sets the thumbnail URI.
        /// </summary>
        /// <value>
        /// The thumbnail URI.
        /// </value>
        [DataMember]
        public virtual string ThumbnailUri { get; set; }

        [DataMember]
        public IList<CategoryNodeModel> Categories { get; set; }
    }
}