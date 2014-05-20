using BetterCms.Core.Exceptions;
using BetterCms.Module.Api.Operations.Users.Users.User.ValidateUser;

namespace BetterCms.Module.Api.Operations.Users.Users.User
{
    public class DefaultUserService : IUserService
    {
        private readonly IValidateUserService validateUserService;

        public DefaultUserService(IValidateUserService validateUserService)
        {
            this.validateUserService = validateUserService;
        }

        public GetUserResponse Get(GetUserRequest request)
        {
            throw new CmsException(UsersApiConstants.UsersApiHasNoImplementationMessage);
        }

        public DeleteUserResponse Delete(DeleteUserRequest request)
        {
            throw new CmsException(UsersApiConstants.UsersApiHasNoImplementationMessage);
        }

        public PutUserResponse Put(PutUserRequest request)
        {
            throw new CmsException(UsersApiConstants.UsersApiHasNoImplementationMessage);
        }

        ValidateUserResponse IUserService.Validate(ValidateUserRequest request)
        {
            return validateUserService.Get(request);
        }
    }
}