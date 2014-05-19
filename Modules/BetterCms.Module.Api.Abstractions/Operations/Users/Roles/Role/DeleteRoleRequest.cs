using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Users.Roles.Role
{
    /// <summary>
    /// Role delete request for REST.
    /// </summary>
    [Route("/roles/{RoleId}", Verbs = "DELETE")]
    [DataContract]
    [Serializable]
    public class DeleteRoleRequest : DeleteRequestBase, IReturn<DeleteRoleResponse>
    {
        /// <summary>
        /// Gets or sets the role identifier.
        /// </summary>
        /// <value>
        /// The role identifier.
        /// </value>
        [DataMember]
        public Guid RoleId { get; set; }
    }
}