/*
    Copyright 2010 CodePlex Foundation, Inc.
    Copyright 2007-2010 Eric Hexter, Jeffrey Palermo
    For Licence, see http://www.codeplex.com/MVCContrib/license

    Modified by Devbridge Group: updated source code to support ASP.NET 5 MVC6
*/

using System.Collections;
using System.Collections.Generic;

namespace BetterCms.Core.Mvc.Pagination
{
    /// <summary>
	/// A collection of objects that has been split into pages.
	/// </summary>
	public interface IPagination : IEnumerable
    {
        /// <summary>
        /// The current page number
        /// </summary>
        int PageNumber { get; }
        /// <summary>
        /// The number of items in each page.
        /// </summary>
        int PageSize { get; }
        /// <summary>
        /// The total number of items.
        /// </summary>
        int TotalItems { get; }
        /// <summary>
        /// The total number of pages.
        /// </summary>
        int TotalPages { get; }
        /// <summary>
        /// The index of the first item in the page.
        /// </summary>
        int FirstItem { get; }
        /// <summary>
        /// The index of the last item in the page.
        /// </summary>
        int LastItem { get; }
        /// <summary>
        /// Whether there are pages before the current page.
        /// </summary>
        bool HasPreviousPage { get; }
        /// <summary>
        /// Whether there are pages after the current page.
        /// </summary>
        bool HasNextPage { get; }
    }


    /// <summary>
    /// Generic form of <see cref="IPagination"/>
    /// </summary>
    /// <typeparam name="T">Type of object being paged</typeparam>
    public interface IPagination<T> : IPagination, IEnumerable<T>
    {

    }
}