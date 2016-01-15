// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SiteSettingWidgetItemViewModel.cs" company="Devbridge Group LLC">
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

using BetterCms.Module.Pages.Models;
using BetterCms.Module.Root.Content.Resources;
using BetterCms.Module.Root.Mvc.Grids;

namespace BetterCms.Module.Pages.ViewModels.SiteSettings
{
    public class SiteSettingWidgetItemViewModel : IEditableGridItem
    {
        /// <summary>
        /// Gets or sets the content id.
        /// </summary>
        /// <value>
        /// The content id.
        /// </value>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the original id.
        /// </summary>
        /// <value>
        /// The original id.
        /// </value>
        public Guid OriginalId { get; set; }

        /// <summary>
        /// Gets or sets the entity version.
        /// </summary>
        /// <value>
        /// The entity version.
        /// </value>
        public int Version { get; set; }


        /// <summary>
        /// Gets or sets the original version.
        /// </summary>
        /// <value>
        /// The original version.
        /// </value>
        public int OriginalVersion { get; set; }

        /// <summary>
        /// Gets or sets the widget name.
        /// </summary>
        /// <value>
        /// The widget name.
        /// </value>
        public string WidgetName { get; set; }

        /// <summary>
        /// Gets or sets the type of the widget entity.
        /// </summary>
        /// <value>
        /// The type of the widget entity.
        /// </value>
        public Type WidgetEntityType { get; set; }

        /// <summary>
        /// Gets or sets the type of the widget.
        /// </summary>
        /// <value>
        /// The type of the widget.
        /// </value>
        public WidgetType? WidgetType { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this widget is published.
        /// </summary>
        /// <value>
        /// <c>true</c> if this widget is published; otherwise, <c>false</c>.
        /// </value>
        public bool IsPublished { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this widget has draft version.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this widget has draft; otherwise, <c>false</c>.
        /// </value>
        public bool HasDraft { get; set; }

        /// <summary>
        /// Gets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        public string Status
        {
            get
            {
                if (IsPublished && HasDraft)
                {
                    return RootGlobalization.ContentStatus_PublishedWithDraft;
                }

                if (IsPublished)
                {
                    return RootGlobalization.ContentStatus_Published;
                }

                if (HasDraft)
                {
                    return RootGlobalization.ContentStatus_Draft;
                }
                
                return null;
            }
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("Id: {0}, Version: {1}, WidgetName: {2}", Id, Version, WidgetName);
        }
        

    }
}