using System;
using System.Collections.Generic;

using BetterCMS.Module.LuceneSearch.Models;

namespace BetterCMS.Module.LuceneSearch.Services.ScrapeService
{
    public interface IScrapeService
    {
        Queue<CrawlLink> GetLinksForProcessing(int? limit = null);

        void MarkStarted(Guid id);

        void MarkVisited(Guid id);

        void MarkFailed(Guid id);

        void FetchNewUrls();

        void Delete(Guid id);
    }
}
