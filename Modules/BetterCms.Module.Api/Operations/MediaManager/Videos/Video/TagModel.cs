using System.Runtime.Serialization;

namespace BetterCms.Module.Api.Operations.MediaManager.Videos.Video
{
    [DataContract]
    public class TagModel : ModelBase
    {
        /// <summary>
        /// Gets or sets the tag name.
        /// </summary>
        /// <value>
        /// The tag name.
        /// </value>
        [DataMember]
        public string Name { get; set; }
    }
}