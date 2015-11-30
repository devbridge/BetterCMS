// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetInsertHtmlContentCommand.cs" company="Devbridge Group LLC">
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

using BetterModules.Core.DataAccess;

using BetterCms.Module.Pages.Services;
using BetterCms.Module.Pages.ViewModels.Content;
using BetterCms.Module.Root.Mvc;

using BetterModules.Core.Web.Mvc.Commands;

namespace BetterCms.Module.Pages.Command.Content.GetInsertHtmlContent
{
    public class GetInsertHtmlContentCommand : CommandBase, ICommand<InsertHtmlContentRequest, PageContentViewModel>
    {
        private readonly IRepository repository;

        private readonly IMasterPageService masterPageService;

        public GetInsertHtmlContentCommand(IRepository repository, IMasterPageService masterPageService)
        {
            this.masterPageService = masterPageService;
            this.repository = repository;
        }

        public PageContentViewModel Execute(InsertHtmlContentRequest request)
        {
            var isMasterPage = repository.First<Root.Models.Page>(request.PageId.ToGuidOrDefault()).IsMasterPage;
            var parentPageContentId = request.ParentPageContentId != null ? request.ParentPageContentId.ToGuidOrDefault() : Guid.Empty;

            var model = new PageContentViewModel
                {
                    PageId = Guid.Parse(request.PageId),
                    RegionId = Guid.Parse(request.RegionId),
                    ParentPageContentId = parentPageContentId,
                    LiveFrom = DateTime.Today,
                    EnableInsertDynamicRegion = isMasterPage,
                    EditInSourceMode = isMasterPage,
                    CanEditContent = true,
                    EnabledCustomCss = false,
                    EnabledCustomJs = false
                };

            if (model.EnableInsertDynamicRegion)
            {
                model.LastDynamicRegionNumber = masterPageService.GetLastDynamicRegionNumber();
            }

            return model;
        }
    }
}