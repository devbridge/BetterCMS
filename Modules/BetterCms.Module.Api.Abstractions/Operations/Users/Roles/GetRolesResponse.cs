using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Users.Roles
{
    [DataContract]
    public class GetRolesResponse : ListResponseBase<RoleModel>
    {
    }
}