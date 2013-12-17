using System;
using System.Collections.Generic;

using BetterCMS.Module.LuceneSearch.Models;

namespace BetterCMS.Module.LuceneSearch.Services.ScrapeService
{
    public interface IScrapeService
    {
        void AddUniqueLink(IList<string> urls);

        Queue<CrawlLink> GetUnprocessedLinks(int limit = 1000);

        Queue<CrawlLink> GetProcessedLinks(int limit = 1000);

        Queue<CrawlLink> GetFailedLinks(int limit = 1000); 

        void MarkVisited(Guid id);
    }
}
