using System.Runtime.Serialization;

namespace BetterCms.Module.Api.Operations.MediaManager.GetFileById
{
    [DataContract]
    public class GetFileByIdResponse : ResponseBase<MediaModel>
    {
        /// <summary>
        /// Gets or sets the list of file tags.
        /// </summary>
        /// <value>
        /// The list of file tags.
        /// </value>
        [DataMember(Order = 10, Name = "tags")]
        private System.Collections.Generic.IList<TagModel> Tags { get; set; }
    }
}