using System.Collections.Generic;
using System.Globalization;
using System.Linq;

using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Core.Mvc.Commands;

using BetterCms.Module.Root.Models.Extensions;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Mvc.Grids.Extensions;
using BetterCms.Module.Root.Mvc.Grids.GridOptions;
using BetterCms.Module.Root.ViewModels.Language;
using BetterCms.Module.Root.ViewModels.SiteSettings;

namespace BetterCms.Module.Root.Commands.Language.GetLanguageList
{
    public class GetLanguageListCommand : CommandBase, ICommand<SearchableGridOptions, SearchableGridViewModel<LanguageViewModel>>
    {
        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>List with languages view models</returns>
        public SearchableGridViewModel<LanguageViewModel> Execute(SearchableGridOptions request)
        {
            SearchableGridViewModel<LanguageViewModel> model;

            request.SetDefaultSortingOptions("Name");

            var query = Repository
                .AsQueryable<Models.Language>();

            if (!string.IsNullOrWhiteSpace(request.SearchQuery))
            {
                query = query.Where(language => language.Name.Contains(request.SearchQuery) || language.Code.Contains(request.SearchQuery));
            }

            var languagesFuture = query
                .Select(language =>
                    new LanguageViewModel
                        {
                            Id = language.Id,
                            Version = language.Version,
                            Name = language.Name,
                            Code = language.Code
                        });

            var count = query.ToRowCountFutureValue();
            languagesFuture = languagesFuture.AddSortingAndPaging(request);

            var languages = languagesFuture.ToList();
            SetLanguageCodes(languages);
            model = new SearchableGridViewModel<LanguageViewModel>(languages, request, count.Value);

            return model;
        }

        private void SetLanguageCodes(List<LanguageViewModel> languages)
        {
            var codes = languages.Select(c => c.Code).ToArray();
            var names = CultureInfo.GetCultures(CultureTypes.AllCultures)
                .Where(c => codes.Contains(c.Name))
                .ToDictionary(c => c.Name, c => c.GetFullName());

            foreach (var pair in names)
            {
                languages.Where(c => c.Code == pair.Key).ToList().ForEach(c => c.Code = pair.Value);
            }
        }
    }
}