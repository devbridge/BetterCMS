using System.Linq;

using BetterCms.Core.Mvc.Commands;

using BetterCms.Module.Blog.ViewModels.Author;
using BetterCms.Module.Pages.Models;

using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Mvc.Grids.Extensions;
using BetterCms.Module.Root.Mvc.Grids.GridOptions;
using BetterCms.Module.Root.ViewModels.SiteSettings;

using NHibernate.Criterion;
using NHibernate.Transform;

namespace BetterCms.Module.Blog.Commands.GetAuthorList
{
    public class GetAuthorListCommand : CommandBase, ICommand<SearchableGridOptions, SearchableGridViewModel<AuthorViewModel>>
    {
        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>List with blog post view models</returns>
        public SearchableGridViewModel<AuthorViewModel> Execute(SearchableGridOptions request)
        {
            SearchableGridViewModel<AuthorViewModel> model;

            request.SetDefaultSortingOptions("Title");

            Author alias = null;
            AuthorViewModel modelAlias = null;

            var query = UnitOfWork.Session
                .QueryOver(() => alias)
                .Where(() => !alias.IsDeleted);

            if (!string.IsNullOrWhiteSpace(request.SearchQuery))
            {
                var searchQuery = string.Format("%{0}%", request.SearchQuery);
                query = query.Where(Restrictions.InsensitiveLike(Projections.Property(() => alias.DisplayName), searchQuery));
            }

            query = query
                .SelectList(select => select
                    .Select(() => alias.Id).WithAlias(() => modelAlias.Id)
                    .Select(() => alias.DisplayName).WithAlias(() => modelAlias.Name)
                    .Select(() => alias.Version).WithAlias(() => modelAlias.Version))
                .TransformUsing(Transformers.AliasToBean<AuthorViewModel>());

            var count = query.ToRowCountFutureValue();

            var authors = query.AddSortingAndPaging(request).Future<AuthorViewModel>();

            model = new SearchableGridViewModel<AuthorViewModel>(authors.ToList(), request, count.Value);

            return model;
        }
    }
}