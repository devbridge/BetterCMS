using BetterCms.Core.Exceptions;

using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Api.Operations.Users.Users.User
{
    public class DefaultUserService : Service, IUserService
    {
        public GetUserResponse Get(GetUserRequest request)
        {
            throw new CmsException(UsersApiConstants.UsersApiHasNoImplementationMessage);
        }
    }
}