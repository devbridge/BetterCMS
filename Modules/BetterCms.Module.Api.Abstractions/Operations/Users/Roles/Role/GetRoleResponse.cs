using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Users.Roles.Role
{
    [DataContract]
    [Serializable]
    public class GetRoleResponse : ResponseBase<RoleModel>
    {
    }
}