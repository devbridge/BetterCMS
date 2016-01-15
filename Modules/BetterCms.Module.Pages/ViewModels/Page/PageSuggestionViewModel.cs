// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PageSuggestionViewModel.cs" company="Devbridge Group LLC">
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

using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.ViewModels.Autocomplete;

namespace BetterCms.Module.Pages.ViewModels.Page
{
    public class PageSuggestionViewModel : SuggestionViewModel 
    {
        /// <summary>
        /// Gets or sets a value indicating whether to retrieve only untranslated pages.
        /// </summary>
        /// <value>
        /// <c>true</c> if retrieve only untranslated pages; otherwise, <c>false</c>.
        /// </value>
        public bool OnlyUntranslatedPages { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to include master pages.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to include master pages; otherwise, <c>false</c>.
        /// </value>
        public bool IncludeMasterPages { get; set; }

        /// <summary>
        /// Gets or sets the excluded language id.
        /// </summary>
        /// <value>
        /// The excluded language id.
        /// </value>
        public Guid? ExcludedLanguageId { get; set; }

        /// <summary>
        /// Gets or sets the excplicitly included pages.
        /// </summary>
        /// <value>
        /// The excplicitly included pages.
        /// </value>
        public string ExcplicitlyIncludedPages { get; set; }

        /// <summary>
        /// Gets the excplicitly included pages array.
        /// </summary>
        /// <value>
        /// The excplicitly included pages array.
        /// </value>
        public Guid[] ExcplicitlyIncludedPagesArray
        {
            get
            {
                var result = new List<Guid>();
                if (!string.IsNullOrWhiteSpace(ExcplicitlyIncludedPages))
                {
                    foreach (var id in ExcplicitlyIncludedPages.Split('|'))
                    {
                        var guid = id.ToGuidOrDefault();
                        if (!guid.HasDefaultValue())
                        {
                            result.Add(guid);
                        }
                    }
                }

                return result.ToArray();
            }
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format(
                "{0}, ExcplicitlyIncludedPages: {1}, OnlyUntranslatedPages: {2}, ExcludedLanguageId: {3}",
                base.ToString(),
                ExcplicitlyIncludedPages,
                OnlyUntranslatedPages,
                ExcludedLanguageId);
        }
    }
}