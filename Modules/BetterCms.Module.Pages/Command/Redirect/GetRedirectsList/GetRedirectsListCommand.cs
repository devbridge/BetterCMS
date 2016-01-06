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