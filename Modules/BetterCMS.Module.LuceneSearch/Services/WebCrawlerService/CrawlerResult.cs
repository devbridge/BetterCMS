using System;
using System.Collections.Generic;

using HtmlAgilityPack;

namespace BetterCMS.Module.LuceneSearch.Services.WebCrawlerService
{
    public class CrawlerResult
    {
        public IEnumerable<string> NewUrls { get; set; }
        public string CurrentUrl { get; set; }
        public HtmlDocument Content { get; set; }
        public bool Succes { get; set; }
        public Guid Id { get; set; }
    }
}
