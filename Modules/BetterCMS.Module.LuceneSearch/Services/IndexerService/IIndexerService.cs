using System.Collections.Generic;

using BetterCMS.Module.LuceneSearch.Services.WebCrawlerService;

namespace BetterCMS.Module.LuceneSearch.Services.IndexerService
{
    public interface IIndexerService
    {
        void AddHtmlDocument(PageData pageData);

        IList<string> Search(string searchString);

        void Close();
    }
}
