// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataFilter.cs" company="Devbridge Group LLC">
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
    /// Represents container for filtering items list
    /// </summary>
    [DataContract]
    [Serializable]
    public class DataFilter
    {
        /// <summary>
        /// Gets or sets the filter connector.
        /// </summary>
        /// <value>
        /// The filter connector.
        /// </value>
        [DataMember]
        public FilterConnector Connector { get; set; }

        /// <summary>
        /// Gets or sets the list filter items.
        /// </summary>
        /// <value>
        /// The list of filter items.
        /// </value>
        [DataMember]
        public List<FilterItem> Where { get; set; }

        /// <summary>
        /// Gets or sets the list of inner filters.
        /// </summary>
        /// <value>
        /// The list of inner filters.
        /// </value>
        [DataMember]
        public List<DataFilter> Inner { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataFilter" /> class.
        /// NOTE: required for JavaScript serialization
        /// </summary>
        public DataFilter()
            : this(FilterConnector.And)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataFilter" /> class.
        /// </summary>
        /// <param name="connector">The filter connector.</param>
        public DataFilter(FilterConnector connector)
        {
            Where = new List<FilterItem>();
            Inner = new List<DataFilter>();
            Connector = connector;
        }

        /// <summary>
        /// Adds the specified filtering field.
        /// </summary>
        /// <param name="field">The filtering field.</param>
        /// <param name="value">The filtering value.</param>
        /// <param name="operation">The filtering operation.</param>
        public void Add(string field, object value, FilterOperation operation = FilterOperation.Equal)
        {
            var filterItem = new FilterItem(field, value, operation);

            Where.Add(filterItem);
        }
    }
}