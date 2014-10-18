namespace BetterCms.Configuration
{
    public interface ICmsModuleConfiguration
    {
        string Name { get; set; }

        string GetValue(string key);
    }
}
