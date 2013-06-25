using System.Runtime.Serialization;

namespace BetterCms.Module.WebApi.Models.MediaManager
{
    [DataContract]
    public class FileModel : MediaModelBase
    {
        /// <summary>
        /// Gets or sets the file extension.
        /// </summary>
        /// <value>
        /// The file extension.
        /// </value>
        [DataMember(Order = 210, Name = "fileExtension")]
        public string FileExtension { get; set; }

        /// <summary>
        /// Gets or sets the size of the file.
        /// </summary>
        /// <value>
        /// The size of the file.
        /// </value>
        [DataMember(Order = 210, Name = "fileSize")]
        public long FileSize { get; set; }

        /// <summary>
        /// Gets or sets the URL.
        /// </summary>
        /// <value>
        /// The URL.
        /// </value>
        [DataMember(Order = 220, Name = "url")]
        public string Url { get; set; }
    }
}