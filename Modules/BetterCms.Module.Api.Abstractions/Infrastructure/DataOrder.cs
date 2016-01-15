// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataOrder.cs" company="Devbridge Group LLC">
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
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure.Enums;

namespace BetterCms.Module.Api.Infrastructure
{
    /// <summary>
    /// Represents container for ordering items list
    /// </summary>
    [DataContract]
    [Serializable]
    public class DataOrder
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataOrder" /> class.
        /// </summary>
        public DataOrder()
        {
            By = new List<OrderItem>();
        }

        /// <summary>
        /// Gets or sets the list of order items.
        /// </summary>
        /// <value>
        /// The list of order items.
        /// </value>
        [DataMember]
        public List<OrderItem> By { get; set; }

        /// <summary>
        /// Adds the order item to orderings list.
        /// </summary>
        /// <param name="field">The ordering field.</param>
        /// <param name="direction">The order direction.</param>
        public void Add(string field, OrderDirection direction = OrderDirection.Asc)
        {
            var filterItem = new OrderItem(field, direction);

            By.Add(filterItem);
        }
    }
}