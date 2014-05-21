namespace BetterCms.Sandbox.Mvc4.ConfigurationOverride
{
    public interface IMyConfiguration
    {
        string GetStorageConnectionString();

        string GetCmsStorageContainerName();
    }
}