using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Users.Users.User
{
    [DataContract]
    [Serializable]
    public class UserModel : ModelBase
    {
        /// <summary>
        /// Gets or sets the first name of user.
        /// </summary>
        /// <value>
        /// The first name of user.
        /// </value>
        [DataMember]
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name of user.
        /// </summary>
        /// <value>
        /// The last name of user.
        /// </value>
        [DataMember]
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        /// <value>
        /// The name of the user.
        /// </value>
        [DataMember]
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the user email.
        /// </summary>
        /// <value>
        /// The user email.
        /// </value>
        [DataMember]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the user image id.
        /// </summary>
        /// <value>
        /// The user image id.
        /// </value>
        [DataMember]
        public Guid? ImageId { get; set; }

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

        /// <summary>
        /// Gets or sets the image caption.
        /// </summary>
        /// <value>
        /// The image caption.
        /// </value>
        [DataMember]
        public string ImageCaption { get; set; }
    }
}