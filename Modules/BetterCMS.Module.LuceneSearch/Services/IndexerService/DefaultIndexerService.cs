using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;

using BetterCMS.Module.LuceneSearch.Content.Resources;
using BetterCMS.Module.LuceneSearch.Services.WebCrawlerService;

using BetterCms;
using BetterCms.Core.DataAccess;
using BetterCms.Core.Security;
using BetterCms.Core.Services;
using BetterCms.Module.Root;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Search;
using BetterCms.Module.Search.Models;

using HtmlAgilityPack;

using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Store;

using Directory = Lucene.Net.Store.Directory;
using Version = Lucene.Net.Util.Version;

namespace BetterCMS.Module.LuceneSearch.Services.IndexerService
{
    public class DefaultIndexerService : IIndexerService
    {
        private IndexWriter writer;

        private IndexReader reader;

        private readonly IRepository repository;

        private readonly ICmsConfiguration cmsConfiguration;

        private readonly ISecurityService securityService;

        private readonly IAccessControlService accessControlService;

        private readonly StandardAnalyzer analyzer;

        private readonly QueryParser parser;

        private readonly Directory index;

        private readonly string directory;

        public DefaultIndexerService(ICmsConfiguration cmsConfiguration, IRepository repository, ISecurityService securityService, IAccessControlService accessControlService)
        {
            this.repository = repository;
            this.cmsConfiguration = cmsConfiguration;
            this.securityService = securityService;
            this.accessControlService = accessControlService;

            directory = GetLuceneDirectory(cmsConfiguration.Search.GetValue(LuceneSearchConstants.ConfigurationKeys.LuceneFileSystemDirectory));
            index = FSDirectory.Open(directory);

            bool disableStopWords;
            if (!bool.TryParse(cmsConfiguration.Search.GetValue(LuceneSearchConstants.ConfigurationKeys.LuceneDisableStopWords), out disableStopWords))
            {
                disableStopWords = false;
            }
            if (disableStopWords)
            {
                analyzer = new StandardAnalyzer(Version.LUCENE_30, new HashSet<string>());
            }
            else
            {
                analyzer = new StandardAnalyzer(Version.LUCENE_30);
            }
            
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
            doc.Add(new Field("isPublished", pageData.IsPublished.ToString(), Field.Store.YES, Field.Index.ANALYZED));

            writer.UpdateDocument(path, doc, analyzer);
        }

        public SearchResults Search(SearchRequest request)
        {
            if (!reader.IsCurrent())
            {
                reader = reader.Reopen();
            }

            var take = request.Take > 0 ? request.Take : SearchModuleConstants.DefaultSearchResultsCount;
            var skip = request.Skip > 0 ? request.Skip : 0;

            var result = new List<SearchResultItem>();
            var searcher = new IndexSearcher(reader);
            TopScoreDocCollector collector = TopScoreDocCollector.Create(take + skip, true);
            var query = parser.Parse(request.Query);

            if (!RetrieveUnpublishedPages())
            {
                // Exclude unpublished pages
                var isPublishedQuery = new TermQuery(new Term("isPublished", "true"));
                Filter isPublishedFilter = new QueryWrapperFilter(isPublishedQuery);

                searcher.Search(query, isPublishedFilter, collector);
            }
            else
            {
                // Search within all the pages
                searcher.Search(query, collector);
            }
            
            ScoreDoc[] hits = collector.TopDocs(skip, take).ScoreDocs;
            
            for (int i = 0; i < hits.Length; i++)
            {
                int docId = hits[i].Doc;
                Document d = searcher.Doc(docId);
                result.Add(new SearchResultItem
                               {
                                   FormattedUrl = d.Get("path"),
                                   Link = d.Get("path"),
                                   Title = d.Get("title"),
                                   Snippet = GetSnippet(d.Get("content"), request.Query)
                               });
            }

            CheckAvailability(result);

            return new SearchResults
                       {
                           Items = result,
                           Query = request.Query,
                           TotalResults = collector.TotalHits
                       };
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

        private static string GetSnippet(string text, string fullSearchString)
        {
            const int beforeStart = 100;
            const int afterEnd = 100;

            if (text.Length <= afterEnd + beforeStart)
            {
                return text;
            }

            var searchString = fullSearchString.Trim().Split(' ')[0].Trim('\'').Trim('"').Trim('*');

            // Find first position of the keyword
            int index;
            var pattern = string.Format("\\b{0}\\b", searchString);
            var match = Regex.Match(text, pattern);
            if (match.Success)
            {
                index = match.Index;
            }
            else
            {
                index = text.IndexOf(searchString, StringComparison.InvariantCulture);
            }
            if (index < 0)
            {
                index = 0;
            }

            var textLength = text.Length;

            // Crop snippet from the whole search text
            var takeFromEnd = (index + afterEnd < textLength) ? 0 : afterEnd - (textLength - index); 
            var startFrom = index - beforeStart - takeFromEnd;
            var addToEnd = 0;
            if (startFrom < 0)
            {
                startFrom = 0;
            }
            if (startFrom < beforeStart)
            {
                addToEnd = beforeStart - startFrom;
            }

            var endWith = index + afterEnd + addToEnd;
            if (endWith > textLength)
            {
                endWith = textLength;
            }

            if (startFrom > 0)
            {
                var spaceIndex = text.LastIndexOf(" ", startFrom, StringComparison.InvariantCulture);
                if (spaceIndex > 0)
                {
                    startFrom = spaceIndex;
                }
            }
            if (endWith < textLength)
            {
                var spaceIndex = text.IndexOf(" ", endWith, StringComparison.InvariantCulture);
                if (spaceIndex > 0)
                {
                    endWith = spaceIndex;
                }
            }

            var snippet = text.Substring(startFrom, endWith - startFrom);

            if (startFrom > 0)
            {
                snippet = string.Concat("...", snippet);
            }
            if (endWith < textLength)
            {
                snippet = string.Concat(snippet, "...");
            }
            
            return HttpUtility.HtmlDecode(snippet);
        }

        private static string GetLuceneDirectory(string directoryRelative)
        {
            if (Path.IsPathRooted(directoryRelative))
            {
                return directoryRelative;
            }
            
            string appDomainPath;
                
            try
            {
                appDomainPath = HttpRuntime.AppDomainAppPath;
            }
            catch
            {
                appDomainPath = null;
            }
            if (string.IsNullOrWhiteSpace(appDomainPath))
            {
                // Fix for tests / console applications
                appDomainPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            }
            if (directoryRelative.StartsWith("~"))
            {
                directoryRelative = directoryRelative.TrimStart('~').TrimStart('/').TrimStart('\\');
            }

            return Path.Combine(appDomainPath, directoryRelative);
        }

        private void CheckAvailability(List<SearchResultItem> results)
        {
            if (results.Count == 0)
            {
                return;
            }

            if (!cmsConfiguration.Security.AccessControlEnabled)
            {
                // If security is not enabled, all content is available
                return;
            }

            // Create query
            var principal = securityService.GetCurrentPrincipal();
            var urls = results.Select(r => r.Link).ToArray();
            var query = repository.AsQueryable<Page>(p => urls.Contains(p.PageUrl));
            var pages = query
                .SelectMany(c => c.AccessRules, (page, accessRule) => new
                    {
                        PageUrl = page.PageUrl,
                        AccessRule = accessRule
                    })
                .ToList();

            foreach (var pageUrl in pages.Select(p => p.PageUrl).Distinct())
            {
                var page = pages.First(p => p.PageUrl == pageUrl);
                IList<IAccessRule> accessRules = pages.Where(p => p.PageUrl == pageUrl).Select(p => p.AccessRule).Cast<IAccessRule>().ToList();

                var level = accessControlService.GetAccessLevel(accessRules, principal);
                if (level < AccessLevel.Read)
                {
                    results.Where(r => r.Link == page.PageUrl).ToList().ForEach(r => r.IsDenied = true);
                }
            }

            results.ForEach(p =>
                    {
                        if (p.IsDenied)
                        {
                            p.Link = string.Empty;
                            p.FormattedUrl = LuceneGlobalization.SearchResults_Secured_LinkTitle;
                            p.Title = LuceneGlobalization.SearchResults_Secured_Title;
                            p.Snippet = LuceneGlobalization.SearchResults_Secured_Snippet;
                        }
                    });
        }

        private bool RetrieveUnpublishedPages()
        {
            // Check if user can manage content
            var allRoles = new List<string>(RootModuleConstants.UserRoles.AllRoles);
            if (!string.IsNullOrEmpty(cmsConfiguration.Security.FullAccessRoles))
            {
                allRoles.Add(cmsConfiguration.Security.FullAccessRoles);
            }

            var retrieveUnpublishedPages = securityService.IsAuthorized(RootModuleConstants.UserRoles.MultipleRoles(allRoles.ToArray()));
            return retrieveUnpublishedPages;
        }
    }
}
