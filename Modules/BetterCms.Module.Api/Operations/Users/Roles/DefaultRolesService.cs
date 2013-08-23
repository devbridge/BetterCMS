using BetterCms.Core.Exceptions;

using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Api.Operations.Users.Roles
{
    public class DefaultRolesService : Service, IRolesService
    {
        public GetRolesResponse Get(GetRolesRequest request)
        {
            throw new CmsException(UsersApiConstants.UsersApiHasNoImplementationMessage);
        }
    }
}