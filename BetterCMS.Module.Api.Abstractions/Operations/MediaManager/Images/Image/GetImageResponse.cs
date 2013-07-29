using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.MediaManager.Images.Image
{
    [DataContract]
    public class GetImageResponse : ResponseBase<ImageModel>
    {
        /// <summary>
        /// Gets or sets the list of image tags.
        /// </summary>
        /// <value>
        /// The list of image tags.
        /// </value>
        [DataMember]
        public System.Collections.Generic.IList<TagModel> Tags { get; set; }
    }
}