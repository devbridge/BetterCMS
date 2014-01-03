using BetterCms.Core.Exceptions;
using BetterCms.Core.Mvc.Commands;
using BetterCms.Core.Web;

using BetterCms.Module.Root.Mvc;

using BetterCms.Module.Search.Helpers;
using BetterCms.Module.Search.Models;
using BetterCms.Module.Search.Services;
using BetterCms.Module.Search.ViewModels;

namespace BetterCms.Module.Search.Command.SearchQuery
{
    public class SearchQueryCommand : CommandBase, ICommand<SearchResultsViewModel, SearchResultsViewModel>
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
        public SearchResultsViewModel Execute(SearchResultsViewModel model)
        {
            model.Query = model.WidgetModel.GetSearchQueryParameter(httpContextAccessor.GetCurrent().Request, model.Query);

            if (searchService == null)
            {
                throw new CmsException("The Better CMS Search Service is not found. Please install BetterCms.Module.GoogleSiteSearch or BetterCms.Module.LuceneSearch module.");
            }

            if (!string.IsNullOrWhiteSpace(model.Query))
            {
                model.Results = searchService.Search(new SearchRequest(model.Query));
            }

            return model;
        }
    }
}