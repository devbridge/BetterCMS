using System.Collections.Generic;

using BetterCMS.Module.LuceneSearch.Services.WebCrawlerService;

using BetterCms.Module.Search;
using BetterCms.Module.Search.Models;

namespace BetterCMS.Module.LuceneSearch.Services.IndexerService
{
    public interface IIndexerService
    {
        void AddHtmlDocument(PageData pageData);

        IList<SearchResultItem> Search(string searchString, int resultCount = SearchModuleConstants.DefaultSearchResultsCount);

        void Open(bool create = false);

        void Close();
    }
}
