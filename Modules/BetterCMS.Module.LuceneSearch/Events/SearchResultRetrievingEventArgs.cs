using System;
using System.Collections.Generic;

using BetterCms.Module.Search.Models;

using Lucene.Net.Search;

// ReSharper disable CheckNamespace
namespace BetterCms.Events
// ReSharper restore CheckNamespace
{
    public class SearchResultRetrievingEventArgs: EventArgs
    {
        public List<ScoreDoc> Documents { get; set; }

        public List<SearchResultItem> ResultItems { get; set; }

        public SearchResultRetrievingEventArgs(List<ScoreDoc> documents, List<SearchResultItem> resultItems)
        {
            Documents = documents;
            ResultItems = resultItems;
        }
    }
}
