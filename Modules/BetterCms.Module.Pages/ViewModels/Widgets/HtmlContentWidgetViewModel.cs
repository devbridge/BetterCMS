// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HtmlContentWidgetViewModel.cs" company="Devbridge Group LLC">
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
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

using BetterCms.Module.Pages.Content.Resources;
using BetterCms.Module.Root.Content.Resources;
using BetterCms.Module.Root.Mvc.Attributes;

using BetterModules.Core.Models;

namespace BetterCms.Module.Pages.ViewModels.Widgets
{
    /// <summary>
    /// A widget view model.
    /// </summary>
    public class HtmlContentWidgetViewModel : EditWidgetViewModel
    {
        /// <summary>
        /// Gets or sets the content name.
        /// </summary>
        /// <value>
        /// The content name.
        /// </value>
        [DisallowHtml(ErrorMessageResourceType = typeof(RootGlobalization), ErrorMessageResourceName = "Validation_DisallowHtml_Name_Message")]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(PagesGlobalization), ErrorMessageResourceName = "HtmlContentWidget_ContentName_RequiredMessage")]
        [StringLength(MaxLength.Name, MinimumLength = 1, ErrorMessageResourceType = typeof(PagesGlobalization), ErrorMessageResourceName = "HtmlContentWidget_ContentName_MaxLengthMessage")]
        public override string Name
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the content of the page.
        /// </summary>
        /// <value>
        /// The content of the page.
        /// </value>
        [AllowHtml]
        public string PageContent { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to enable custom HTML.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to enable custom HTML; otherwise, <c>false</c>.
        /// </value>
        public bool EnableCustomHtml { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to enable custom CSS.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to enable custom CSS; otherwise, <c>false</c>.
        /// </value>
        public bool EnableCustomCSS { get; set; }

        /// <summary>
        /// Gets or sets the custom CSS.
        /// </summary>
        /// <value>
        /// The custom CSS.
        /// </value>
        [AllowHtml]
        public string CustomCSS { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to enable custom JavaScript.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to enable custom JavaScript; otherwise, <c>false</c>.
        /// </value>
        public bool EnableCustomJS { get; set; }

        /// <summary>
        /// Gets or sets the custom JavaScript.
        /// </summary>
        /// <value>
        /// The custom JavaScript.
        /// </value>
        [AllowHtml]
        public string CustomJS { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("{0}, Name: {1}, EnableCustomCSS: {2}, EnableCustomJS: {3}", base.ToString(), Name, EnableCustomCSS, EnableCustomJS);
        }
    }
}