// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetCategoryTreesListCommand.cs" company="Devbridge Group LLC">
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
using System.Linq;

using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Mvc.Grids.Extensions;
using BetterCms.Module.Root.ViewModels;
using BetterCms.Module.Root.ViewModels.Category;
using BetterCms.Module.Root.ViewModels.SiteSettings;

using BetterModules.Core.Web.Mvc.Commands;

using NHibernate.Criterion;
using NHibernate.Transform;

namespace BetterCms.Module.Root.Commands.Category.GetCategoryTreesList
{
    public class GetCategoryTreesListCommand : CommandBase, ICommand<CategoryTreesFilter, CategoryTreesGridViewModel<SiteSettingCategoryTreeViewModel>>
    {
        public CategoryTreesGridViewModel<SiteSettingCategoryTreeViewModel> Execute(CategoryTreesFilter request)
        {
            request.SetDefaultSortingOptions("Title");

            CategoryTree alias = null;
            SiteSettingCategoryTreeViewModel modelAlias = null;
            
            var query = UnitOfWork.Session.QueryOver(() => alias).Where(() => !alias.IsDeleted);
            
            if (!string.IsNullOrWhiteSpace(request.SearchQuery))
            {
                var searchQuery = string.Format("%{0}%", request.SearchQuery);
                query = query.Where(Restrictions.Disjunction().Add(Restrictions.InsensitiveLike(NHibernate.Criterion.Projections.Property(() => alias.Title), searchQuery)));
            }
            
            query =
                query.SelectList(
                    select =>
                    select.Select(() => alias.Id)
                            .WithAlias(() => modelAlias.Id)
                            .Select(() => alias.Version)
                            .WithAlias(() => modelAlias.Version)
                            .Select(() => alias.Title)
                            .WithAlias(() => modelAlias.Title)).TransformUsing(Transformers.AliasToBean<SiteSettingCategoryTreeViewModel>());
            
            var count = query.ToRowCountFutureValue();
            
            var sitemaps = query.AddSortingAndPaging(request).Future<SiteSettingCategoryTreeViewModel>();

            return new CategoryTreesGridViewModel<SiteSettingCategoryTreeViewModel>(sitemaps.ToList(), request, count.Value);
        }
    }
}