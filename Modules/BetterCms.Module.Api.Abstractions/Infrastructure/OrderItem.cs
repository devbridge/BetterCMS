// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OrderItem.cs" company="Devbridge Group LLC">
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
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure.Enums;

namespace BetterCms.Module.Api.Infrastructure
{
    /// <summary>
    /// Represents class for ordering items
    /// </summary>
    [DataContract]
    [Serializable]
    public class OrderItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OrderItem" /> class.
        /// NOTE: required for JavaScript serialization
        /// </summary>
        public OrderItem()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OrderItem" /> class.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="direction">The order direction.</param>
        public OrderItem(string field, OrderDirection direction = OrderDirection.Asc)
        {
            Direction = direction;
            Field = field;
        }

        /// <summary>
        /// Gets or sets the ordering field.
        /// </summary>
        /// <value>
        /// The ordering field.
        /// </value>
        [DataMember]
        public string Field { get; set; }

        /// <summary>
        /// Gets or sets the order direction.
        /// </summary>
        /// <value>
        /// The order direction.
        /// </value>
        [DataMember]
        public OrderDirection Direction { get; set; }

        /// <summary>
        /// Gets a value indicating whether query must be ordered by descending.
        /// </summary>
        /// <value>
        ///   <c>true</c> if query must be ordered by descending; otherwise, <c>false</c>.
        /// </value>
        public bool OrderByDescending
        {
            get
            {
                return Direction == OrderDirection.Desc;
            }
        }
    }
}