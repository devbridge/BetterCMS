using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Users.Users.User
{
    [DataContract]
    [Serializable]
    public class GetUserResponse : ResponseBase<UserModel>
    {
        /// <summary>
        /// Gets or sets the list of user roles.
        /// </summary>
        /// <value>
        /// The list of user roles.
        /// </value>
        [DataMember]
        public List<RoleModel> Roles { get; set; }
    }
}