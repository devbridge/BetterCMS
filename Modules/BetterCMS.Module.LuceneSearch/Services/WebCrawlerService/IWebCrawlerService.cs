using System;
using System.Collections.Generic;

using BetterCMS.Module.LuceneSearch.Services.WebCrawlerService;

namespace BetterCMS.Module.LuceneSearch.Services
{
    public interface IWebCrawlerService
    {
        CrawlerResult ProccessUrl(string url, Guid id);

        IList<string> GetRootNodes();

        IList<string> GetPagesList();

        PageData FetchPage(string url);
    }
}