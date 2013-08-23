using BetterCms.Core.Exceptions;

using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Api.Operations.Users.Users
{
    public class DefaultUsersService : Service, IUsersService
    {
        public GetUsersResponse Get(GetUsersRequest request)
        {
            throw new CmsException(UsersApiConstants.UsersApiHasNoImplementationMessage);
        }
    }
}