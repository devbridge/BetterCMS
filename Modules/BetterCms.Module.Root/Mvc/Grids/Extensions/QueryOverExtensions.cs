// --------------------------------------------------------------------------------------------------------------------
// <copyright file="QueryOverExtensions.cs" company="Devbridge Group LLC">
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
using MvcContrib.Sorting;

using NHibernate;
using NHibernate.Criterion;

using SortAndPagingOptions = BetterCms.Module.Root.Mvc.Grids.GridOptions.GridOptions;

using BetterModules.Core.DataAccess.DataContext;

namespace BetterCms.Module.Root.Mvc.Grids.Extensions
{
    public static class QueryOverExtensions
    {
        public static IFutureValue<int> ToRowCountFutureValue<TRoot, TSubType>(this IQueryOver<TRoot, TSubType> query)
        {
            return query.ToRowCountQuery().FutureValue<int>();
        }

        /// <summary>
        /// Adds the sorting and paging to nHibernate query.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="options">The options.</param>
        /// <returns>The query</returns>
        public static IQueryOver<TRoot, TSubType> AddSortingAndPaging<TRoot, TSubType>(this IQueryOver<TRoot, TSubType> query, SortAndPagingOptions options)
        {
            if (options != null)
            {
                return query.AddOrder(options).AddPaging(options);
            }
            return query;
        }

        /// <summary>
        /// Adds the sorting to nHibernate query.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="options">The options.</param>
        /// <returns>The query</returns>
        public static IQueryOver<TRoot, TSubType> AddOrder<TRoot, TSubType>(this IQueryOver<TRoot, TSubType> query, SortAndPagingOptions options)
        {
            if (options != null && !string.IsNullOrWhiteSpace(options.Column))
            {
                query.UnderlyingCriteria.AddOrder(new Order(options.Column, options.Direction == SortDirection.Ascending));
            }

            return query;
        }

        /// <summary>
        /// Adds the paging to nHibernate query.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="options">The options.</param>
        /// <returns>The query</returns>
        public static IQueryOver<TRoot, TSubType> AddPaging<TRoot, TSubType>(this IQueryOver<TRoot, TSubType> query, SortAndPagingOptions options)
        {
            return query.AddPaging(options.PageNumber, options.PageSize);
        }
    }
}