using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Users.Users.User
{
    [DataContract]
    [Serializable]
    public class SaveUserModel : SaveModelBase
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
        /// Gets or sets the password.
        /// </summary>
        /// <value>
        /// The password.
        /// </value>
        [DataMember]
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the user image id.
        /// </summary>
        /// <value>
        /// The user image id.
        /// </value>
        [DataMember]
        public Guid? ImageId { get; set; }

        /// <summary>
        /// Gets or sets the list of user roles.
        /// </summary>
        /// <value>
        /// The list of user roles.
        /// </value>
        [DataMember]
        public List<string> Roles { get; set; }
    }
}