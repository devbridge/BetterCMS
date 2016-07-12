// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IUrlService.cs" company="Devbridge Group LLC">
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
using System.Collections.Generic;

namespace BetterCms.Module.Pages.Services
{
    public interface IUrlService
    {
        /// <summary>
        /// Adds the postfix to the specified URL.
        /// </summary>
        /// <param name="url">The page URL.</param>
        /// <param name="prefixPattern">The prefix pattern.</param>
        /// <param name="unsavedUrls">The list of not saved yet urls.</param>
        /// <returns></returns>
        string AddPageUrlPostfix(string url, string prefixPattern, List<string> unsavedUrls = null);

        /// <summary>
        /// Validates the internal URL.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns>true, if url is valid for internal use (without http:// and other prefixes and any suffixes, such as ?# etc.)</returns>
        bool ValidateInternalUrl(string url);

        /// <summary>
        /// Validates the internal URL with query string.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns>true, if url is valid for internal use</returns>
        bool ValidateInternalUrlWithQueryString(string url);
        
        /// <summary>
        /// Validates the external URL.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns>true, if url is valid for external links (can contain http://, ftp://  and other prefixes and any suffixes, such as ?# etc.)</returns>
        bool ValidateExternalUrl(string url);

        /// <summary>
        /// Validates URL using URL validation patterns from cms.config.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="message">The error message.</param>
        /// <param name="validatingFieldName">Name of the validating field.</param>
        /// <returns><c>true</c> if URL is valid</returns>
        bool ValidateUrlPatterns(string url, out string message, string validatingFieldName = null);

        /// <summary>
        /// Fixes the URL (adds slashes in front and bottom).
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns>Fixed url</returns>
        string FixUrl(string url);

        string FixUrlFront(string url);
    }
}