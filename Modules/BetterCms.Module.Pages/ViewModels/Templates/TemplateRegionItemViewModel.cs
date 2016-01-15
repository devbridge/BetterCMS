// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TemplateRegionItemViewModel.cs" company="Devbridge Group LLC">
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
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

using BetterCms.Module.Root.Content.Resources;
using BetterCms.Module.Root.Mvc.Attributes;
using BetterCms.Module.Root.Mvc.Grids;

using BetterModules.Core.Models;

namespace BetterCms.Module.Pages.ViewModels.Templates
{
    public class TemplateRegionItemViewModel : IEditableGridItem, IEquatable<TemplateRegionItemViewModel> 
    {
        /// <summary>
        /// Gets or sets the region id.
        /// </summary>
        /// <value>
        /// The region id.
        /// </value>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the region version.
        /// </summary>
        /// <value>
        /// The region version.
        /// </value>
        public int Version { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        [AllowHtml]
        [DisallowHtml(ErrorMessageResourceType = typeof(RootGlobalization), ErrorMessageResourceName = "Validation_DisallowHtml_Field_Message")]
        [StringLength(MaxLength.Name, ErrorMessageResourceType = typeof(RootGlobalization), ErrorMessageResourceName = "Validation_StringLengthAttribute_Message")]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the region identifier.
        /// </summary>
        /// <value>
        /// The region identifier.
        /// </value>
        [AllowHtml]
        [DisallowHtml(ErrorMessageResourceType = typeof(RootGlobalization), ErrorMessageResourceName = "Validation_DisallowHtml_Field_Message")]
        [Required(ErrorMessageResourceType = typeof(RootGlobalization), ErrorMessageResourceName = "Validation_RequiredAttribute_Message")]
        [StringLength(MaxLength.Name, ErrorMessageResourceType = typeof(RootGlobalization), ErrorMessageResourceName = "Validation_StringLengthAttribute_Message")]
        public string Identifier { get; set; }

        public bool Equals(TemplateRegionItemViewModel other)
        {
            return Identifier == other.Identifier;
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("Id: {0}, Version: {1}, Description: {2}, Identifier: {3}", Id, Version, Description, Identifier);
        }
    }
}