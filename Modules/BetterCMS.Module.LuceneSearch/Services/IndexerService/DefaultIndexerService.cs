using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;

using BetterCMS.Module.LuceneSearch.Services.WebCrawlerService;

using BetterCms;
using BetterCms.Module.Search;
using BetterCms.Module.Search.Models;

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
        private IndexWriter writer;

        private IndexReader reader;

        private readonly StandardAnalyzer analyzer;

        private readonly QueryParser parser;

        private readonly Directory index;

        private readonly string directory;

        public DefaultIndexerService(ICmsConfiguration configuration)
        {
            directory = configuration.Search.GetValue(LuceneSearchConstants.LuceneSearchFileSystemDirectoryConfigurationKey);

            index = FSDirectory.Open(directory);

            analyzer = new StandardAnalyzer(Version.LUCENE_30);
            parser = new QueryParser(Version.LUCENE_30, "content", analyzer);
            
            if (!IndexReader.IndexExists(index))
            {
                Open(true);
                Close();
            }

            reader = IndexReader.Open(index, true);
        }

        public void Open(bool create = false)
        {
            while (File.Exists(IndexWriter.WRITE_LOCK_NAME))
            {
                Thread.Sleep(1000);
            }

            writer = new IndexWriter(index, analyzer, create, IndexWriter.MaxFieldLength.LIMITED);
        }

        public void AddHtmlDocument(PageData pageData)
        {
            var doc = new Document();

            var path = new Term("path", pageData.AbsolutePath);

            doc.Add(new Field("path", pageData.AbsolutePath, Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.Add(new Field("title", GetTitle(pageData.Content), Field.Store.YES, Field.Index.ANALYZED));
            doc.Add(new Field("content", GetBody(pageData.Content), Field.Store.YES, Field.Index.ANALYZED));

            writer.UpdateDocument(path, doc, analyzer);
        }

        public IList<SearchResultItem> Search(string searchString, int resultCount = SearchModuleConstants.DefaultSearchResultsCount)
        {
            if (!reader.IsCurrent())
            {
                reader = reader.Reopen();
            }

            var result = new List<SearchResultItem>();
            var searcher = new IndexSearcher(reader);
            TopScoreDocCollector collector = TopScoreDocCollector.Create(resultCount, true);
            var query = parser.Parse(searchString);
            
            searcher.Search(query, collector);
            ScoreDoc[] hits = collector.TopDocs().ScoreDocs;
            
            for (int i = 0; i < hits.Length; i++)
            {
                int docId = hits[i].Doc;
                Document d = searcher.Doc(docId);
                result.Add(new SearchResultItem
                               {
                                   FormattedUrl = d.Get("path"),
                                   Link = d.Get("path"),
                                   Title = d.Get("title"),
                                   Snippet = d.Get("content").Substring(0, 200) + "..."
                               });
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
            writer.Optimize();
            writer.Dispose();
        }
    }
}
