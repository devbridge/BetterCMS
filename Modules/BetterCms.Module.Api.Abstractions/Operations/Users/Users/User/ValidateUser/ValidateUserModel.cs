using System;
using System.Runtime.Serialization;

namespace BetterCms.Module.Api.Operations.Users.Users.User.ValidateUser
{
    [DataContract]
    [Serializable]
    public class ValidateUserModel
    {
        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        /// <value>
        /// The name of the user.
        /// </value>
        [DataMember]
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>
        /// The password.
        /// </value>
        [DataMember]
        public string Password { get; set; }
    }
}
