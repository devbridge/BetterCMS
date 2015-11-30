// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetRedirectsListCommand.cs" company="Devbridge Group LLC">
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

using BetterCms.Module.Pages.ViewModels.SiteSettings;

using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Mvc.Grids.Extensions;
using BetterCms.Module.Root.Mvc.Grids.GridOptions;
using BetterCms.Module.Root.ViewModels.SiteSettings;

using BetterModules.Core.Web.Mvc.Commands;

using NHibernate.Criterion;
using NHibernate.Transform;

namespace BetterCms.Module.Pages.Command.Redirect.GetRedirectsList
{
    /// <summary>
    /// Command for loading  sorted/filtered list of redirect view models
    /// </summary>
    public class GetRedirectsListCommand : CommandBase, ICommand<SearchableGridOptions, SearchableGridViewModel<SiteSettingRedirectViewModel>>
    {
        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="options">The request.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public SearchableGridViewModel<SiteSettingRedirectViewModel> Execute(SearchableGridOptions options)
        {
            SearchableGridViewModel<SiteSettingRedirectViewModel> model;

            options.SetDefaultSortingOptions("PageUrl");

            Models.Redirect alias = null;
            SiteSettingRedirectViewModel modelAlias = null;

            var query = UnitOfWork.Session.QueryOver(() => alias).Where(() => !alias.IsDeleted);

            if (!string.IsNullOrWhiteSpace(options.SearchQuery))
            {
                var searchQuery = string.Format("%{0}%", options.SearchQuery);
                query =
                    query.Where(
                        Restrictions.InsensitiveLike(Projections.Property(() => alias.PageUrl), searchQuery)
                        || Restrictions.InsensitiveLike(Projections.Property(() => alias.RedirectUrl), searchQuery));
            }

            query =
                query.SelectList(
                    select =>
                    select.Select(() => alias.Id)
                          .WithAlias(() => modelAlias.Id)
                          .Select(() => alias.PageUrl)
                          .WithAlias(() => modelAlias.PageUrl)
                          .Select(() => alias.RedirectUrl)
                          .WithAlias(() => modelAlias.RedirectUrl)
                          .Select(() => alias.Version)
                          .WithAlias(() => modelAlias.Version)).TransformUsing(Transformers.AliasToBean<SiteSettingRedirectViewModel>());

            var count = query.ToRowCountFutureValue();

            var redirects = query.AddSortingAndPaging(options).Future<SiteSettingRedirectViewModel>();

            model = new SearchableGridViewModel<SiteSettingRedirectViewModel>(redirects.ToList(), options, count.Value);
            
            return model;
        }
    }
}