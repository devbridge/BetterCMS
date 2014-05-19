using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Users.Roles.Role
{
    /// <summary>
    /// Request for role update.
    /// </summary>
    [Route("/roles/{RoleId}", Verbs = "PUT")]
    [DataContract]
    [Serializable]
    public class PutRoleRequest : RequestBase<SaveRoleModel>, IReturn<PutRoleResponse>
    {
        /// <summary>
        /// Gets or sets the role identifier.
        /// </summary>
        /// <value>
        /// The role identifier.
        /// </value>
        [DataMember]
        public Guid? RoleId { get; set; }
    }
}
