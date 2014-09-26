using System;
using System.Collections.Generic;

using BetterCms.Module.Search.Models;

using Lucene.Net.Documents;

// ReSharper disable CheckNamespace
namespace BetterCms.Events
// ReSharper restore CheckNamespace
{
    public class SearchResultRetrievingEventArgs: EventArgs
    {
        public List<Document> Documents { get; set; }

        public List<SearchResultItem> ResultItems { get; set; }

        public SearchResultRetrievingEventArgs(List<Document> documents, List<SearchResultItem> resultItems)
        {
            Documents = documents;
            ResultItems = resultItems;
        }
    }
}
