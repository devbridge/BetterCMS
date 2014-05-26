namespace BetterCms.Module.Api.Operations.Users.Users
{
    public interface IUsersService
    {
        GetUsersResponse Get(GetUsersRequest request);
        
        PostUserResponse Post(PostUserRequest request);
    }
}
