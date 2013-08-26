namespace BetterCms.Module.Users.Services
{
    public interface IRoleService
    {
        Models.Role CreateRole(string name);
        
        Models.Role UpdateRole(System.Guid id, int version, string name);
    }
}