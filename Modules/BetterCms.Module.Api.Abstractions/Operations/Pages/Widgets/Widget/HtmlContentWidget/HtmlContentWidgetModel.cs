// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HtmlContentWidgetModel.cs" company="Devbridge Group LLC">
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
using BetterCms.Module.Api.Operations.Root.Categories.Category;

namespace BetterCms.Module.Api.Operations.Pages.Widgets.Widget.HtmlContentWidget
{
    [DataContract]
    [Serializable]
    public class HtmlContentWidgetModel : ModelBase
    {
        /// <summary>
        /// Gets or sets the widget name.
        /// </summary>
        /// <value>
        /// The widget name.
        /// </value>
        [DataMember]
        public string Name { get; set; }

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

        /// <summary>
        /// Gets or sets the custom CSS.
        /// </summary>
        /// <value>
        /// The custom CSS.
        /// </value>
        [DataMember]
        public string CustomCss { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to use custom CSS.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to use custom CSS; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool UseCustomCss { get; set; }

        /// <summary>
        /// Gets or sets the HTML.
        /// </summary>
        /// <value>
        /// The HTML.
        /// </value>
        [DataMember]
        public string Html { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to use HTML.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to use HTML; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool UseHtml { get; set; }

        /// <summary>
        /// Gets or sets the custom JavaScripts.
        /// </summary>
        /// <value>
        /// The custom js.
        /// </value>
        [DataMember]
        public string CustomJavaScript { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to use custom JavaScript.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to use custom JavaScript; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool UseCustomJavaScript { get; set; }
    }
}