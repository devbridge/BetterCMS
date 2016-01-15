// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FilterItem.cs" company="Devbridge Group LLC">
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
    /// Represents class for filtering items
    /// </summary>
    [DataContract]
    [Serializable]
    public class FilterItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FilterItem" /> class.
        /// NOTE: required for JavaScript serialization
        /// </summary>
        public FilterItem()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FilterItem" /> class.
        /// </summary>
        /// <param name="field">The filtering field.</param>
        /// <param name="value">The filtering value.</param>
        /// <param name="operation">The filtering operation.</param>
        public FilterItem(string field, object value, FilterOperation operation = FilterOperation.Equal)
        {
            Field = field;
            Value = value;
            Operation = operation;
        }

        /// <summary>
        /// Gets or sets the filtering field.
        /// </summary>
        /// <value>
        /// The filtering field.
        /// </value>
        [DataMember]
        public string Field { get; set; }

        /// <summary>
        /// Gets or sets the filtering value.
        /// </summary>
        /// <value>
        /// The filtering value.
        /// </value>
        [DataMember]
        public object Value { get; set; }

        /// <summary>
        /// Gets or sets the filtering operation.
        /// </summary>
        /// <value>
        /// The filtering operation.
        /// </value>
        [DataMember]
        public FilterOperation Operation { get; set; }
    }
}