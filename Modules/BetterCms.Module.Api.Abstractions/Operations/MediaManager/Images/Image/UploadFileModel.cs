using System;
using System.IO;
using System.Runtime.Serialization;

namespace BetterCms.Module.Api.Operations.MediaManager.Images.Image
{
    /// <summary>
    /// The upload file model.
    /// </summary>
    [DataContract]
    [Serializable]
    public class UploadFileModel
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
