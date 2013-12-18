using HtmlAgilityPack;

namespace BetterCMS.Module.LuceneSearch.Services.WebCrawlerService
{
    public class PageData
    {
        public string AbsolutePath { get; set; }

        public string AbsoluteUri { get; set; }

        public HtmlDocument Content { get; set; }
    }
}
