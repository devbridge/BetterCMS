using System.Runtime.Serialization;

namespace BetterCms.Module.Api.Operations.MediaManager.Files.File
{
    [DataContract]
    public class GetFileModel
    {
        /// <summary>
        /// Gets or sets a value indicating whether to include tags to response.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to include tags to response; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool IncludeTags { get; set; }
    }
}