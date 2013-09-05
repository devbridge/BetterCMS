using BetterCms.Module.Api.Operations.Users.Users.User.ValidateUser;

namespace BetterCms.Module.Api.Operations.Users.Users.User
{
    public interface IUserService
    {
        GetUserResponse Get(GetUserRequest request);

        ValidateUserResponse Validate(ValidateUserRequest request);
    }
}
