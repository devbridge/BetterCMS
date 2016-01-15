// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetRedirectsListCommandTest.cs" company="Devbridge Group LLC">
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
using Autofac;

using BetterModules.Core.DataAccess.DataContext;
using BetterCms.Module.Pages.ViewModels.SiteSettings;
using BetterCms.Module.Root.Mvc.Grids.Extensions;
using BetterCms.Module.Root.Mvc.Grids.GridOptions;
using BetterCms.Module.Root.ViewModels.SiteSettings;

using MvcContrib.Pagination;
using MvcContrib.Sorting;

using NHibernate;
using NHibernate.Criterion;
using NHibernate.Transform;

using NUnit.Framework;

namespace BetterCms.Test.Module.Pages.CommandTests.RedirectTests
{
    [TestFixture]
    public class GetRedirectsListCommandTest : TestBase
    {
        [Test]
        public void Should_Find_Page_And_Return_ViewModel_Successfully()
        {
            // DEBUG START
            var request = new SearchableGridOptions
                {
                    SearchQuery = "a",
                    PageNumber = 2,
                    PageSize = 3,
                    Column = "PageUrl",
                    Direction = SortDirection.Descending
                };
            var sessionFactory = this.Container.Resolve<ISessionFactoryProvider>();
            // DEBUG END

            SearchableGridViewModel<SiteSettingRedirectViewModel> model;

            using (var session = sessionFactory.OpenSession())
            {
                BetterCms.Module.Pages.Models.Redirect alias = null;
                SiteSettingRedirectViewModel modelAlias = null;

                IQueryOver<BetterCms.Module.Pages.Models.Redirect, BetterCms.Module.Pages.Models.Redirect> query = session.QueryOver(() => alias);

                // Filter
                if (!string.IsNullOrWhiteSpace(request.SearchQuery))
                {
                    var searchQuery = string.Format("%{0}%", request.SearchQuery);
                    query = query
                        .Where(
                            Restrictions.InsensitiveLike(Projections.Property(() => alias.PageUrl), searchQuery) ||
                            Restrictions.InsensitiveLike(Projections.Property(() => alias.RedirectUrl), searchQuery));
                }

                // Select fields
                query = query
                    .SelectList(select => select
                        .Select(() => alias.Id).WithAlias(() => modelAlias.Id)
                        .Select(() => alias.PageUrl).WithAlias(() => modelAlias.PageUrl)
                        .Select(() => alias.RedirectUrl).WithAlias(() => modelAlias.RedirectUrl)
                        .Select(() => alias.Version).WithAlias(() => modelAlias.Version))
                    .TransformUsing(Transformers.AliasToBean<SiteSettingRedirectViewModel>());

                // Count
                var count = query.RowCount();

                // Sorting and paging
                query.AddSortingAndPaging(request);

                // Get results
                var redirects = query.List<SiteSettingRedirectViewModel>();

                model = new SearchableGridViewModel<SiteSettingRedirectViewModel>
                {                    
                    GridOptions = request,                    
                    Items = new CustomPagination<SiteSettingRedirectViewModel>(redirects, request.PageNumber, request.PageSize, count)
                };
            }

            // DEBUG START
            Assert.IsNotNull(model);
            // DEBUG END
        }
    }
}
