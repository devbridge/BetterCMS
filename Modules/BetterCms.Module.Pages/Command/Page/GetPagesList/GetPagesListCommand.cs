using System.Linq;

using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Pages.Models;
using BetterCms.Module.Pages.ViewModels.SiteSettings;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Mvc.Grids.Extensions;
using BetterCms.Module.Root.Mvc.Grids.GridOptions;
using BetterCms.Module.Root.ViewModels.SiteSettings;

using NHibernate;
using NHibernate.Criterion;
using NHibernate.Transform;

namespace BetterCms.Module.Pages.Command.Page.GetPagesList
{
    /// <summary>
    /// Command for loading  sorted/filtered list of page view models
    /// </summary>
    public class GetPagesListCommand : CommandBase, ICommand<SearchableGridOptions, SearchableGridViewModel<SiteSettingPageViewModel>>
    {
        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public SearchableGridViewModel<SiteSettingPageViewModel> Execute(SearchableGridOptions request)
        {
            SearchableGridViewModel<SiteSettingPageViewModel> model;

            request.SetDefaultSortingOptions("Title");

            PageProperties alias = null;
            SiteSettingPageViewModel modelAlias = null;

            var query = UnitOfWork.Session
                .QueryOver(() => alias)
                .Where(() => !alias.IsDeleted);

            if (!string.IsNullOrWhiteSpace(request.SearchQuery))
            {
                var searchQuery = string.Format("%{0}%", request.SearchQuery);
                query = query.Where(Restrictions.Disjunction()
                                        .Add(Restrictions.InsensitiveLike(NHibernate.Criterion.Projections.Property(() => alias.Title), searchQuery))
                                        .Add(Restrictions.InsensitiveLike(NHibernate.Criterion.Projections.Property(() => alias.PageUrl), searchQuery))
                                        .Add(Restrictions.InsensitiveLike(NHibernate.Criterion.Projections.Property(() => alias.MetaTitle), searchQuery))
                                        .Add(Restrictions.InsensitiveLike(NHibernate.Criterion.Projections.Property(() => alias.MetaDescription), searchQuery))
                                        .Add(Restrictions.InsensitiveLike(NHibernate.Criterion.Projections.Property(() => alias.MetaKeywords), searchQuery)));
            }

            IProjection hasSeoProjection = NHibernate.Criterion.Projections.Conditional(
                Restrictions.Disjunction()
                    .Add(RestrictionsExtensions.IsNullOrWhiteSpace(NHibernate.Criterion.Projections.Property(() => alias.MetaTitle)))
                    .Add(RestrictionsExtensions.IsNullOrWhiteSpace(NHibernate.Criterion.Projections.Property(() => alias.MetaKeywords)))
                    .Add(RestrictionsExtensions.IsNullOrWhiteSpace(NHibernate.Criterion.Projections.Property(() => alias.MetaDescription))),
                NHibernate.Criterion.Projections.Constant(false, NHibernateUtil.Boolean),
                NHibernate.Criterion.Projections.Constant(true, NHibernateUtil.Boolean));

            query = query
                .SelectList(select => select
                    .Select(() => alias.Id).WithAlias(() => modelAlias.Id)
                    .Select(() => alias.Version).WithAlias(() => modelAlias.Version)
                    .Select(() => alias.Title).WithAlias(() => modelAlias.Title)
                    .Select(() => alias.IsPublished).WithAlias(() => modelAlias.IsPublished)
                    .Select(hasSeoProjection).WithAlias(() => modelAlias.HasSEO)
                    .Select(() => alias.CreatedOn).WithAlias(() => modelAlias.CreatedOn)
                    .Select(() => alias.ModifiedOn).WithAlias(() => modelAlias.ModifiedOn))
                .TransformUsing(Transformers.AliasToBean<SiteSettingPageViewModel>());

            var count = query.ToRowCountFutureValue();

            var pages = query.AddSortingAndPaging(request).Future<SiteSettingPageViewModel>();

            model = new SearchableGridViewModel<SiteSettingPageViewModel>(pages.ToList(), request, count.Value);


            return model;
        }
    }
}