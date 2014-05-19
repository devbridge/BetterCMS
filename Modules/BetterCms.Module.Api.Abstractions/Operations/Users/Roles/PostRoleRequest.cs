using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;
using BetterCms.Module.Api.Operations.Users.Roles.Role;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Users.Roles
{
    /// <summary>
    /// Request for role creation.
    /// </summary>
    [Route("/roles/", Verbs = "POST")]
    [DataContract]
    [Serializable]
    public class PostRoleRequest : RequestBase<SaveRoleModel>, IReturn<PostRoleResponse>
    {
    }
}
