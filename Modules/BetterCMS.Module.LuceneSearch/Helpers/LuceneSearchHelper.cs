using System;

using Lucene.Net.Search;

namespace BetterCMS.Module.LuceneSearch.Helpers
{
    public class LuceneSearchHelper
    {
        public static Func<Query, Filter, TopScoreDocCollector, TopScoreDocCollector> Search { get; set; }
    }
}
