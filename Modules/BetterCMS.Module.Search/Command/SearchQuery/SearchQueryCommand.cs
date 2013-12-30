using BetterCMS.Module.GoogleSiteSearch.Services.Search;

using BetterCms.Core.Exceptions;
using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Search.Models;

namespace BetterCms.Module.Search.Command.SearchQuery
{
    public class SearchQueryCommand : CommandBase, ICommand<SearchRequest, SearchResults>
    {
        /// <summary>
        /// Gets or sets the search service.
        /// </summary>
        /// <value>
        /// The search service.
        /// </value>
        public ISearchService SearchService { get; set; }

        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public SearchResults Execute(SearchRequest request)
        {
            if (SearchService == null)
            {
                throw new CmsException("The Better CMS Search Service is not found. Please install BetterCms.Module.GoogleSiteSearch or BetterCms.Module.LuceneSearch module.");
            }

            return SearchService.Search(request);
        }
    }
}