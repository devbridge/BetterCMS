// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetSitemapsModel.cs" company="Devbridge Group LLC">
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

using BetterCms.Module.Api.Infrastructure;
using BetterCms.Module.Api.Infrastructure.Enums;

namespace BetterCms.Module.Api.Operations.Pages.Sitemaps
{
    /// <summary>
    /// Data model for getting sitemaps list.
    /// </summary>
    [DataContract]
    [Serializable]
    public class GetSitemapsModel : DataOptions, IFilterByTags
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetSitemapsModel" /> class.
        /// </summary>
        public GetSitemapsModel()
        {
            FilterByTagsConnector = FilterConnector.And;
        }

        /// <summary>
        /// Gets or sets the tags.
        /// </summary>
        /// <value>
        /// The tags.
        /// </value>
        [DataMember]
        public System.Collections.Generic.List<string> FilterByTags { get; set; }

        /// <summary>
        /// Gets or sets the tags filter connector.
        /// </summary>
        /// <value>
        /// The tags filter connector.
        /// </value>
        [DataMember]
        public FilterConnector FilterByTagsConnector { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to include tags collections to results.
        /// </summary>
        /// <value>
        ///   <c>true</c> if  to include tags collections to results; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool IncludeTags { get; set; }
    }
}