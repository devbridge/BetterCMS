using System;
using System.Runtime.Serialization;

namespace BetterCms.Module.Api.Operations.Blog.GetAuthors
{
    [DataContract]
    public class AuthorModel : ModelBase
    {
        /// <summary>
        /// Gets or sets the author name.
        /// </summary>
        /// <value>
        /// The author name.
        /// </value>
        [DataMember(Order = 10, Name = "name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the image id.
        /// </summary>
        /// <value>
        /// The image id.
        /// </value>
        [DataMember(Order = 20, Name = "imageId")]
        public Guid ImageId { get; set; }

        /// <summary>
        /// Gets or sets the image URL.
        /// </summary>
        /// <value>
        /// The image URL.
        /// </value>
        [DataMember(Order = 30, Name = "imagePublicUrl")]
        public string ImageUrl { get; set; }

        /// <summary>
        /// Gets or sets the image thumbnail URL.
        /// </summary>
        /// <value>
        /// The image thumbnail URL.
        /// </value>
        [DataMember(Order = 40, Name = "imageThumbnailUrl")]
        public string ImageThumbnailUrl { get; set; }
    }
}