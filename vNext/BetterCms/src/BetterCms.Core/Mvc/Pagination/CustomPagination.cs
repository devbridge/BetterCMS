/*
    Copyright 2010 CodePlex Foundation, Inc.
    Copyright 2007-2010 Eric Hexter, Jeffrey Palermo
    For Licence, see http://www.codeplex.com/MVCContrib/license

    Modified by Devbridge Group: updated source code to support ASP.NET 5 MVC6
*/

using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace BetterCms.Core.Mvc.Pagination
{
    /// <summary>
    /// Implementation of IPagination that wraps a pre-paged data source. 
    /// </summary>
    public class CustomPagination<T> : IPagination<T>
    {
        private readonly IList<T> _dataSource;

        /// <summary>
        /// Creates a new instance of CustomPagination
        /// </summary>
        /// <param name="dataSource">A pre-paged slice of data</param>
        /// <param name="pageNumber">The current page number</param>
        /// <param name="pageSize">The number of items per page</param>
        /// <param name="totalItems">The total number of items in the overall datasource</param>
        public CustomPagination(IEnumerable<T> dataSource, int pageNumber, int pageSize, int totalItems)
        {
            _dataSource = dataSource.ToList();
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalItems = totalItems;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _dataSource.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int PageNumber { get; private set; }
        public int PageSize { get; private set; }
        public int TotalItems { get; private set; }

        public int TotalPages
        {
            get { return (int)Math.Ceiling(((double)TotalItems) / PageSize); }
        }

        public int FirstItem
        {
            get
            {
                return ((PageNumber - 1) * PageSize) + 1;
            }
        }

        public int LastItem
        {
            get { return FirstItem + _dataSource.Count - 1; }
        }

        public bool HasPreviousPage
        {
            get { return PageNumber > 1; }
        }

        public bool HasNextPage
        {
            get { return PageNumber < TotalPages; }
        }
    }
}