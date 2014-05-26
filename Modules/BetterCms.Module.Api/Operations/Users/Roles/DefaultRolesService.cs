using BetterCms.Core.Exceptions;

namespace BetterCms.Module.Api.Operations.Users.Roles
{
    public class DefaultRolesService : IRolesService
    {
        public GetRolesResponse Get(GetRolesRequest request)
        {
            throw new CmsException(UsersApiConstants.UsersApiHasNoImplementationMessage);
        }

        public PostRoleResponse Post(PostRoleRequest request)
        {
            throw new CmsException(UsersApiConstants.UsersApiHasNoImplementationMessage);
        }
    }
}