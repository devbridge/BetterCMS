using System.Collections.Generic;
using System.Text.RegularExpressions;

using BetterCMS.Module.LuceneSearch.Services.WebCrawlerService;

using HtmlAgilityPack;

using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Store;
using Lucene.Net.Util;

namespace BetterCMS.Module.LuceneSearch.Services.IndexerService
{
    public class DefaultIndexerService : IIndexerService
    {
        private readonly IndexWriter Writer;

        private readonly IndexReader Reader;

        private readonly StandardAnalyzer Analyzer;

        private readonly QueryParser Parser;

        private readonly Directory Index;

        private string directory = "C:\\LuceneWorkFolder5";

        private readonly int ResultCount = 10;

        public DefaultIndexerService()
        {
            Index = FSDirectory.Open(directory);

            Analyzer = new StandardAnalyzer(Version.LUCENE_30);
            Writer = new IndexWriter(Index, Analyzer, true, IndexWriter.MaxFieldLength.LIMITED);
            Parser = new QueryParser(Version.LUCENE_30, "title", Analyzer);
            Reader = IndexReader.Open(Index, true);    
        }

        public void AddHtmlDocument(PageData pageData)
        {
            var doc = new Document();
            
            doc.Add(new Field("path", pageData.AbsolutePath, Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.Add(new Field("title", GetTitle(pageData.Content), Field.Store.YES, Field.Index.ANALYZED));
            doc.Add(new Field("content", GetBody(pageData.Content), Field.Store.YES, Field.Index.ANALYZED));

            Writer.AddDocument(doc);
        }

        public IList<string> Search(string searchString)
        {
            var result = new List<string>();
            var searcher = new IndexSearcher(Reader);
            TopScoreDocCollector collector = TopScoreDocCollector.Create(ResultCount, true);
            var query = Parser.Parse(searchString);
            
            searcher.Search(query, collector);
            ScoreDoc[] hits = collector.TopDocs().ScoreDocs;
            
            for (int i = 0; i < hits.Length; i++)
            {
                int docId = hits[i].Doc;
                Document d = searcher.Doc(docId);
                result.Add(d.Get("path"));
            }
            
            return result;
        }

        private static string GetTitle(HtmlDocument html)
        {
            var titleHtml = string.Empty;
            var title = html.DocumentNode.SelectSingleNode("//title");

            if (title != null)
            {
                titleHtml = title.InnerHtml;
            }

            return HtmlEntity.DeEntitize(titleHtml);
        }

        private static string GetBody(HtmlDocument html)
        {
            var contentHtml = string.Empty;
            var content = html.DocumentNode.SelectSingleNode("//body");

            if (content != null)
            {
                contentHtml = content.InnerHtml;
            }

            return RemoveDuplicateWhitespace(HtmlEntity.DeEntitize(contentHtml));
        }

        private static string RemoveDuplicateWhitespace(string html)
        {
            return Regex.Replace(html, "\\s+", " ");
        }

        public void Commit()
        {
            Writer.Optimize();
            Writer.Commit();
        }
    }
}
