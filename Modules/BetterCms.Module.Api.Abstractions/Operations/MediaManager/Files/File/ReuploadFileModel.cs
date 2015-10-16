using System;
using System.IO;
using System.Runtime.Serialization;

namespace BetterCms.Module.Api.Operations.MediaManager.Files.File
{
    /// <summary>
    /// The upload file model.
    /// </summary>
    [DataContract]
    [Serializable]
    public class ReuploadFileModel
    {
        /// <summary>
        /// Gets or sets file id.
        /// </summary>
        [DataMember]
        public Guid Id { get; set; }

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

        /// <summary>
        /// Gets or sets a value indicating whether to wait for upload result or not.
        /// </summary>
        /// <value>
        /// <c>true</c> if to wait for upload result; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool WaitForUploadResult { get; set; }
    }
}