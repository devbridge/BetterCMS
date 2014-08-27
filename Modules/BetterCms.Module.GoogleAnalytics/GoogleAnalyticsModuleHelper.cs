namespace BetterCms.Module.GoogleAnalytics
{
    public static class GoogleAnalyticsModuleHelper
    {
        public static string GetLinkType(ICmsConfiguration cmsConfiguration)
        {
            var value = GetConfigurationValue(cmsConfiguration, GoogleAnalyticsModuleConstants.LinkTypeKey);
            return !string.IsNullOrEmpty(value) ? value : GoogleAnalyticsModuleConstants.DefaultLinkType;
        }

        public static string GetChangeFrequency(ICmsConfiguration cmsConfiguration)
        {
            var value = GetConfigurationValue(cmsConfiguration, GoogleAnalyticsModuleConstants.ChangeFrequencyKey);
            return !string.IsNullOrEmpty(value) ? value : GoogleAnalyticsModuleConstants.DefaultChangeFrequency;
        }

        public static string GetPriority(ICmsConfiguration cmsConfiguration)
        {
            var value = GetConfigurationValue(cmsConfiguration, GoogleAnalyticsModuleConstants.PriorityKey);
            return !string.IsNullOrEmpty(value) ? value : GoogleAnalyticsModuleConstants.DefaultPriority;
        }

        public static string GetSitemapUrl(ICmsConfiguration cmsConfiguration)
        {
            var value = GetConfigurationValue(cmsConfiguration, GoogleAnalyticsModuleConstants.SitemapUrlKey);
            return !string.IsNullOrEmpty(value) ? value : GoogleAnalyticsModuleConstants.DefaultSitemapUrl;
        }

        public static string GetDefaultSitemapTitle(ICmsConfiguration cmsConfiguration)
        {
            var value = GetConfigurationValue(cmsConfiguration, GoogleAnalyticsModuleConstants.DefaultSitemapTitleKey);
            return !string.IsNullOrEmpty(value) ? value : GoogleAnalyticsModuleConstants.DefaultSitemapTitle;
        }

        public static string GetDefaultDateTimeFormat(ICmsConfiguration cmsConfiguration)
        {
            var value = GetConfigurationValue(cmsConfiguration, GoogleAnalyticsModuleConstants.GoogleSitemapDateFormatKey);
            return !string.IsNullOrEmpty(value) ? value : GoogleAnalyticsModuleConstants.GoogleSitemapDateFormat;
        }

        private static string GetConfigurationValue(ICmsConfiguration cmsConfiguration, string key)
        {
            var moduleConfiguration = cmsConfiguration.Modules.GetByName(GoogleAnalyticsModuleDescriptor.ModuleName);
            return moduleConfiguration != null ? moduleConfiguration.GetValue(key) : string.Empty;
        }
    }
}