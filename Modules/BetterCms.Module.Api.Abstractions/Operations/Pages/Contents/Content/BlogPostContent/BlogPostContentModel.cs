// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BlogPostContentModel.cs" company="Devbridge Group LLC">
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
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Pages.Contents.Content.BlogPostContent
{
    [DataContract]
    [System.Serializable]
    public class BlogPostContentModel : ModelBase
    {
        /// <summary>
        /// Gets or sets the content name.
        /// </summary>
        /// <value>
        /// The content name.
        /// </value>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the content HTML.
        /// </summary>
        /// <value>
        /// The content HTML.
        /// </value>
        [DataMember]
        public string Html { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether widget is published.
        /// </summary>
        /// <value>
        /// <c>true</c> if widget is published; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool IsPublished { get; set; }

        /// <summary>
        /// Gets or sets the data widget was published on.
        /// </summary>
        /// <value>
        /// The date widget was published on.
        /// </value>
        [DataMember]
        public System.DateTime? PublishedOn { get; set; }

        /// <summary>
        /// Gets or sets the widget published user name.
        /// </summary>
        /// <value>
        /// The widget published user name.
        /// </value>
        [DataMember]
        public string PublishedByUser { get; set; }
    }
}