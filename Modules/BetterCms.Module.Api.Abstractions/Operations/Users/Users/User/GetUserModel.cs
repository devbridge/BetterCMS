using System;
using System.Runtime.Serialization;

namespace BetterCms.Module.Api.Operations.Users.Users.User
{
    [DataContract]
    [Serializable]
    public class GetUserModel
    {
        /// <summary>
        /// Gets or sets a value indicating whether to include roles.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to include roles; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool IncludeRoles { get; set; }
    }
}
