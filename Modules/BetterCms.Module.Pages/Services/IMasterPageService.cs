// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IMasterPageService.cs" company="Devbridge Group LLC">
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
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.ViewModels.Option;

namespace BetterCms.Module.Pages.Services
{
    public interface IMasterPageService
    {
        IList<Guid> GetPageMasterPageIds(Guid masterPageId);

        void SetPageMasterPages(Page page, Guid masterPageId);

        void SetPageMasterPages(Page page, IList<Guid> masterPageIds);

        IList<OptionValueEditViewModel> GetMasterPageOptionValues(System.Guid id);

        /// <summary>
        /// Prepares for master page update.
        /// </summary>
        /// <param name="page">The page.</param>
        /// <param name="masterPageId">The master page identifier.</param>
        /// <param name="newMasterIds">The new master ids.</param>
        /// <param name="oldMasterIds">The old master ids.</param>
        /// <param name="childrenPageIds">The children page ids.</param>
        /// <param name="existingChildrenMasterPages">The existing children master pages.</param>
        void PrepareForUpdateChildrenMasterPages(
            PageProperties page,
            Guid? masterPageId,
            out IList<Guid> newMasterIds,
            out IList<Guid> oldMasterIds,
            out IList<Guid> childrenPageIds,
            out IList<MasterPage> existingChildrenMasterPages);

        void SetMasterOrLayout(PageProperties page, Guid? masterPageId, Guid? layoutId);

        /// <summary>
        /// Updates the children master pages.
        /// </summary>
        /// <param name="existingChildrenMasterPages">The existing children master pages.</param>
        /// <param name="oldMasterIds">The old master ids.</param>
        /// <param name="newMasterIds">The new master ids.</param>
        /// <param name="childrenPageIds">The children page ids.</param>
        void UpdateChildrenMasterPages(
            IList<MasterPage> existingChildrenMasterPages,
            IList<Guid> oldMasterIds,
            IList<Guid> newMasterIds,
            IEnumerable<Guid> childrenPageIds);

        int GetLastDynamicRegionNumber();
    }
}