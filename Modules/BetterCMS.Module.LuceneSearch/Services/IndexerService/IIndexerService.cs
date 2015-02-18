using BetterCMS.Module.LuceneSearch.Services.WebCrawlerService;

using BetterCms.Module.Search.Services;

namespace BetterCMS.Module.LuceneSearch.Services.IndexerService
{
    public interface IIndexerService : ISearchService, System.IDisposable
    {
        void AddHtmlDocument(PageData pageData);

        void DeleteDocuments(System.Guid[] ids);

        bool OpenWriter();

        void OptimizeIndex();

        void CleanLock();

        bool StartIndexer();
    }
}
