using BetterCms.Core.Exceptions;

using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Api.Operations.Users.Roles.Role
{
    public class DefaultRoleService : Service, IRoleService
    {
        public GetRoleResponse Get(GetRoleRequest request)
        {
            throw new CmsException(UsersApiConstants.UsersApiHasNoImplementationMessage);
        }
    }
}