using System;

using BetterCMS.Module.LuceneSearch.Services.WebCrawlerService;

using Lucene.Net.Documents;

// ReSharper disable CheckNamespace
namespace BetterCms.Events
// ReSharper restore CheckNamespace
{
    public class DocumentSavingEventArgs: EventArgs
    {
        public Document Document { get; set; }

        public PageData PageData { get; set; }

        public DocumentSavingEventArgs(Document document, PageData pageData)
        {
            Document = document;
            PageData = pageData;
        }
    }
}
