// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PagesConstants.cs" company="Devbridge Group LLC">
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
namespace BetterCms.Module.Pages
{
    public static class PagesConstants
    {
        /// <summary>
        /// The intenal URL regular expression (used in page naming)
        /// </summary>
        // NOTE: After changing this regexp please run BetterCms.Test.Module.Pages.ServiceTests.UrlServiceTests test.
        public const string InternalUrlRegularExpression = @"^\/?([^\\:?#[\]@!$&'()*+.,;=""<> \/%]{1,260}\/)*([^\\:?#[\]@!$&'()*+.,;=""<> \/%]{1,260})?$";

        /// <summary>
        /// The internal URL regular expression used for redirects
        /// </summary>
        public const string InternalUrlWithQueryStringRegularExpression = @"^\/?([^\\:?#[\]@!$&'()*+.,;=""<> \/%]{1,260}\/)*([^\\:?#[\]@!$&'()*+.,;=""<> \/%]{1,260})?((\?|#).*)?$";

        /// <summary>
        /// The external URL regular expression (used in sitemaps, external URLs)
        /// </summary>
        public const string ExternalUrlRegularExpression = @"^[^<>*&?]*((\?|#).*)?$";

        /// <summary>
        /// The options grid template.
        /// </summary>
        public const string OptionsGridTemplate = "~/Areas/bcms-pages/Views/Option/EditOptions.cshtml";
        
        /// <summary>
        /// The option values grid template.
        /// </summary>
        public const string OptionValuesGridTemplate = "~/Areas/bcms-pages/Views/Option/EditOptionValues.cshtml";

        /// <summary>
        /// The content version preview template
        /// </summary>
        public const string ContentVersionPreviewTemplate = "~/Areas/bcms-pages/Views/History/ContentVersion.cshtml";

        /// <summary>
        /// The editable grid cell template.
        /// </summary>
        public const string CustomTitleCellTemplate = "~/Areas/bcms-pages/Views/Shared/EditableGrid/CustomTitleCell.cshtml";
    }
}