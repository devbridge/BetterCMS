using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;

using BetterCMS.Module.LuceneSearch.Services.WebCrawlerService;

using HtmlAgilityPack;

using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Store;
using Lucene.Net.Util;

using Directory = Lucene.Net.Store.Directory;

namespace BetterCMS.Module.LuceneSearch.Services.IndexerService
{
    public class DefaultIndexerService : IIndexerService
    {
        private IndexWriter Writer;

        private IndexReader Reader;

        private readonly StandardAnalyzer Analyzer;

        private readonly QueryParser Parser;

        private readonly Directory Index;

        //private string directory = "C:\\LuceneWorkFolder5";

        private string directory = "C:\\Lucene\\Test";

        private readonly int ResultCount = 10;

        public DefaultIndexerService()
        {
            Index = FSDirectory.Open(directory);

            Analyzer = new StandardAnalyzer(Version.LUCENE_30);
            Parser = new QueryParser(Version.LUCENE_30, "content", Analyzer);
            
            if (!IndexReader.IndexExists(Index))
            {
                Open(true);
                Close();
            }

            Reader = IndexReader.Open(Index, true);
        }

        public void Open(bool create = false)
        {
            while (File.Exists(IndexWriter.WRITE_LOCK_NAME))
            {
                Thread.Sleep(1000);
            }

            Writer = new IndexWriter(Index, Analyzer, create, IndexWriter.MaxFieldLength.LIMITED);
        }

        public void AddHtmlDocument(PageData pageData)
        {
            var doc = new Document();

            var path = new Term("path", pageData.AbsolutePath);

            doc.Add(new Field("path", pageData.AbsolutePath, Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.Add(new Field("title", GetTitle(pageData.Content), Field.Store.YES, Field.Index.ANALYZED));
            doc.Add(new Field("content", GetBody(pageData.Content), Field.Store.YES, Field.Index.ANALYZED));

            Writer.UpdateDocument(path, doc, Analyzer);
        }

        public IList<string> Search(string searchString)
        {
            if (!Reader.IsCurrent())
            {
                Reader = Reader.Reopen();
            }

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
            var titleText = string.Empty;
            var title = html.DocumentNode.SelectSingleNode("//title");

            if (title != null)
            {
                titleText = title.InnerText;
            }

            return titleText;
        }

        private static string GetBody(HtmlDocument html)
        {
            var contentText = string.Empty;
            var content = html.DocumentNode.SelectSingleNode("//body");

            if (content != null)
            {
                contentText = content.InnerText;
            }

            return RemoveDuplicateWhitespace(contentText);
        }

        private static string RemoveDuplicateWhitespace(string html)
        {
            return Regex.Replace(html, "\\s+", " ");
        }

        public void Close()
        {
            Writer.Optimize();
            Writer.Dispose();
        }
    }
}
