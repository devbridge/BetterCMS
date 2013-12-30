using BetterCms.Module.Search.Models;

namespace BetterCMS.Module.GoogleSiteSearch.Services.Search
{
    public interface ISearchService
    {
        SearchResults Search(SearchRequest request);
    }
}