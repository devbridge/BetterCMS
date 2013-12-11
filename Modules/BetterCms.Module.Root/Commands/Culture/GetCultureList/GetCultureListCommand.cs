using System.Linq;

using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Core.Mvc.Commands;

using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Mvc.Grids.Extensions;
using BetterCms.Module.Root.Mvc.Grids.GridOptions;
using BetterCms.Module.Root.ViewModels.Cultures;
using BetterCms.Module.Root.ViewModels.SiteSettings;

namespace BetterCms.Module.Root.Commands.Culture.GetCultureList
{
    public class GetCultureListCommand : CommandBase, ICommand<SearchableGridOptions, SearchableGridViewModel<CultureViewModel>>
    {
        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>List with cultures view models</returns>
        public SearchableGridViewModel<CultureViewModel> Execute(SearchableGridOptions request)
        {
            SearchableGridViewModel<CultureViewModel> model;

            request.SetDefaultSortingOptions("Name");

            var query = Repository
                .AsQueryable<Models.Culture>();

            if (!string.IsNullOrWhiteSpace(request.SearchQuery))
            {
                query = query.Where(culture => culture.Name.Contains(request.SearchQuery) || culture.Code.Contains(request.SearchQuery));
            }

            var cultures = query
                .Select(culture =>
                    new CultureViewModel
                        {
                            Id = culture.Id,
                            Version = culture.Version,
                            Name = culture.Name,
                            Code = culture.Code
                        });

            var count = query.ToRowCountFutureValue();
            cultures = cultures.AddSortingAndPaging(request);

            model = new SearchableGridViewModel<CultureViewModel>(cultures.ToList(), request, count.Value);

            return model;
        }
    }
}