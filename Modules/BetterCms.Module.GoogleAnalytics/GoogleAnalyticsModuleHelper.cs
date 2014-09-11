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

        public static string GetSitemapTitle(ICmsConfiguration cmsConfiguration)
        {
            var value = GetConfigurationValue(cmsConfiguration, GoogleAnalyticsModuleConstants.SitemapTitleKey);
            return !string.IsNullOrEmpty(value) ? value : GoogleAnalyticsModuleConstants.DefaultSitemapTitle;
        }

        public static string GetDateTimeFormat(ICmsConfiguration cmsConfiguration)
        {
            var value = GetConfigurationValue(cmsConfiguration, GoogleAnalyticsModuleConstants.SitemapDateFormatKey);
            return !string.IsNullOrEmpty(value) ? value : GoogleAnalyticsModuleConstants.DefaultSitemapDateFormat;
        }

        public static string GetAnalyticsKey(ICmsConfiguration cmsConfiguration)
        {
            return GetConfigurationValue(cmsConfiguration, GoogleAnalyticsModuleConstants.KeyForAnalyticsKey);
        }

        private static string GetConfigurationValue(ICmsConfiguration cmsConfiguration, string key)
        {
            var moduleConfiguration = cmsConfiguration.Modules.GetByName(GoogleAnalyticsModuleDescriptor.ModuleName);
            return moduleConfiguration != null ? moduleConfiguration.GetValue(key) : string.Empty;
        }
    }
}