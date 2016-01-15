// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EditHtmlContentWidgetViewModel.cs" company="Devbridge Group LLC">
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

using BetterCms.Module.Pages.ViewModels.Content;

namespace BetterCms.Module.Pages.ViewModels.Widgets
{
    /// <summary>
    /// A widget view model.
    /// </summary>
    public class EditHtmlContentWidgetViewModel : HtmlContentWidgetViewModel, IDraftDestroy
    {
        /// <summary>
        /// Gets or sets the page content id to preview this widget.
        /// </summary>
        /// <value>
        /// The page content id to preview this widget.
        /// </value>
        public Guid? PreviewOnPageContentId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether content editor must be opened in source mode.
        /// </summary>
        /// <value>
        ///   <c>true</c> if content editor must be opened in source mode; otherwise, <c>false</c>.
        /// </value>
        public bool EditInSourceMode { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether user can destroy draft.
        /// </summary>
        /// <value>
        ///   <c>true</c> if user can destroy draft; otherwise, <c>false</c>.
        /// </value>
        public bool CanDestroyDraft { get; set; }

        /// <summary>
        /// Gets or sets the last dynamic region number.
        /// </summary>
        /// <value>
        /// The last dynamic region number.
        /// </value>
        public int LastDynamicRegionNumber { get; set; }

        /// <summary>
        /// Determines, if child regions should be included to the results.
        /// </summary>
        /// <value>
        ///   <c>true</c> if child regions should be included to the results; otherwise, <c>false</c>.
        /// </value>
        public bool IncludeChildRegions { get; set; }

        /// <summary>
        /// Gets or sets the categories filter key.
        /// </summary>
        /// <value>
        /// The categories filter key.
        /// </value>
        public string CategoriesFilterKey { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("{0}", base.ToString());
        }
    }
}