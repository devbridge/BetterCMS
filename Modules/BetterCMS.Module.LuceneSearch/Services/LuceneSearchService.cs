using BetterCMS.Module.LuceneSearch.Services.IndexerService;

using BetterCms.Module.Search.Models;
using BetterCms.Module.Search.Services;

namespace BetterCMS.Module.LuceneSearch.Services
{
    public class LuceneSearchService : ISearchService
    {
        private readonly IIndexerService indexerService;

        public LuceneSearchService(IIndexerService indexerService)
        {
            this.indexerService = indexerService;
        }

        public SearchResults Search(SearchRequest request)
        {
            var results = indexerService.Search(request.Query);

            return new SearchResults
                       {
                           Items = results
                       };
        }
    }
}
