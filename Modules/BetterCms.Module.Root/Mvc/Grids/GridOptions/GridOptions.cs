using System;

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