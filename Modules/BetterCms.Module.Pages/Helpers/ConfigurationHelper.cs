namespace BetterCms.Module.Pages.Helpers
{
    public class ConfigurationHelper
    {
        private const string enableAddNewPageToSitemapActionKey = "enableAddNewPageToSitemapAction";
        private const string enableAddNewTranslationPageToSitemapActionKey = "enableAddNewTranslationPageToSitemapAction";
        private const string enableAddClonedPageToSitemapActionKey = "enableAddClonedPageToSitemapAction";

        public static bool IsSitemapActionEnabledAfterAddingNewPage(ICmsConfiguration cmsConfiguration)
        {
            var value = GetConfigurationValue(cmsConfiguration, enableAddNewPageToSitemapActionKey);

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

        public static bool IsSitemapActionEnabledAfterAddingTranslationForPage(ICmsConfiguration cmsConfiguration)
        {
            var value = GetConfigurationValue(cmsConfiguration, enableAddNewTranslationPageToSitemapActionKey);

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

        public static bool IsSitemapActionEnabledAfterCloningPage(ICmsConfiguration cmsConfiguration)
        {
            var value = GetConfigurationValue(cmsConfiguration, enableAddClonedPageToSitemapActionKey);

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
                var moduleConfiguration = cmsConfiguration.Modules.GetByName(PagesModuleDescriptor.ModuleName);
                if (moduleConfiguration != null)
                {
                    return moduleConfiguration.GetValue(key);
                }
            }

            return null;
        }
    }
}