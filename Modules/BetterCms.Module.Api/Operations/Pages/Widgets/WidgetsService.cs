// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WidgetsService.cs" company="Devbridge Group LLC">
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
using System.Linq;

using BetterModules.Core.DataAccess;
using BetterCms.Core.DataContracts.Enums;
using BetterCms.Module.Api.Helpers;
using BetterCms.Module.Api.Infrastructure;
using BetterCms.Module.Api.Operations.Root.Categories.Category;
using BetterCms.Module.Root.Services;

using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Api.Operations.Pages.Widgets
{
    public class WidgetsService : Service, IWidgetsService
    {
        private readonly IRepository repository;
        private readonly ICategoryService categoryService;

        public WidgetsService(IRepository repository, ICategoryService categoryService)
        {
            this.repository = repository;
            this.categoryService = categoryService;
        }

        public GetWidgetsResponse Get(GetWidgetsRequest request)
        {
            request.Data.SetDefaultOrder("Name");

            var query = repository
                .AsQueryable<Module.Root.Models.Widget>()
                .Where(widget => widget.Original == null);

            if (!request.Data.IncludeUnpublished)
            {
                query = query.Where(widget => widget.Status == ContentStatus.Published);
            }

            query.ApplyCategoriesFilter(categoryService, request.Data);

            var listResponse = query
                 .Select(widget => new WidgetModel
                 {
                     Id = widget.Id,
                     Version = widget.Version,
                     CreatedBy = widget.CreatedByUser,
                     CreatedOn = widget.CreatedOn,
                     LastModifiedBy = widget.ModifiedByUser,
                     LastModifiedOn = widget.ModifiedOn,

                     Name = widget.Name,
                     IsPublished = widget.Status == ContentStatus.Published,
                     PublishedOn = widget.PublishedOn,
                     PublishedByUser = widget.PublishedByUser,
                     OriginalWidgetType = widget.GetType()
                 }).ToDataListResponse(request);

            // Set content types
            listResponse.Items.ToList().ForEach(
                item =>
                {
                    item.WidgetType = item.OriginalWidgetType.ToContentTypeString();

                    item.Categories = (from pagePr in repository.AsQueryable<Module.Root.Models.Widget>()
                                       from category in pagePr.Categories
                                       where pagePr.Id == item.Id && !category.IsDeleted
                                       select new CategoryModel
                                       {
                                           Id = category.Category.Id,
                                           Version = category.Version,
                                           CreatedBy = category.CreatedByUser,
                                           CreatedOn = category.CreatedOn,
                                           LastModifiedBy = category.ModifiedByUser,
                                           LastModifiedOn = category.ModifiedOn,
                                           Name = category.Category.Name
                                       }).ToList();
                });

            return new GetWidgetsResponse
                       {
                           Data = listResponse
                       };
        }
    }
}