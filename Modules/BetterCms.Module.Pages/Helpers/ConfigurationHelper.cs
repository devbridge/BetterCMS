// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConfigurationHelper.cs" company="Devbridge Group LLC">
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