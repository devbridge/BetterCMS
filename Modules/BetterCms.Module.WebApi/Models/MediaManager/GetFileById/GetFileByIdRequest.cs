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

        /// <summary>
        /// Gets or sets a value indicating whether to include tags to response.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to include tags to response; otherwise, <c>false</c>.
        /// </value>
        [DataMember(Order = 20, Name = "includeTags")]
        public bool IncludeTags { get; set; }
    }
}