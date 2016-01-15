// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataListResponse.cs" company="Devbridge Group LLC">
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

namespace BetterCms.Module.Api.Infrastructure
{
    [DataContract]
    [Serializable]
    public class DataListResponse<TEntity>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataListResponse{TEntity}" /> class.
        /// </summary>
        public DataListResponse()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataListResponse{TEntity}" /> class.
        /// </summary>
        /// <param name="items">The items.</param>
        /// <param name="totalCount">The total count.</param>
        public DataListResponse(IList<TEntity> items = null, int totalCount = 0)
        {
            Items = items;
            TotalCount = totalCount;
        }

        /// <summary>
        /// Gets or sets the list of items.
        /// </summary>
        /// <value>
        /// The list of items.
        /// </value>
        [DataMember]
        public IList<TEntity> Items { get; set; }

        /// <summary>
        /// Gets or sets the total count.
        /// </summary>
        /// <value>
        /// The total count.
        /// </value>
        [DataMember]
        public int TotalCount { get; set; }
    }
}
