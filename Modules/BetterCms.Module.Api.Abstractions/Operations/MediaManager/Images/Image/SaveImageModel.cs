using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.MediaManager.Images.Image
{
    /// <summary>
    /// The save image model.
    /// </summary>
    [DataContract]
    [Serializable]
    public class SaveImageModel : SaveModelBase
    {
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
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
        /// Gets or sets the caption.
        /// </summary>
        /// <value>
        /// The caption.
        /// </value>
        [DataMember]
        public string Caption { get; set; }

        /// <summary>
        /// Gets or sets the size of the file.
        /// </summary>
        /// <value>
        /// The size of the file.
        /// </value>
        [DataMember]
        public long FileSize { get; set; }

        /// <summary>
        /// Gets or sets the image URL.
        /// </summary>
        /// <value>
        /// The image URL.
        /// </value>
        [DataMember]
        public string ImageUrl { get; set; }

        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        /// <value>
        /// The width.
        /// </value>
        [DataMember]
        public int Width { get; set; }

        /// <summary>
        /// Gets or sets the height.
        /// </summary>
        /// <value>
        /// The height.
        /// </value>
        [DataMember]
        public int Height { get; set; }

        /// <summary>
        /// Gets or sets the thumbnail URL.
        /// </summary>
        /// <value>
        /// The thumbnail URL.
        /// </value>
        [DataMember]
        public string ThumbnailUrl { get; set; }

        /// <summary>
        /// Gets or sets the width of the thumbnail.
        /// </summary>
        /// <value>
        /// The width of the thumbnail.
        /// </value>
        [DataMember]
        public int ThumbnailWidth { get; set; }

        /// <summary>
        /// Gets or sets the height of the thumbnail.
        /// </summary>
        /// <value>
        /// The height of the thumbnail.
        /// </value>
        [DataMember]
        public int ThumbnailHeight { get; set; }

        /// <summary>
        /// Gets or sets the size of the thumbnail.
        /// </summary>
        /// <value>
        /// The size of the thumbnail.
        /// </value>
        [DataMember]
        public long ThumbnailSize { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is archived.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is archived; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool IsArchived { get; set; }

        /// <summary>
        /// Gets or sets the folder identifier.
        /// </summary>
        /// <value>
        /// The folder identifier.
        /// </value>
        [DataMember]
        public Guid? FolderId { get; set; }

        /// <summary>
        /// Gets or sets the published on.
        /// </summary>
        /// <value>
        /// The published on.
        /// </value>
        [DataMember]
        public DateTime PublishedOn { get; set; }

        /// <summary>
        /// Gets or sets the name of the original file.
        /// </summary>
        /// <value>
        /// The name of the original file.
        /// </value>
        [DataMember]
        public string OriginalFileName { get; set; }

        /// <summary>
        /// Gets or sets the original file extension.
        /// </summary>
        /// <value>
        /// The original file extension.
        /// </value>
        [DataMember]
        public string OriginalFileExtension { get; set; }

        /// <summary>
        /// Gets or sets the width of the original.
        /// </summary>
        /// <value>
        /// The width of the original.
        /// </value>
        [DataMember]
        public int OriginalWidth { get; set; }

        /// <summary>
        /// Gets or sets the height of the original.
        /// </summary>
        /// <value>
        /// The height of the original.
        /// </value>
        [DataMember]
        public int OriginalHeight { get; set; }

        /// <summary>
        /// Gets or sets the size of the original.
        /// </summary>
        /// <value>
        /// The size of the original.
        /// </value>
        [DataMember]
        public long OriginalSize { get; set; }

        /// <summary>
        /// Gets or sets the original URL.
        /// </summary>
        /// <value>
        /// The original URL.
        /// </value>
        [DataMember]
        public string OriginalUrl { get; set; }

        /// <summary>
        /// Gets or sets the file URI.
        /// </summary>
        /// <value>
        /// The file URI.
        /// </value>
        [DataMember]
        public string FileUri { get; set; }

        /// <summary>
        /// Gets or sets the is uploaded.
        /// </summary>
        /// <value>
        /// The is uploaded.
        /// </value>
        [DataMember]
        public bool? IsUploaded { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is temporary.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is temporary; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool IsTemporary { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is canceled.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is canceled; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool IsCanceled { get; set; }

        /// <summary>
        /// Gets or sets the original URI.
        /// </summary>
        /// <value>
        /// The original URI.
        /// </value>
        [DataMember]
        public string OriginalUri { get; set; }

        /// <summary>
        /// Gets or sets the thumbnail URI.
        /// </summary>
        /// <value>
        /// The thumbnail URI.
        /// </value>
        [DataMember]
        public string ThumbnailUri { get; set; }

        /// <summary>
        /// Gets or sets the tags.
        /// </summary>
        /// <value>
        /// The tags.
        /// </value>
        [DataMember]
        public IList<string> Tags { get; set; }

        /// <summary>
        /// Gets or sets the tags.
        /// </summary>
        /// <value>
        /// The tags.
        /// </value>
        [DataMember]
        public IList<Guid> Categories { get; set; }
    }
}