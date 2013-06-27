using System.Runtime.Serialization;

namespace BetterCms.Module.Api.Operations.Blog.GetBlogPostPropertiesById
{
    [DataContract]
    public class ImageModel : ModelBase
    {
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        [DataMember(Order = 10, Name = "title")]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the caption.
        /// </summary>
        /// <value>
        /// The caption.
        /// </value>
        [DataMember(Order = 20, Name = "caption")]
        public string Caption { get; set; }

        /// <summary>
        /// Gets or sets the URL.
        /// </summary>
        /// <value>
        /// The URL.
        /// </value>
        [DataMember(Order = 30, Name = "url")]
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets the thumbnail URL.
        /// </summary>
        /// <value>
        /// The thumbnail URL.
        /// </value>
        [DataMember(Order = 40, Name = "thumbnailUrl")]
        public string ThumbnailUrl { get; set; }
    }
}