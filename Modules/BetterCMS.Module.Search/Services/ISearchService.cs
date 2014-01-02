using BetterCms.Module.Search.Models;

namespace BetterCms.Module.Search.Services
{
    public interface ISearchService
    {
        SearchResults Search(SearchRequest request);
    }
}