// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ClonePageWithLanguageViewModel.cs" company="Devbridge Group LLC">
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
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using BetterCms.Module.Pages.Content.Resources;
using BetterCms.Module.Root.Models;

namespace BetterCms.Module.Pages.ViewModels.Page
{
    public class ClonePageWithLanguageViewModel : ClonePageViewModel
    {
        public ClonePageWithLanguageViewModel()
        {
            Languages = new List<LookupKeyValue>();
        }

        /// <summary>
        /// Gets or sets the language id.
        /// </summary>
        /// <value>
        /// The language id.
        /// </value>
        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(PagesGlobalization), ErrorMessageResourceName = "ClonePageWithLanguage_Language_RequiredMessage")]
        public Guid? LanguageId { get; set; }

        /// <summary>
        /// Gets or sets the list of languages.
        /// </summary>
        /// <value>
        /// The list of languages.
        /// </value>
        public List<LookupKeyValue> Languages { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to show warning about no cultures created.
        /// </summary>
        /// <value>
        /// <c>true</c> if to show warning about no cultures created; otherwise, <c>false</c>.
        /// </value>
        public bool ShowWarningAboutNoCultures { get; set; }

        /// <summary>
        /// Gets or sets the sitemap action enabled flag.
        /// </summary>
        /// <value>
        /// The sitemap action enabled flag.
        /// </value>
        public bool IsSitemapActionEnabled { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("{0}, LanguageId: {1}", base.ToString(), LanguageId);
        }
    }
}