using System.Collections.Generic;
using System.Globalization;
using System.Linq;

using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Root.Models.Extensions;
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

            var culturesFuture = query
                .Select(culture =>
                    new CultureViewModel
                        {
                            Id = culture.Id,
                            Version = culture.Version,
                            Name = culture.Name,
                            Code = culture.Code
                        });

            var count = query.ToRowCountFutureValue();
            culturesFuture = culturesFuture.AddSortingAndPaging(request);

            var cultures = culturesFuture.ToList();
            SetCultureCodes(cultures);
            model = new SearchableGridViewModel<CultureViewModel>(cultures, request, count.Value);

            return model;
        }

        private void SetCultureCodes(List<CultureViewModel> cultures)
        {
            var codes = cultures.Select(c => c.Code).ToArray();
            var names = CultureInfo.GetCultures(CultureTypes.AllCultures)
                .Where(c => codes.Contains(c.Name))
                .ToDictionary(c => c.Name, c => c.GetFullName());

            foreach (var pair in names)
            {
                cultures.Where(c => c.Code == pair.Key).ToList().ForEach(c => c.Code = pair.Value);
            }
        }
    }
}