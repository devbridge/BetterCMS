// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ClonePageViewModel.cs" company="Devbridge Group LLC">
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
using System.Web.Mvc;

using BetterCms.Module.Pages.Content.Resources;
using BetterCms.Module.Pages.Mvc.Attributes;
using BetterCms.Module.Root.ViewModels.Security;

using BetterModules.Core.Models;

namespace BetterCms.Module.Pages.ViewModels.Page
{
    public class ClonePageViewModel : IAccessSecuredViewModel
    {
        /// <summary>
        /// Gets or sets the page id.
        /// </summary>
        /// <value>
        /// The page id.
        /// </value>
        public Guid PageId { get; set; }

        /// <summary>
        /// Gets or sets the page title.
        /// </summary>
        /// <value>
        /// The page title.
        /// </value>
        [AllowHtml]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(PagesGlobalization), ErrorMessageResourceName = "ClonePage_PageTitle_RequiredMessage")]
        [StringLength(MaxLength.Name, MinimumLength = 1, ErrorMessageResourceType = typeof(PagesGlobalization), ErrorMessageResourceName = "ClonePage_PageTitle_MaxLengthMessage")]
        public string PageTitle { get; set; }

        /// <summary>
        /// Gets or sets the page permalink.
        /// </summary>
        /// <value>
        /// The page permalink.
        /// </value>
        [CustomPageUrlValidation]
        [StringLength(MaxLength.Url, MinimumLength = 1, ErrorMessageResourceType = typeof(PagesGlobalization), ErrorMessageResourceName = "ClonePage_PageUrl_MaxLengthMessage")]
        public string PageUrl { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("PageId: {0}, PageTitle: {1}, PageUrl: {2}", PageId, PageTitle, PageUrl);
        }

        /// <summary>
        /// Gets or sets a value indicating whether access control enabled.
        /// </summary>
        /// <value>
        /// <c>true</c> if access control enabled; otherwise, <c>false</c>.
        /// </value>
        public bool AccessControlEnabled { get; set; }

        /// <summary>
        /// Gets or sets the user access list.
        /// </summary>
        /// <value>
        /// The user access list.
        /// </value>
        public System.Collections.Generic.IList<UserAccessViewModel> UserAccessList { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether dialog should be opened in the read only mode.
        /// </summary>
        /// <value>
        /// <c>true</c> if dialog should be opened in the read only mode; otherwise, <c>false</c>.
        /// </value>
        public bool IsReadOnly { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether page is a master page.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this page is a master page; otherwise, <c>false</c>.
        /// </value>
        public bool IsMasterPage { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to clone page as master page.
        /// </summary>
        /// <value>
        ///   <c>true</c> if clone page as master page; otherwise, <c>false</c>.
        /// </value>
        public bool CloneAsMasterPage { get; set; }

        /// <summary>
        /// Gets or sets the sitemap action enabled flag.
        /// </summary>
        /// <value>
        /// The sitemap action enabled flag.
        /// </value>
        public bool IsSitemapActionEnabled { get; set; }
    }
}