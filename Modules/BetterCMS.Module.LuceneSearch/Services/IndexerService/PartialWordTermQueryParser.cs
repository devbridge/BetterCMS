using Lucene.Net.Analysis;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Util;

namespace BetterCMS.Module.LuceneSearch.Services.IndexerService
{
    public class PartialWordTermQueryParser : QueryParser
    {
        public PartialWordTermQueryParser(Version matchVersion, string f, Analyzer a)
            : base(matchVersion, f, a)
        {
            AllowLeadingWildcard = true;
        }

        protected override Query NewTermQuery(Term term)
        {
            var query = CreateQuery(term);
            if (query != null)
            {
                return query;
            }

            return base.NewTermQuery(term);
        }

        private Query CreateQuery(Term term)
        {
            var text = FixQueryWord(term.Text);
            if (text != term.Text)
            {
                return new WildcardQuery(new Term(term.Field, text));
            }

            return null;
        }

        private string FixQueryWord(string queryText)
        {
            if (!queryText.StartsWith("*"))
            {
                queryText = string.Concat("*", queryText);
            }
            if (!queryText.EndsWith("*"))
            {
                queryText = string.Concat(queryText, "*");
            }

            return queryText;
        }
    }
}
