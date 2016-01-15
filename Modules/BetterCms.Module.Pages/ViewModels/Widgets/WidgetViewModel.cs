// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WidgetViewModel.cs" company="Devbridge Group LLC">
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
using System.ComponentModel.DataAnnotations;

using BetterCms.Module.Pages.Models;
using BetterCms.Module.Pages.ViewModels.Content;
using BetterCms.Module.Root.Content.Resources;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.ViewModels.Category;

using BetterModules.Core.Models;

namespace BetterCms.Module.Pages.ViewModels.Widgets
{
    /// <summary>
    /// A widget view model.
    /// </summary>
    public class WidgetViewModel : ContentViewModel
    {
        /// <summary>
        /// Gets or sets the list of SelectedCategories Ids.
        /// </summary>
        /// <value>
        /// The list of categories Ids.
        /// </value>
        public IList<LookupKeyValue> Categories { get; set; }

        public IList<CategoryLookupModel> CategoriesLookupList { get; set; }

        /// <summary>
        /// Gets or sets the image URL.
        /// </summary>
        /// <value>
        /// The image URL.
        /// </value>        
        [StringLength(MaxLength.Url, ErrorMessageResourceType = typeof(RootGlobalization), ErrorMessageResourceName = "Validation_StringLengthAttribute_Message")]      
        public virtual string PreviewImageUrl { get; set; }

        /// <summary>
        /// Gets or sets the type of the widget.
        /// </summary>
        /// <value>
        /// The type of the widget.
        /// </value>
        public virtual WidgetType? WidgetType { get; set; }

        public override string ToString()
        {
            return string.Format("{0}, PreviewImageUrl: {1}, WidgetType: {2}", base.ToString(), PreviewImageUrl, WidgetType);
        }
    }
}