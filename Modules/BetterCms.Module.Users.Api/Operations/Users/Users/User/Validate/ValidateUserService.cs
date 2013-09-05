using BetterCms.Module.Api.Operations.Users.Users.User.ValidateUser;
using BetterCms.Module.Users.Services;

using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Users.Api.Operations.Users.Users.User.Validate
{
    public class ValidateUserService : Service, IValidateUserService
    {
        private readonly IAuthenticationService authenticationService;

        public ValidateUserService(IAuthenticationService authenticationService)
        {
            this.authenticationService = authenticationService;
        }

        public ValidateUserResponse Get(ValidateUserRequest request)
        {
            return new ValidateUserResponse
                       {
                           Data = authenticationService.ValidateUser(request.Data.UserName, request.Data.Password)
                       };
        }
    }
}