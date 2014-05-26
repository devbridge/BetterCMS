namespace BetterCms.Module.Api.Operations.Users.Roles.Role
{
    public interface IRoleService
    {
        GetRoleResponse Get(GetRoleRequest request);

        PutRoleResponse Put(PutRoleRequest request);

        DeleteRoleResponse Delete(DeleteRoleRequest request);
    }
}
