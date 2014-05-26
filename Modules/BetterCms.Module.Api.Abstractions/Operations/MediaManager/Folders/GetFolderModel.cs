using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.MediaManager.Folders
{
    /// <summary>
    /// Data model to get folders.
    /// </summary>
    [DataContract]
    [System.Serializable]
    public class GetFolderModel : DataOptions
    {
        /// <summary>
        /// Gets or sets the folder id.
        /// </summary>
        /// <value>
        /// The folder id.
        /// </value>
        [DataMember]
        public System.Guid? ParentFolderId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to include archived folders.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to include archived folders; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool IncludeArchived { get; set; }
    }
}