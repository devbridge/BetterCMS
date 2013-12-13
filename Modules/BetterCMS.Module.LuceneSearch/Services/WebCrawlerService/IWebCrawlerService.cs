using System;
using System.Collections.Generic;

using BetterCMS.Module.LuceneSearch.Services.WebCrawlerService;

namespace BetterCMS.Module.LuceneSearch.Services
{
    public interface IWebCrawleService
    {
        void Start();

        void Stop();

        int GetPageCount();

        CrawlerResult ProccessUrl(string url, Guid id);

        IList<string> GetRootNodes();
    }
}