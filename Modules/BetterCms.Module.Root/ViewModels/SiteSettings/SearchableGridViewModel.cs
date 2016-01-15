// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SearchableGridViewModel.cs" company="Devbridge Group LLC">
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

using BetterCms.Module.Root.Mvc.Grids;
using BetterCms.Module.Root.Mvc.Grids.GridOptions;

using MvcContrib.Pagination;

namespace BetterCms.Module.Root.ViewModels.SiteSettings
{
    [Serializable]
    public class SearchableGridViewModel<TModel> where TModel : IEditableGridItem
    {
        /// <summary>
        /// Gets or sets the list of view models.
        /// </summary>
        /// <value>
        /// The list of view models.
        /// </value>
        public IPagination<TModel> Items { get; set; }

        /// <summary>
        /// Gets or sets the grid options.
        /// </summary>
        /// <value>
        /// The grid sort options.
        /// </value>
        public SearchableGridOptions GridOptions { get; set; }

        /// <summary>
        /// Gets or sets the search query.
        /// </summary>
        /// <value>
        /// The search query.
        /// </value>
        public string SearchQuery { get; set; }

        /// <summary>
        /// Gets the total count - used for Json serialization.
        /// </summary>
        /// <value>
        /// The total count.
        /// </value>
        public int TotalCount
        {
            get
            {
                return Items != null ?  Items.TotalItems : 0;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchableGridViewModel{TModel}" /> class.
        /// </summary>
        public SearchableGridViewModel()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchableGridViewModel{TModel}" /> class.
        /// </summary>
        /// <param name="items">The models.</param>
        /// <param name="options">The options.</param>
        /// <param name="totalCount">The total count.</param>
        public SearchableGridViewModel(IEnumerable<TModel> items, SearchableGridOptions options, int totalCount)
        {
            options.TotalCount = totalCount;
            Items = new CustomPagination<TModel>(items, options.PageNumber, options.PageSize, totalCount);
            SearchQuery = options.SearchQuery;
            GridOptions = options;
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("GridOptions: {0}, SearchQuery: {1}, TotalCount: {2}", GridOptions, SearchQuery, TotalCount);
        }
    }
}