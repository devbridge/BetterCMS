// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IContentService.cs" company="Devbridge Group LLC">
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
using BetterCms.Core.DataContracts.Enums;
using BetterCms.Module.Root.Models;

namespace BetterCms.Module.Root.Services
{
    public interface IContentService
    {
        Tuple<PageContent, Models.Content> GetPageContentForEdit(Guid pageContentId);

        Models.Content GetContentForEdit(Guid contentId);

        Models.Content SaveContentWithStatusUpdate(Models.Content updatedContent, ContentStatus requestedStatus);

        Models.Content RestoreContentFromArchive(Models.Content restoreFrom);

        int GetPageContentNextOrderNumber(Guid pageId, Guid? parentPageContentId);

        void PublishDraftContent(Guid pageId);

        bool CheckIfContentHasDeletingChildren(Guid? pageId, Guid contentId, string html = null);

        void CheckIfContentHasDeletingChildrenWithException(Guid? pageId, Guid contentId, string html = null);

        void UpdateDynamicContainer(Models.Content content);

        TEntity GetDraftOrPublishedContent<TEntity>(TEntity content) where TEntity : Models.Content;
    }
}
