using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;
using BetterCms.Module.Api.Operations.Users.Roles.Role;

namespace BetterCms.Module.Api.Operations.Users.Roles
{
    [DataContract]
    [Serializable]
    public class GetRolesResponse : ListResponseBase<RoleModel>
    {
    }
}