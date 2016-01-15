// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DeletePageViewModel.cs" company="Devbridge Group LLC">
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
using System;
using System.ComponentModel.DataAnnotations;

using BetterCms.Module.Pages.Content.Resources;
using BetterCms.Module.Root.Content.Resources;

namespace BetterCms.Module.Pages.ViewModels.Page
{
    public class DeletePageViewModel
    {
        /// <summary>
        /// Gets or sets the page id.
        /// </summary>
        /// <value>
        /// The page id.
        /// </value>
        public Guid PageId { get; set; }

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        /// <value>
        /// The version.
        /// </value>
        public int Version { get; set; }

        /// <summary>
        /// Gets or sets the redirect URL.
        /// </summary>
        /// <value>
        /// The redirect URL.
        /// </value>
        [RegularExpression(PagesConstants.ExternalUrlRegularExpression, ErrorMessageResourceType = typeof(PagesGlobalization), ErrorMessageResourceName = "RedirectUrl_InvalidMessage")]
        public string RedirectUrl { get; set; }

        /// <summary>
        /// Gets or sets the security word.
        /// </summary>
        /// <value>
        /// The security word.
        /// </value>
        [Required(ErrorMessageResourceType = typeof(RootGlobalization), ErrorMessageResourceName = "Validation_RequiredAttribute_Message")]
        [RegularExpression("^[Dd][Ee][Ll][Ee][Tt][Ee]$", ErrorMessageResourceType = typeof(PagesGlobalization), ErrorMessageResourceName = "DeletePage_EnterDelete_Message")]
        public string SecurityWord { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is in sitemap.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is in sitemap; otherwise, <c>false</c>.
        /// </value>
        public bool IsInSitemap { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [update sitemap].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [update sitemap]; otherwise, <c>false</c>.
        /// </value>
        public bool UpdateSitemap { get; set; }

        /// <summary>
        /// Gets or sets the validation message.
        /// </summary>
        /// <value>
        /// The validation message.
        /// </value>
        public string ValidationMessage { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("PageId: {0}, Version: {1}, RedirectUrl: {2}", PageId, Version, RedirectUrl);
        }
    }
}