using BetterCms.Core.Exceptions;

namespace BetterCms.Module.Api.Operations.Users.Roles.Role
{
    public class DefaultRoleService : IRoleService
    {
        public GetRoleResponse Get(GetRoleRequest request)
        {
            throw new CmsException(UsersApiConstants.UsersApiHasNoImplementationMessage);
        }

        public PutRoleResponse Put(PutRoleRequest request)
        {
            throw new CmsException(UsersApiConstants.UsersApiHasNoImplementationMessage);
        }

        public DeleteRoleResponse Delete(DeleteRoleRequest request)
        {
            throw new CmsException(UsersApiConstants.UsersApiHasNoImplementationMessage);
        }
    }
}