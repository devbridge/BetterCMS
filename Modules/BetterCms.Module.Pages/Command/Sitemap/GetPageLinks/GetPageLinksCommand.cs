using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.DataContracts.Enums;
using BetterCms.Core.Models;
using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Pages.Models;
using BetterCms.Module.Pages.ViewModels.Sitemap;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Mvc.Grids.Extensions;
using BetterCms.Module.Root.Mvc.Grids.GridOptions;
using BetterCms.Module.Root.ViewModels.SiteSettings;

using NHibernate.Criterion;
using NHibernate.Transform;

namespace BetterCms.Module.Pages.Command.Sitemap.GetPageLinks
{
    /// <summary>
    /// Command to get page links data.
    /// </summary>
    public class GetPageLinksCommand : CommandBase, ICommand<SearchableGridOptions, SearchableGridViewModel<PageLinkViewModel>>
    {
        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>Sitemap root nodes.</returns>
        public SearchableGridViewModel<PageLinkViewModel> Execute(SearchableGridOptions request)
        {
            PageProperties alias = null;
            PageLinkViewModel modelAlias = null;

            SearchableGridViewModel<PageLinkViewModel> model = null;

            var query = UnitOfWork.Session
                .QueryOver(() => alias)
                .Where(() => !alias.IsDeleted && alias.Status != PageStatus.Preview && !alias.IsMasterPage);

            if (request != null && !string.IsNullOrWhiteSpace(request.SearchQuery))
            {
                var searchQuery = string.Format("%{0}%", request.SearchQuery);
                query = query.Where(Restrictions.Disjunction()
                                        .Add(Restrictions.InsensitiveLike(Projections.Property(() => alias.Title), searchQuery))
                                        .Add(Restrictions.InsensitiveLike(Projections.Property(() => alias.PageUrl), searchQuery)));
            }

            query = query
                .SelectList(select => select
                    .Select(() => alias.Id).WithAlias(() => modelAlias.Id)
                    .Select(() => alias.Title).WithAlias(() => modelAlias.Title)
                    .Select(() => alias.PageUrl).WithAlias(() => modelAlias.Url)
                    .Select(() => alias.Language.Id).WithAlias(() => modelAlias.LanguageId))
                .TransformUsing(Transformers.AliasToBean<PageLinkViewModel>());

            query.UnderlyingCriteria.AddOrder(new Order("Title", true));

            var count = query.ToRowCountFutureValue();
            var pageLinks = query.AddSortingAndPaging(request).Future<PageLinkViewModel>();

            model = new SearchableGridViewModel<PageLinkViewModel>(pageLinks.ToList(), request, count.Value);
            return model;
        }
    }
}