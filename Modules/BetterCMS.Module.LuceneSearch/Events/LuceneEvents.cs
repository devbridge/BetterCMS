using System.Collections.Generic;

using BetterCMS.Module.LuceneSearch.Services.WebCrawlerService;
using BetterCms.Module.Search.Models;

using BetterModules.Events;

using Lucene.Net.Documents;
using Lucene.Net.Search;

// ReSharper disable CheckNamespace
namespace BetterCms.Events
// ReSharper restore CheckNamespace
{
    public class LuceneEvents : EventsBase<LuceneEvents>
    {
        /// <summary>
        /// Occurs before document is saved.
        /// </summary>
        public event DefaultEventHandler<DocumentSavingEventArgs> DocumentSaving;

        /// <summary>
        /// Occurs before document is saved.
        /// </summary>
        public event DefaultEventHandler<SearchQueryExecutingEventArgs> SearchQueryExecuting;

        /// <summary>
        /// Occurs before document is saved.
        /// </summary>
        public event DefaultEventHandler<SearchResultRetrievingEventArgs> SearchResultRetrieving;

        public DocumentSavingEventArgs OnDocumentSaving(Document document, PageData pageData)
        {
            var args = new DocumentSavingEventArgs(document, pageData);

            if (DocumentSaving != null)
            {
                DocumentSaving(args);
            }

            return args;
        }

        public SearchQueryExecutingEventArgs OnSearchQueryExecuting(Query query, string requestQuery)
        {
            var args = new SearchQueryExecutingEventArgs(query, requestQuery);

            if (SearchQueryExecuting != null)
            {
                SearchQueryExecuting(args);
            }

            return args;
        }

        public SearchResultRetrievingEventArgs OnSearchResultRetrieving(List<Document> documents, List<SearchResultItem> resultItems)
        {
            var args = new SearchResultRetrievingEventArgs(documents, resultItems);

            if (SearchResultRetrieving != null)
            {
                SearchResultRetrieving(args);
            }

            return args;
        }
    }
}
