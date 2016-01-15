// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GoogleAnalyticsModuleHelper.cs" company="Devbridge Group LLC">
// 
// Copyright (C) 2015,2016 Devbridge Group LLC
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public License
// along with this program.  If not, see http://www.gnu.org/licenses/. 
// </copyright>
// 
// <summary>
// Better CMS is a publishing focused and developer friendly .NET open source CMS.
// 
// Website: https://www.bettercms.com 
// GitHub: https://github.com/devbridge/bettercms
// Email: info@bettercms.com
// </summary>
// --------------------------------------------------------------------------------------------------------------------
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