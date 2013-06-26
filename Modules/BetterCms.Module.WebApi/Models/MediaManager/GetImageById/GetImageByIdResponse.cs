using System.Runtime.Serialization;

namespace BetterCms.Module.WebApi.Models.MediaManager.GetImageById
{
    [DataContract]
    public class GetImageByIdResponse : ResponseBase<MediaModel>
    {
        /// <summary>
        /// Gets or sets the list of image tags.
        /// </summary>
        /// <value>
        /// The list of image tags.
        /// </value>
        [DataMember(Order = 10, Name = "tags")]
        private System.Collections.Generic.IList<TagModel> Tags { get; set; }
    }
}