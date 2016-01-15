// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PageTranslationModel.cs" company="Devbridge Group LLC">
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

namespace BetterCms.Module.Api.Operations.Pages.Pages.Page.Translations
{
    [DataContract]
    [System.Serializable]
    public class PageTranslationModel
    {
        /// <summary>
        /// Gets or sets the translated page id.
        /// </summary>
        /// <value>
        /// The translated page id.
        /// </value>
        [DataMember]
        public System.Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the translated page title.
        /// </summary>
        /// <value>
        /// The translated page title.
        /// </value>
        [DataMember]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the translated page url.
        /// </summary>
        /// <value>
        /// The translated page url.
        /// </value>
        [DataMember]
        public string PageUrl { get; set; }

        /// <summary>
        /// Gets or sets the translated page language id.
        /// </summary>
        /// <value>
        /// The translated page language id.
        /// </value>
        [DataMember]
        public System.Guid? LanguageId { get; set; }

        /// <summary>
        /// Gets or sets the translated page language code.
        /// </summary>
        /// <value>
        /// The translated page language code.
        /// </value>
        [DataMember]
        public string LanguageCode { get; set; }

        /// <summary>
        /// Gets or sets the page status.
        /// </summary>
        /// <value>
        /// The page status.
        /// </value>
        [DataMember]
        public bool IsPublished { get; set; }

        /// <summary>
        /// Gets or sets the page published date.
        /// </summary>
        /// <value>
        /// The page published date.
        /// </value>
        [DataMember]
        public System.DateTime? PublishedOn { get; set; }
    }
}