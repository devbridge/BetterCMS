using System;
using System.Collections.Generic;

using BetterCMS.Module.LuceneSearch.Services.IndexerService;
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

        public bool ExcludeDefaultDocumentFromIndex { get; set; }

        public IList<DocumentData> AdditionalDocuments { get; set; }

        public DocumentSavingEventArgs(Document document, PageData pageData)
        {
            Document = document;
            PageData = pageData;
            ExcludeDefaultDocumentFromIndex = false;
            AdditionalDocuments = new List<DocumentData>();
        }
    }
}
