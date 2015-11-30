// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SavePageResponse.cs" company="Devbridge Group LLC">
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
using System.Drawing.Printing;

using BetterCms.Core.DataContracts;
using BetterCms.Core.DataContracts.Enums;

using BetterCms.Module.Pages.Models;
using BetterCms.Module.Root.Mvc;

namespace BetterCms.Module.Pages.Command.Page.SavePageProperties
{
    public class SavePageResponse
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
        /// Gets or sets the page title.
        /// </summary>
        /// <value>
        /// The page title.
        /// </value>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the page URL.
        /// </summary>
        /// <value>
        /// The page URL.
        /// </value>
        public string PageUrl { get; set; }

        /// <summary>
        /// Gets or sets the date the page is created on.
        /// </summary>
        /// <value>
        /// The date the page is created on.
        /// </value>
        public string CreatedOn { get; set; }

        /// <summary>
        /// Gets or sets the date the page is modified on.
        /// </summary>
        /// <value>
        /// The date the page is modified on.
        /// </value>
        public string ModifiedOn { get; set; }

        /// <summary>
        /// Gets or sets the page status.
        /// </summary>
        /// <value>
        /// The page status.
        /// </value>
        public PageStatus PageStatus { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether page has SEO.
        /// </summary>
        /// <value>
        ///   <c>true</c> if page has SEO; otherwise, <c>false</c>.
        /// </value>
        public bool HasSEO { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is archived.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is archived; otherwise, <c>false</c>.
        /// </value>
        public bool IsArchived { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this page is master.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this page is a master page; otherwise, <c>false</c>.
        /// </value>
        public bool IsMasterPage { get; set; }

        /// <summary>
        /// Gets or sets the language id.
        /// </summary>
        /// <value>
        /// The language id.
        /// </value>
        public Guid? LanguageId { get; set; }

        /// <summary>
        /// Gets or sets the sitemap action enabled flag.
        /// </summary>
        /// <value>
        /// The sitemap action enabled flag.
        /// </value>
        public bool IsSitemapActionEnabled { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SavePageResponse" /> class.
        /// </summary>
        /// <param name="page">The page.</param>
        public SavePageResponse(PageProperties page)
        {
            PageId = page.Id;
            Title = page.Title;
            CreatedOn = page.CreatedOn.ToFormattedDateString();
            ModifiedOn = page.ModifiedOn.ToFormattedDateString();
            PageStatus = page.Status;
            HasSEO = ((IPage)page).HasSEO;
            PageUrl = page.PageUrl;
            IsArchived = page.IsArchived;
            IsMasterPage = page.IsMasterPage;
            LanguageId = page.Language != null ? page.Language.Id : (Guid?)null;
        }
    }
}