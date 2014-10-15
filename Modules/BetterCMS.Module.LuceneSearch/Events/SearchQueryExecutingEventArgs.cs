using System;

using Lucene.Net.Search;

// ReSharper disable CheckNamespace
namespace BetterCms.Events
// ReSharper restore CheckNamespace
{
    public class SearchQueryExecutingEventArgs: EventArgs
    {
        public Query Query { get; set; }

        public string RequestQuery { get; set; }

        public SearchQueryExecutingEventArgs(Query query, string requestQuery)
        {
            Query = query;
            RequestQuery = requestQuery;
        }
    }
}
