// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GridOptions.cs" company="Devbridge Group LLC">
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

using MvcContrib.Sorting;
using MvcContrib.UI.Grid;

namespace BetterCms.Module.Root.Mvc.Grids.GridOptions
{
    [Serializable]
    public class GridOptions : GridSortOptions
    {
        /// <summary>
        /// The default page size
        /// </summary>
        public const int DefaultPageSize = 20;

        /// <summary>
        /// Initializes a new instance of the <see cref="GridOptions" /> class.
        /// </summary>
        public GridOptions()
        {
            PageNumber = 1;
        }

        /// <summary>
        /// Gets or sets the size of the page.
        /// </summary>
        /// <value>
        /// The size of the page.
        /// </value>
        public int PageSize { get; set; }

        /// <summary>
        /// Gets or sets the page number.
        /// </summary>
        /// <value>
        /// The page number.
        /// </value>
        public int PageNumber { get; set; }

        /// <summary>
        /// Gets or sets the total count.
        /// </summary>
        /// <value>
        /// The total count.
        /// </value>
        public int TotalCount { get; set; }

        /// <summary>
        /// Sets the default sorting options, if values are not set.
        /// </summary>
        public void SetDefaultSortingOptions(string sortColumn, bool isDescending = false)
        {
            if (string.IsNullOrWhiteSpace(Column))
            {
                Column = sortColumn;
                Direction = (isDescending) ? SortDirection.Descending : SortDirection.Ascending;
            }
        }

        /// <summary>
        /// Sets the default paging.
        /// </summary>
        public void SetDefaultPaging()
        {
            if (PageSize <= 0)
            {
                PageSize = DefaultPageSize;
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
            return string.Format("PageSize: {0}, PageNumber: {1}, TotalCount: {2}", PageSize, PageNumber, TotalCount);
        }

        /// <summary>
        /// Populates the specified options.
        /// </summary>
        /// <param name="options">The options.</param>
        public virtual void CopyFrom(GridOptions options)
        {
            PageNumber = options.PageNumber;
            PageSize = options.PageSize;
            TotalCount = options.TotalCount;
            Column = options.Column;
            Direction = options.Direction;
        }
    }
}