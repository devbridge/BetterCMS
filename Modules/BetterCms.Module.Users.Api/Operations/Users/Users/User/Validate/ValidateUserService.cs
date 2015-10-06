using System;
using System.Web.Http;

using BetterCms.Module.Api.Operations.Users.Users.User.ValidateUser;
using BetterCms.Module.Users.Services;

namespace BetterCms.Module.Users.Api.Operations.Users.Users.User.Validate
{
    [RoutePrefix("bcms-api")]
    public class ValidateUserController : ApiController, IValidateUserService
    {
        private readonly IAuthenticationService authenticationService;

        public ValidateUserController(IAuthenticationService authenticationService)
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