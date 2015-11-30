// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetPagesModel.cs" company="Devbridge Group LLC">
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

using BetterCms.Module.Api.Infrastructure;
using BetterCms.Module.Api.Infrastructure.Enums;

namespace BetterCms.Module.Api.Operations.Pages.Pages
{
    [DataContract]
    [System.Serializable]
    public class GetPagesModel : DataOptions, IFilterByTags, IFilterByCategories
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetPagesModel" /> class.
        /// </summary>
        public GetPagesModel()
        {
            FilterByTagsConnector = FilterConnector.And;
        }

        /// <summary>
        /// Gets or sets a value indicating whether to include archived pages.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to include archived pages; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool IncludeArchived { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to include unpublished pages.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to include unpublished pages; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool IncludeUnpublished { get; set; }

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
        /// Gets or sets a value indicating whether to include page options collections to results.
        /// </summary>
        /// <value>
        ///   <c>true</c> if  to include page options collections to results; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool IncludePageOptions { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to include tags collections to results.
        /// </summary>
        /// <value>
        ///   <c>true</c> if  to include tags collections to results; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool IncludeTags { get; set; }
        
        /// <summary>
        /// Gets or sets a value indicating whether to include metadata to results.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to include metadata to results; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool IncludeMetadata { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to include master pages.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to include master pages to results; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool IncludeMasterPages { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to include access rules.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to include access rules; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool IncludeAccessRules { get; set; }
        
        /// <summary>
        /// Gets or sets a value indicating whether to include Categories.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to includecategories; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool IncludeCategories { get; set; }

        /// <summary>
        /// Gets or sets the categories.
        /// </summary>
        /// <value>
        /// The tags.
        /// </value>
        [DataMember]
        public List<Guid> FilterByCategories { get; set; }

        /// <summary>
        /// Gets or sets the categories names.
        /// </summary>
        /// <value>
        /// The tags.
        /// </value>
        [DataMember]
        public List<string> FilterByCategoriesNames { get; set; }

        /// <summary>
        /// Gets or sets the categories filter connector.
        /// </summary>
        /// <value>
        /// The tags filter connector.
        /// </value>
        [DataMember]
        public FilterConnector FilterByCategoriesConnector { get; set; }
    }
}