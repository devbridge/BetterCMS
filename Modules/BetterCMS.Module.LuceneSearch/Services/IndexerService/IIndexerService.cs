using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using BetterCMS.Module.LuceneSearch.Services.WebCrawlerService;

using HtmlAgilityPack;

namespace BetterCMS.Module.LuceneSearch.Services.IndexerService
{
    public interface IIndexerService
    {
        void AddHtmlDocument(PageData pageData);

        IList<string> Search(string searchString);

        void Commit();
    }
}
