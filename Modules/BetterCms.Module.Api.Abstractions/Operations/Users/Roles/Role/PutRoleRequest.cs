using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Users.Roles.Role
{
    /// <summary>
    /// Request for role update.
    /// </summary>
    [Route("/roles/{Id}", Verbs = "PUT")]
    [DataContract]
    [Serializable]
    public class PutRoleRequest : PutRequestBase<SaveRoleModel>, IReturn<PutRoleResponse>
    {
    }
}
