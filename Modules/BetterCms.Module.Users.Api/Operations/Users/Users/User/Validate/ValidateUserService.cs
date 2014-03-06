using System;

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
            var userId = authenticationService.GetUserIdIfValid(request.Data.UserName, request.Data.Password);
            return new ValidateUserResponse
                       {
                           Data = new ValidUserModel { UserId = userId, Valid = userId.HasValue && userId.Value != Guid.Empty }
                       };
        }
    }
}