namespace BetterCms.Module.Api.Operations.Users.Roles
{
    public interface IRolesService
    {
        GetRolesResponse Get(GetRolesRequest request);

        PostRoleResponse Post(PostRoleRequest request);
    }
}
