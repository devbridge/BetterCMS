using BetterCms.Core.Exceptions;

namespace BetterCms.Module.Api.Operations.Users.Users
{
    public class DefaultUsersService : IUsersService
    {
        public GetUsersResponse Get(GetUsersRequest request)
        {
            throw new CmsException(UsersApiConstants.UsersApiHasNoImplementationMessage);
        }
    }
}