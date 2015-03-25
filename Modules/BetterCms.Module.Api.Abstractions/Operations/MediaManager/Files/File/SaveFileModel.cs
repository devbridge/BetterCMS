using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;
using BetterCms.Module.Api.Operations.Root;

namespace BetterCms.Module.Api.Operations.MediaManager.Files.File
{
    [DataContract]
    [Serializable]
    public class SaveFileModel : SaveModelBase
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
        /// Gets or sets the tags.
        /// </summary>
        /// <value>
        /// The tags.
        /// </value>
        [DataMember]
        public IList<string> Tags { get; set; }

        /// <summary>
        /// Gets or sets the public URL.
        /// </summary>
        /// <value>
        /// The public URL.
        /// </value>
        [DataMember]
        public string PublicUrl { get; set; }

        /// <summary>
        /// Gets or sets the access rules.
        /// </summary>
        /// <value>
        /// The access rules.
        /// </value>
        [DataMember]
        public IList<AccessRuleModel> AccessRules { get; set; }

        /// <summary>
        /// Gets or sets the folder identifier.
        /// </summary>
        /// <value>
        /// The folder identifier.
        /// </value>
        [DataMember]
        public Guid? FolderId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is archived.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is archived; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool IsArchived { get; set; }

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
        /// Gets or sets the file URI.
        /// </summary>
        /// <value>
        /// The file URI.
        /// </value>
        [DataMember]
        public string FileUri { get; set; }

        /// <summary>
        /// Gets or sets the size of the file.
        /// </summary>
        /// <value>
        /// The size of the file.
        /// </value>
        [DataMember]
        public long FileSize { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is temporary.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is temporary; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool IsTemporary { get; set; }

        /// <summary>
        /// Gets or sets the is uploaded.
        /// </summary>
        /// <value>
        /// The is uploaded.
        /// </value>
        [DataMember]
        public bool? IsUploaded { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is canceled.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is canceled; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool IsCanceled { get; set; }

        /// <summary>
        /// Gets or sets the thumbnail identifier.
        /// </summary>
        /// <value>
        /// The thumbnail identifier.
        /// </value>
        [DataMember]
        public Guid? ThumbnailId { get; set; }

        /// <summary>
        /// Gets or sets the Categories.
        /// </summary>
        /// <value>
        /// The tags.
        /// </value>
        [DataMember]
        public IList<Guid> Categories { get; set; }
    }
}