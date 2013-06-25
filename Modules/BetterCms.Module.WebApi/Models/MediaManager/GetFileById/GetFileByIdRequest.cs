using System.Runtime.Serialization;

namespace BetterCms.Module.WebApi.Models.MediaManager.GetFileById
{
    [DataContract]
    public class GetFileByIdRequest : RequestBase
    {
        /// <summary>
        /// Gets or sets the file id.
        /// </summary>
        /// <value>
        /// The file id.
        /// </value>
        [DataMember(Order = 10, Name = "fileId")]
        public System.Guid FileId { get; set; }
    }
}