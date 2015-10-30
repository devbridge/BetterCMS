
namespace BetterCms.Module.Blog.Helpers
{
    public class ConfigurationHelper
    {
        private const string enableFillSeoDataFromArticleProperties = "enableFillSeoDataFromArticleProperties";


        public static bool IsFillSeoDataFromArticlePropertiesEnabled(ICmsConfiguration cmsConfiguration)
        {
            var value = GetConfigurationValue(cmsConfiguration, enableFillSeoDataFromArticleProperties);

            if (!string.IsNullOrEmpty(value))
            {
                bool isEnabled;
                if (bool.TryParse(value, out isEnabled))
                {
                    return isEnabled;
                }
            }

            return true;
        }


        private static string GetConfigurationValue(ICmsConfiguration cmsConfiguration, string key)
        {
            if (cmsConfiguration.Modules != null)
            {
                var moduleConfiguration = cmsConfiguration.Modules.GetByName(BlogModuleDescriptor.ModuleName);
                if (moduleConfiguration != null)
                {
                    return moduleConfiguration.GetValue(key);
                }
            }

            return null;
        }
    }
}