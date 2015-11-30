// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ILayoutService.cs" company="Devbridge Group LLC">
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

using BetterCms.Module.Pages.ViewModels.Page;
using BetterCms.Module.Pages.ViewModels.Templates;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.ViewModels.Option;

namespace BetterCms.Module.Pages.Services
{
    public interface ILayoutService
    {
        /// <summary>
        /// Gets the future query for the list of layout view models.
        /// </summary>
        /// <param name="currentPageId">The current page id.</param>
        /// <param name="currentPageMasterPageId">The current page master page identifier.</param>
        /// <returns>
        /// The future query for the list of layout view models
        /// </returns>
        IList<TemplateViewModel> GetAvailableLayouts(System.Guid? currentPageId = null, System.Guid? currentPageMasterPageId = null);

        /// <summary>
        /// Gets the list of layout option view models.
        /// </summary>
        /// <param name="id">The layout id.</param>
        /// <returns>
        /// The list of layout option view models
        /// </returns>
        IList<OptionViewModel> GetLayoutOptions(System.Guid id);

        /// <summary>
        /// Gets the list of layout option values.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>
        /// The list of layout option values.
        /// </returns>
        IList<OptionValueEditViewModel> GetLayoutOptionValues(System.Guid id);

        /// <summary>
        /// Saves the layout.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="treatNullsAsLists">if set to <c>true</c> treat null lists as empty lists.</param>
        /// <param name="createIfNotExists">if set to <c>true</c> create if not exists.</param>
        /// <returns>
        /// Saved layout entity
        /// </returns>
        Layout SaveLayout(TemplateEditViewModel model, bool treatNullsAsLists = true, bool createIfNotExists = false);

        /// <summary>
        /// Deletes the layout.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="version">The version.</param>
        /// <returns><c>true</c>, if delete was successful, otherwise <c>false</c></returns>
        bool DeleteLayout(Guid id, int version);
    }
}