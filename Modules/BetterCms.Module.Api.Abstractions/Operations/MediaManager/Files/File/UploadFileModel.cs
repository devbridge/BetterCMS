using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;
using BetterCms.Module.Api.Operations.Root;

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

        /// <summary>
        /// Gets or sets a value indicating whether to wait for upload result or not.
        /// </summary>
        /// <value>
        /// <c>true</c> if to wait for upload result; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool WaitForUploadResult { get; set; }

        /// <summary>
        /// Gets or sets the access rules.
        /// </summary>
        /// <value>
        /// The access rules.
        /// </value>
        [DataMember]
        public IList<AccessRuleModel> AccessRules { get; set; }
    }
}
