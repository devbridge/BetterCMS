using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Users.Roles.Role
{
    /// <summary>
    /// Role delete request for REST.
    /// </summary>
    [Route("/roles/{Id}", Verbs = "DELETE")]
    [DataContract]
    [Serializable]
    public class DeleteRoleRequest : DeleteRequestBase, IReturn<DeleteRoleResponse>
    {
    }
}