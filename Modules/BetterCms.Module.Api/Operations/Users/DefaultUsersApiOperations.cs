using BetterCms.Module.Api.Operations.Users.Roles;
using BetterCms.Module.Api.Operations.Users.Roles.Role;
using BetterCms.Module.Api.Operations.Users.Users;
using BetterCms.Module.Api.Operations.Users.Users.User;

namespace BetterCms.Module.Api.Operations.Users
{
    public class DefaultUsersApiOperations : IUsersApiOperations
    {
        public DefaultUsersApiOperations(IUsersService users, IUserService user, IRolesService roles, IRoleService role)
        {
            Users = users;
            User = user;
            Roles = roles;
            Role = role;
        }

        public IUsersService Users
        {
            get; 
            private set;
        }
        
        public IUserService User
        {
            get; 
            private set;
        }
        
        public IRolesService Roles
        {
            get; 
            private set;
        }
        
        public IRoleService Role
        {
            get; 
            private set;
        }
    }
}
