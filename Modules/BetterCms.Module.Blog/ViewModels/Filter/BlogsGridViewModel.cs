// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BlogsGridViewModel.cs" company="Devbridge Group LLC">
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

using BetterCms.Module.Pages.ViewModels.Filter;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc.Grids;
using BetterCms.Module.Root.Mvc.Grids.GridOptions;
using BetterCms.Module.Root.ViewModels.Category;
using BetterCms.Module.Root.ViewModels.SiteSettings;

namespace BetterCms.Module.Blog.ViewModels.Filter
{
    public class BlogsGridViewModel<TModel> : SearchableGridViewModel<TModel> where TModel : IEditableGridItem
    {
        public IEnumerable<LookupKeyValue> Tags { get; set; }
        public IEnumerable<LookupKeyValue> Categories { get; set; }
        public IEnumerable<CategoryLookupModel> CategoriesLookupList { get; set; } 
        public Guid? LanguageId { get; set; }
       
        public IList<LookupKeyValue> Languages { get; set; }
        public IList<LookupKeyValue> Statuses { get; set; }
        public IList<LookupKeyValue> SeoStatuses { get; set; }
        public IList<SortAlias> SortAliases { get; set; }
        public bool IncludeArchived { get; set; }
        public PageStatusFilterType? Status { get; set; }
        public SeoStatusFilterType? SeoStatus { get; set; }

        public BlogsGridViewModel(IEnumerable<TModel> items, BlogsFilter filter, int totalCount)
            : base(items, filter, totalCount)
        {
            Tags = filter.Tags;
            Categories = filter.Categories;
            LanguageId = filter.LanguageId;
            
            IncludeArchived = filter.IncludeArchived;
            Status = filter.Status;
            SeoStatus = filter.SeoStatus;

            Statuses = PagesFilter.PageStatuses;
            SeoStatuses = PagesFilter.SeoStatuses;
            SortAliases = PagesFilter.SortAliases;
        }
    }
}