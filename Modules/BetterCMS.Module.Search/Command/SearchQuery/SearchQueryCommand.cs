using BetterCms.Core.Exceptions;

using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Mvc.Helpers;

using BetterCms.Module.Search.Helpers;
using BetterCms.Module.Search.Models;
using BetterCms.Module.Search.Services;
using BetterCms.Module.Search.ViewModels;

using BetterModules.Core.Web.Mvc.Commands;
using BetterModules.Core.Web.Web;

namespace BetterCms.Module.Search.Command.SearchQuery
{
    public class SearchQueryCommand : CommandBase, ICommand<SearchRequestViewModel, SearchResultsViewModel>
    {
        private readonly ISearchService searchService;

        private readonly IHttpContextAccessor httpContextAccessor;

        public SearchQueryCommand(ISearchService searchService, IHttpContextAccessor httpContextAccessor)
        {
            this.searchService = searchService;
            this.httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="model">The request.</param>
        /// <returns></returns>
        public SearchResultsViewModel Execute(SearchRequestViewModel model)
        {
            var query = model.WidgetModel.GetSearchQueryParameter(httpContextAccessor.GetCurrent().Request, model.Query);
            SearchResults results;

            if (searchService == null)
            {
                throw new CmsException("The Better CMS Search Service is not found. Please install BetterCms.Module.GoogleSiteSearch or BetterCms.Module.LuceneSearch module.");
            }

            if (!string.IsNullOrWhiteSpace(query))
            {
                var take = model.WidgetModel.GetOptionValue<int>(SearchModuleConstants.WidgetOptionNames.ResultsCount);
                if (take <= 0)
                {
                    take = SearchModuleConstants.DefaultSearchResultsCount;
                }
                results = searchService.Search(new SearchRequest(query, take, model.Skip));
            }
            else
            {
                results = new SearchResults();
            }

            return new SearchResultsViewModel
                       {
                           Results = results,
                           WidgetViewModel = model.WidgetModel
                       };
        }
    }
}