using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.MediaManager.Files.File
{
    [DataContract]
    public class GetFileResponse : ResponseBase<FileModel>
    {
        /// <summary>
        /// Gets or sets the list of file tags.
        /// </summary>
        /// <value>
        /// The list of file tags.
        /// </value>
        [DataMember]
        public System.Collections.Generic.IList<TagModel> Tags { get; set; }
    }
}