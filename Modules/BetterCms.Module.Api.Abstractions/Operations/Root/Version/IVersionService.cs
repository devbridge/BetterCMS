namespace BetterCms.Module.Api.Operations.Root.Version
{
    public interface IVersionService
    {
        GetVersionResponse Get(GetVersionRequest request = null);
    }
}
