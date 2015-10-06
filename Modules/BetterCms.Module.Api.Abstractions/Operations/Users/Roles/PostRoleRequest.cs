using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;
using BetterCms.Module.Api.Operations.Users.Roles.Role;

namespace BetterCms.Module.Api.Operations.Users.Roles
{
    /// <summary>
    /// Request for role creation.
    /// </summary>
    [DataContract]
    [Serializable]
    public class PostRoleRequest : RequestBase<SaveRoleModel>
    {
    }
}
