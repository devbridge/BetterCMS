// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SitemapTreeNodeTranslationModel.cs" company="Devbridge Group LLC">
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
using BetterCms.Module.Api.Infrastructure.Attributes;

namespace BetterCms.Module.Api.Operations.Pages.Sitemap.Tree
{
    [Obsolete("Use everything from BetterCms.Module.Api.Operations.Pages.Sitemaps name space.")]
    [DataContract]
    [Serializable]
    public class SitemapTreeNodeTranslationModel : ModelBase
    {
        /// <summary>
        /// Gets or sets the parent node id.
        /// </summary>
        /// <value>
        /// The parent node id.
        /// </value>
        [DataMember]
        public Guid LanguageId { get; set; }

        /// <summary>
        /// Gets or sets the node title.
        /// </summary>
        /// <value>
        /// The node title.
        /// </value>
        [DataMember]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the node URL.
        /// </summary>
        /// <value>
        /// The node URL.
        /// </value>
        [DataMember]
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets the node id.
        /// </summary>
        /// <value>
        /// The node id.
        /// </value>
        [DataMemberIgnore]
        public Guid NodeId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to use page title as node title.
        /// </summary>
        /// <value>
        /// <c>true</c> if to use page title as node title; otherwise, <c>false</c>.
        /// </value>
        [DataMemberIgnore]
        public bool UsePageTitleAsNodeTitle { get; set; }

        /// <summary>
        /// Gets or sets the macro.
        /// </summary>
        /// <value>
        /// The macro.
        /// </value>
        [DataMember]
        public string Macro { get; set; }
    }
}