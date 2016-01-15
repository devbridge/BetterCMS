// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EditWidgetViewModel.cs" company="Devbridge Group LLC">
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

using BetterCms.Core.DataContracts.Enums;
using BetterCms.Core.Mvc.Binders;
using BetterCms.Module.Root.Models;

using Newtonsoft.Json;

namespace BetterCms.Module.Pages.ViewModels.Widgets
{
    /// <summary>
    /// Editable widget view model
    /// </summary>
    public class EditWidgetViewModel : WidgetViewModel
    {
        /// <summary>
        /// Gets or sets the current status for the saved widget.
        /// </summary>
        /// <value>
        /// The current status.
        /// </value>
        public ContentStatus CurrentStatus { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether content has original published content.
        /// </summary>
        /// <value>
        /// <c>true</c> if content has published original content; otherwise, <c>false</c>.
        /// </value>
        public bool HasPublishedContent { get; set; }

        /// <summary>
        /// Gets or sets the desirable status for the saved widget.
        /// </summary>
        /// <value>
        /// The desirable status.
        /// </value>
        public ContentStatus DesirableStatus { get; set; }

        /// <summary>
        /// Gets or sets the page published date.
        /// </summary>
        /// <value>
        /// The page published date.
        /// </value>
        public System.DateTime? PublishedOn { get; set; }

        /// <summary>
        /// Gets or sets the published by user.
        /// </summary>
        /// <value>
        /// The published by user.
        /// </value>
        public string PublishedByUser { get; set; }

        /// <summary>
        /// Gets or sets a value whether user confirmed content saving when affecting children pages.
        /// </summary>
        /// <value>
        /// <c>true</c> if user confirmed content saving when affecting children pages; otherwise, <c>false</c>.
        /// </value>
        public bool IsUserConfirmed { get; set; }

        /// <summary>
        /// Gets or sets the languages.
        /// </summary>
        /// <value>
        /// The languages.
        /// </value>
        public List<LookupKeyValue> Languages { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to show languages.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [show languages]; otherwise, <c>false</c>.
        /// </value>
        public bool ShowLanguages { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("{0}, CurrentStatus: {1}, DesirableStatus: {2}, HasPublishedContent: {3}", base.ToString(), CurrentStatus, DesirableStatus, HasPublishedContent);
        }
    }
}