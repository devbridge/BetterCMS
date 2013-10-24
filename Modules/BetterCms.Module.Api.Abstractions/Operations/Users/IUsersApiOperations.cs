using BetterCms.Module.Api.Operations.Users.Roles;
using BetterCms.Module.Api.Operations.Users.Roles.Role;
using BetterCms.Module.Api.Operations.Users.Users;
using BetterCms.Module.Api.Operations.Users.Users.User;

namespace BetterCms.Module.Api.Operations.Users
{
    public interface IUsersApiOperations
    {
        IUsersService Users { get; }

        IUserService User { get; }

        IRolesService Roles { get; }

        IRoleService Role { get; }
    }
}
