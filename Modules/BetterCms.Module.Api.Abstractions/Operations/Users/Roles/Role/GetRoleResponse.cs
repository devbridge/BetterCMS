using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Users.Roles.Role
{
    [DataContract]
    public class GetRoleResponse : ResponseBase<RoleModel>
    {
    }
}