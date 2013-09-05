using BetterCms.Core.Exceptions;
using BetterCms.Module.Api.Operations.Users.Users.User.ValidateUser;

namespace BetterCms.Module.Api.Operations.Users.Users.User.Validate
{
    public class DefaultValidateUserService : IValidateUserService
    {
        public ValidateUserResponse Get(ValidateUserRequest request)
        {
            throw new CmsException(UsersApiConstants.UsersApiHasNoImplementationMessage);
        }
    }
}