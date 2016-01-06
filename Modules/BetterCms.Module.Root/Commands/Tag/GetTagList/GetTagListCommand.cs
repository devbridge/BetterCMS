using System.Linq;

using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Mvc.Grids.Extensions;
using BetterCms.Module.Root.Mvc.Grids.GridOptions;
using BetterCms.Module.Root.ViewModels.SiteSettings;
using BetterCms.Module.Root.ViewModels.Tags;

using BetterModules.Core.Web.Mvc.Commands;

using NHibernate.Criterion;
using NHibernate.Transform;

namespace BetterCms.Module.Root.Commands.Tag.GetTagList
{
    /// <summary>
    /// A command to get tag list by filter.
    /// </summary>
    public class GetTagListCommand : CommandBase, ICommand<SearchableGridOptions, SearchableGridViewModel<TagItemViewModel>>
    {
        /// <summary>
        /// Executes this command.
        /// </summary>
        /// <param name="request">A filter to search for specific tags.</param>
        /// <returns>A list of tags.</returns>
        public SearchableGridViewModel<TagItemViewModel> Execute(SearchableGridOptions request)
        {
            SearchableGridViewModel<TagItemViewModel> model;

            request.SetDefaultSortingOptions("Name");

            Root.Models.Tag alias = null;
            TagItemViewModel modelAlias = null;

            var query = UnitOfWork.Session
                .QueryOver(() => alias)
                .Where(() => !alias.IsDeleted);

            if (!string.IsNullOrWhiteSpace(request.SearchQuery))
            {
                var searchQuery = string.Format("%{0}%", request.SearchQuery);
                query = query.Where(Restrictions.InsensitiveLike(NHibernate.Criterion.Projections.Property(() => alias.Name), searchQuery));
            }

            query = query
                .SelectList(select => select
                    .Select(() => alias.Id).WithAlias(() => modelAlias.Id)
                    .Select(() => alias.Name).WithAlias(() => modelAlias.Name)
                    .Select(() => alias.Version).WithAlias(() => modelAlias.Version))
                .TransformUsing(Transformers.AliasToBean<TagItemViewModel>());

            var count = query.ToRowCountFutureValue();

            var tags = query.AddSortingAndPaging(request).Future<TagItemViewModel>();

            model = new SearchableGridViewModel<TagItemViewModel>(tags.ToList(), request, count.Value);

            return model;
        }
    }
}