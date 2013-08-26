using BetterCms.Core.Exceptions;

namespace BetterCms.Module.Api.Operations.Users.Users.User
{
    public class DefaultUserService : IUserService
    {
        public GetUserResponse Get(GetUserRequest request)
        {
            throw new CmsException(UsersApiConstants.UsersApiHasNoImplementationMessage);
        }
    }
}