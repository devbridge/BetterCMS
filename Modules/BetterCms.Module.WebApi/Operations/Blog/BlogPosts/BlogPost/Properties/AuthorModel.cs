using System;
using System.Runtime.Serialization;

namespace BetterCms.Module.Api.Operations.Blog.BlogPosts.BlogPost.Properties
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
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the image id.
        /// </summary>
        /// <value>
        /// The image id.
        /// </value>
        [DataMember]
        public Guid ImageId { get; set; }

        /// <summary>
        /// Gets or sets the image URL.
        /// </summary>
        /// <value>
        /// The image URL.
        /// </value>
        [DataMember]
        public string ImageUrl { get; set; }

        /// <summary>
        /// Gets or sets the image thumbnail URL.
        /// </summary>
        /// <value>
        /// The image thumbnail URL.
        /// </value>
        [DataMember]
        public string ImageThumbnailUrl { get; set; }
    }
}