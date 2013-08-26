using ServiceStack.FluentValidation;

namespace BetterCms.Module.Api.Operations.Users.Users.User
{
    public class GetUserRequestValidator : AbstractValidator<GetUserRequest>
    {
        public GetUserRequestValidator()
        {
            RuleFor(request => request.UserId).Must(UserNameMustBeNullIfUserIdProvided).WithMessage("A UserName field must be null if UserId is provided.");
            RuleFor(request => request.UserName).Must(AtLeastOneFieldShouldBeProvided).WithMessage("A UserId or UserName should be provided.");
        }

        private bool AtLeastOneFieldShouldBeProvided(GetUserRequest getUserRequest, string userName)
        {
            return getUserRequest.UserId != null || !string.IsNullOrEmpty(getUserRequest.UserName);
        }

        private bool UserNameMustBeNullIfUserIdProvided(GetUserRequest getUserRequest, System.Guid? userId)
        {
            return userId != null && string.IsNullOrEmpty(getUserRequest.UserName) ||
                   userId == null && !string.IsNullOrEmpty(getUserRequest.UserName);
        }
    }
}
