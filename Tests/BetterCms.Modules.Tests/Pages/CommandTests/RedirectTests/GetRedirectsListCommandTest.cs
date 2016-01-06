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
