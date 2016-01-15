// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataOptionsExtensions.cs" company="Devbridge Group LLC">
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
using System.Linq;

using BetterCms.Module.Api.Infrastructure.Enums;

namespace BetterCms.Module.Api.Infrastructure
{
    /// <summary>
    /// DataOptions class extensions
    /// </summary>
    public static class DataOptionsExtensions
    {
        /// <summary>
        /// Sets the default order.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <param name="column">The column.</param>
        /// <param name="direction">The direction.</param>
        public static void SetDefaultOrder(this DataOptions options, string column, OrderDirection direction = OrderDirection.Asc)
        {
            if (!options.Order.By.Any())
            {
                options.Order.Add(column, direction);
            }
        }

        /// <summary>
        /// Determines whether the specified options has paging.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <returns>
        ///   <c>true</c> if the specified options has paging; otherwise, <c>false</c>.
        /// </returns>
        public static bool HasPaging(this DataOptions options)
        {
            return options != null && options.Take > 0;
        }

        /// <summary>
        /// Determines whether the specified data options has a column in sort-by section.
        /// </summary>
        /// <param name="options">The data options.</param>
        /// <param name="column">The column.</param>
        /// <returns>
        ///   <c>true</c> if a column is in the sort-by section; otherwise, <c>false</c>.
        /// </returns>
        public static bool HasColumnInSortBySection(this DataOptions options, string column)
        {
            var order = options.Order;
            if (order != null && order.By != null && order.By.Count > 0)
            {
                return order.By.Count(f => f.Field != null && f.Field.ToLowerInvariant() == column.ToLowerInvariant()) > 0;
            }

            return false;
        }

        /// <summary>
        /// Determines whether the specified data options has a column in the where section.
        /// </summary>
        /// <param name="options">The data options.</param>
        /// <param name="column">The column.</param>
        /// <returns>
        ///   <c>true</c> if a column is in the where section; otherwise, <c>false</c>.
        /// </returns>
        public static bool HasColumnInWhereSection(this DataOptions options, string column)
        {
            var filter = options.Filter;

            if (filter != null)
            {
                return HasColumnInWhereSection(filter, column);
            }

            return false;
        }

        private static bool HasColumnInWhereSection(DataFilter filter, string column)
        {
            if (filter.Where != null && filter.Where.Count > 0)
            {
                if (filter.Where.Count(f => f.Field != null && f.Field.ToLowerInvariant() == column.ToLowerInvariant()) > 0)
                {
                    return true;
                }
            }

            if (filter.Inner != null && filter.Inner.Count > 0)
            {
                foreach (var innerFilter in filter.Inner)
                {
                    if (HasColumnInWhereSection(innerFilter, column))
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}