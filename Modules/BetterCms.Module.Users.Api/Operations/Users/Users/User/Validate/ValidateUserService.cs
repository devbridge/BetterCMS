using System;
using System.Web.Http;
using System.Web.Http.ModelBinding;

using BetterCms.Module.Api.ApiExtensions;
using BetterCms.Module.Api.Operations.Users.Users.User.ValidateUser;
using BetterCms.Module.Users.Services;

using ServiceStack.ServiceInterface;

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

        [Route("users/validate")]
        public ValidateUserResponse Get([ModelBinder(typeof(JsonModelBinder))]ValidateUserRequest request)
        {
            var userId = authenticationService.GetUserIdIfValid(request.Data.UserName, request.Data.Password);
            return new ValidateUserResponse
                       {
                           Data = new ValidUserModel { UserId = userId, Valid = userId.HasValue && userId.Value != Guid.Empty }
                       };
        }
    }
}