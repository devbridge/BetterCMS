using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Users.Roles.Role
{
    [DataContract]
    [Serializable]
    public class SaveRoleModel : SaveModelBase
    {
        /// <summary>
        /// Gets or sets the name of the role.
        /// </summary>
        /// <value>
        /// The name of the role.
        /// </value>
        [DataMember]
        public string Name { get; set; }
        
        /// <summary>
        /// Gets or sets the description of the role.
        /// </summary>
        /// <value>
        /// The description of the role.
        /// </value>
        [DataMember]
        public string Description { get; set; }
    }
}