using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.DataContracts.Enums;

using BetterCms.Module.Pages.Models;
using BetterCms.Module.Pages.ViewModels.Sitemap;
using BetterCms.Module.Root.Mvc;

using BetterModules.Core.Web.Mvc.Commands;

using NHibernate.Criterion;
using NHibernate.Transform;

namespace BetterCms.Module.Pages.Command.Sitemap.GetPageLinks
{
    /// <summary>
    /// Command to get page links data.
    /// </summary>
    public class GetPageLinksCommand : CommandBase, ICommand<string, IList<PageLinkViewModel>>
    {
        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>Sitemap root nodes.</returns>
        public IList<PageLinkViewModel> Execute(string request)
        {
            PageProperties alias = null;
            PageLinkViewModel modelAlias = null;

            var query = UnitOfWork.Session
                .QueryOver(() => alias)
                .Where(() => !alias.IsDeleted && alias.Status != PageStatus.Preview && !alias.IsMasterPage);

            if (!string.IsNullOrWhiteSpace(request))
            {
                var searchQuery = string.Format("%{0}%", request);
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

            return query.Future<PageLinkViewModel>().ToList();
        }
    }
}