// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UntranslatedPagesGridViewModel.cs" company="Devbridge Group LLC">
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

using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc.Grids;

namespace BetterCms.Module.Pages.ViewModels.Filter
{
    [Serializable]
    public class UntranslatedPagesGridViewModel<TModel> : PagesGridViewModel<TModel> where TModel : IEditableGridItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UntranslatedPagesGridViewModel{TModel}" /> class.
        /// </summary>
        /// <param name="items">The items.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="totalCount">The total count.</param>
        /// <param name="categories">The categories.</param>
        public UntranslatedPagesGridViewModel(IEnumerable<TModel> items, UntranslatedPagesFilter filter, int totalCount)
            : base(items, filter, totalCount)
        {
            ExcplicitlyIncludedPages = filter.ExcplicitlyIncludedPages;
            ExcludedLanguageId = filter.ExcludedLanguageId;
            ExistingItems = filter.ExistingItems;
        }

        /// <summary>
        /// Gets or sets the excplicitly included pages.
        /// </summary>
        /// <value>
        /// The excplicitly included pages.
        /// </value>
        public string ExcplicitlyIncludedPages { get; set; }

        /// <summary>
        /// Gets or sets the excluded language id.
        /// </summary>
        /// <value>
        /// The excluded language id.
        /// </value>
        public Guid? ExcludedLanguageId { get; set; }

        /// <summary>
        /// Gets or sets the existing items.
        /// </summary>
        /// <value>
        /// The existing items.
        /// </value>
        public string ExistingItems { get; set; }
    }
}