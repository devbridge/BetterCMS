using System;
using System.IO;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.MediaManager.Files.File
{
    /// <summary>
    /// The upload file model.
    /// </summary>
    [DataContract]
    [Serializable]
    public class UploadFileModel: SaveModelBase
    {
        /// <summary>
        /// Gets or sets file id.
        /// </summary>
        [DataMember]
        public Guid? Id { get; set; }

        /// <summary>
        /// Gets or sets folder id.
        /// </summary>
        [DataMember]
        public Guid? FolderId { get; set; }

        /// <summary>
        /// Gets or sets file title.
        /// </summary>
        [DataMember]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets file description.
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets file name.
        /// </summary>
        [DataMember]
        public string FileName { get; set; }

        /// <summary>
        /// Gets or sets file stream.
        /// </summary>
        [DataMember]
        public Stream FileStream { get; set; }
    }
}
