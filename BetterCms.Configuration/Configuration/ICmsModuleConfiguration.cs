namespace BetterCms.Configuration
{
    public interface ICmsModuleConfiguration
    {
        string Name { get; set; }

        string GetValue(string key);

        void SetValue(string key, string value);
    }
}
