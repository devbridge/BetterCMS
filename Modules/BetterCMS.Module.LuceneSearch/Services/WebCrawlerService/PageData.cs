using System;
using System.Net;

using HtmlAgilityPack;

namespace BetterCMS.Module.LuceneSearch.Services.WebCrawlerService
{
    public class PageData
    {
        public Guid Id { get; set; }

        public string AbsolutePath { get; set; }

        public string AbsoluteUri { get; set; }

        public bool IsPublished { get; set; }

        public HtmlDocument Content { get; set; }

        public HttpStatusCode StatusCode { get; set; }
    }
}
