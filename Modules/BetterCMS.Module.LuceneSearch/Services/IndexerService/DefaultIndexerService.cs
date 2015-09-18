using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;

using BetterCms.Events;

using BetterCMS.Module.LuceneSearch.Content.Resources;
using BetterCMS.Module.LuceneSearch.Helpers;
using BetterCMS.Module.LuceneSearch.Services.WebCrawlerService;

using BetterCms;
using BetterModules.Core.DataAccess;
using BetterCms.Core.Exceptions.Mvc;
using BetterCms.Core.Security;
using BetterCms.Core.Services;
using BetterCms.Module.Root;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Search;
using BetterCms.Module.Search.Models;

using Common.Logging;

using HtmlAgilityPack;

using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Store;

using NHibernate.Linq;
using NHibernate.Mapping;

using Directory = Lucene.Net.Store.Directory;
using Version = Lucene.Net.Util.Version;

namespace BetterCMS.Module.LuceneSearch.Services.IndexerService
{
    public class DefaultIndexerService : IIndexerService
    {
        private static class LuceneIndexDocumentKeys
        {
            public const string Path = "path";
            public const string Content = "content";
            public const string IsPublished = "isPublished";
            public const string Title = "title";
            public const string Id = "id";
        }

        private static readonly ILog Log = LogManager.GetLogger("LuceneSearchModule");

        private static string[] defaultExcludedNodeTypes = new[] { "noscript", "script", "button", "style" };
        private static string[] defaultExcludedIds = new[] { "bcms-browser-info", "bcms-sidemenu" };
        private static string[] defautlExcludedClasses = new[] { "bcms-layout-path" };

        private static ICollection<string> configurationExcludedNodeTypes = new List<string>();
        private static ICollection<string> configurationExcludedIds = new List<string>();
        private static ICollection<string> configurationExcludedClasses = new List<string>();
        private static ICollection<string> configurationExcludedPages = new List<string>();

        private IndexWriter writer;

        private IndexReader reader;

        private readonly IRepository repository;

        private readonly ICmsConfiguration cmsConfiguration;

        private readonly ISecurityService securityService;

        private readonly IAccessControlService accessControlService;

        private bool failedToInitialize;

        private bool initialized;
        
        private bool searchForPartOfWords;

        private bool isClosing;

        private StandardAnalyzer analyzer;

        private QueryParser parser;

        private Directory index;

        private string directory;

        public DefaultIndexerService(ICmsConfiguration cmsConfiguration, IRepository repository, ISecurityService securityService, IAccessControlService accessControlService)
        {
            this.repository = repository;
            this.cmsConfiguration = cmsConfiguration;
            this.securityService = securityService;
            this.accessControlService = accessControlService;
        }

        private bool Initialize()
        {
            if (initialized)
            {
                return true;
            }

            if (failedToInitialize)
            {
                return false;
            }

            var fileSystemPath = cmsConfiguration.Search.GetValue(LuceneSearchConstants.ConfigurationKeys.LuceneFileSystemDirectory);
            if (string.IsNullOrWhiteSpace(fileSystemPath))
            {
                Log.Error("Cannot Initialize Lucene indexer. Lucene file system path is not set.");
                failedToInitialize = true;

                return false;
            }
            directory = GetLuceneDirectory(fileSystemPath);
            index = FSDirectory.Open(directory);

            try
            {
                configurationExcludedNodeTypes = GetCollectionFromConfiguration(LuceneSearchConstants.ConfigurationKeys.LuceneExcludedNodes);
                configurationExcludedIds = GetCollectionFromConfiguration(LuceneSearchConstants.ConfigurationKeys.LuceneExcludedIds);
                configurationExcludedClasses = GetCollectionFromConfiguration(LuceneSearchConstants.ConfigurationKeys.LuceneExcludedClasses);
                configurationExcludedPages = GetCollectionFromConfiguration(LuceneSearchConstants.ConfigurationKeys.LuceneExcludedPages).Select(t => t.Split('#', '?').FirstOrDefault()).ToList();

                bool.TryParse(cmsConfiguration.Search.GetValue(LuceneSearchConstants.ConfigurationKeys.LuceneSearchForPartOfWordsPrefix), out searchForPartOfWords);
                
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

                if (searchForPartOfWords)
                {
                    parser = new PartialWordTermQueryParser(Version.LUCENE_30, "content", analyzer);
                }
                else
                {
                    parser = new QueryParser(Version.LUCENE_30, "content", analyzer);
                }
                parser.AllowLeadingWildcard = true;

                if (!IndexReader.IndexExists(index))
                {
                    OpenWriter(true);
                    CloseWriter();
                }
            }
            catch (Exception exc)
            {
                Log.Error("Failed to initialize Lucene search engine.", exc);
                failedToInitialize = true;

                return false;
            }

            initialized = true;
            return true;
        }

        private ICollection<string> GetCollectionFromConfiguration(string configurationKey)
        {
            var configValue = cmsConfiguration.Search.GetValue(configurationKey);
            return configValue != null ? configValue.Split(',',';').Select(val => val.Trim()).ToList() : new List<string>();
        }

        private bool OpenWriter(bool create)
        {
            if (isClosing)
            {
                return false;
            }

            var tryNumber = 0;
            const int maxTryCount = 3;
            while (File.Exists(Path.Combine(directory, IndexWriter.WRITE_LOCK_NAME)))
            {
                Thread.Sleep(1000);

                tryNumber++;
                if (tryNumber >= maxTryCount)
                {
                    Log.Error("Failed to open Lucene index writer. Write lock file is locked.");

                    return false;
                }
            }

            writer = new IndexWriter(index, analyzer, create, IndexWriter.MaxFieldLength.LIMITED);

            return true;
        }

        public bool OpenWriter()
        {
            if (!Initialize())
            {
                return false;
            }

            return OpenWriter(false);
        }

        private bool OpenReader()
        {
            if (!Initialize())
            {
                return false;
            }

            if (reader != null)
            {
                return true;
            }

            try
            {
                reader = IndexReader.Open(index, true);
            }
            catch (Exception ex)
            {
                Log.ErrorFormat("Failed to open Lucene search index reader.", ex);
                return false;
            }

            return true;
        }

        public void AddHtmlDocument(PageData pageData)
        {
            if (!Initialize())
            {
                Log.ErrorFormat("Cannot add document. Lucene search engine is not initialized.");
                return;
            }

            if (configurationExcludedPages.Contains(pageData.AbsolutePath) ||
                configurationExcludedPages.Contains(pageData.AbsoluteUri))
            {
                var term = new Term(LuceneIndexDocumentKeys.Id, pageData.Id.ToString().ToLower());
                writer.DeleteDocuments(term);
                return;
            }

            var doc = new Document();

            var path = new Term(LuceneIndexDocumentKeys.Path, pageData.AbsolutePath);

            var body = GetBody(pageData.Content);
            var title = GetTitle(pageData.Content);
            doc.Add(new Field(LuceneIndexDocumentKeys.Path, pageData.AbsolutePath, Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.Add(new Field(LuceneIndexDocumentKeys.Title, title, Field.Store.YES, Field.Index.ANALYZED));
            doc.Add(new Field(LuceneIndexDocumentKeys.Content, body, Field.Store.YES, Field.Index.ANALYZED));
            doc.Add(new Field(LuceneIndexDocumentKeys.Id, pageData.Id.ToString(), Field.Store.YES, Field.Index.ANALYZED));
            doc.Add(new Field(LuceneIndexDocumentKeys.IsPublished, pageData.IsPublished.ToString(), Field.Store.YES, Field.Index.ANALYZED));

            doc = LuceneEvents.Instance.OnDocumentSaving(doc, pageData).Document;

            writer.UpdateDocument(path, doc, analyzer);
        }

        public void DeleteDocuments(Guid[] ids)
        {
            if (!Initialize())
            {
                Log.ErrorFormat("Cannot delete documents. Lucene search engine is not initialized.");
                return;
            }

            foreach (var id in ids)
            {
                var term = new Term(LuceneIndexDocumentKeys.Id, id.ToString().ToLower());
                writer.DeleteDocuments(term);
            }
        }

        public SearchResults Search(SearchRequest request)
        {
            if (!OpenReader())
            {
                return new SearchResults { Query = request.Query };
            }

            if (!reader.IsCurrent())
            {
                reader = reader.Reopen();
            }

            var take = request.Take > 0 ? request.Take : SearchModuleConstants.DefaultSearchResultsCount;
            var skip = request.Skip > 0 ? request.Skip : 0;

            var result = new List<SearchResultItem>();
            TopScoreDocCollector collector = TopScoreDocCollector.Create(take + skip, true);
            using (var searcher = new IndexSearcher(reader))
            {
                var searchQuery = request.Query;
                Query query;
                try
                {
                    query = parser.Parse(searchQuery);
                }
                catch (ParseException)
                {
                    try
                    {
                        searchQuery = QueryParser.Escape(searchQuery);
                        query = parser.Parse(searchQuery);
                    }
                    catch (ParseException exc)
                    {
                        throw new ValidationException(() => exc.Message, exc.Message, exc);
                    }
                }

                Filter isPublishedFilter = null;
                if (!RetrieveUnpublishedPages())
                {
                    var isPublishedQuery = new TermQuery(new Term(LuceneIndexDocumentKeys.IsPublished, "true"));
                    isPublishedFilter = new QueryWrapperFilter(isPublishedQuery);
                }

                if (LuceneSearchHelper.Search != null)
                {
                    collector = LuceneSearchHelper.Search(query, isPublishedFilter, collector);
                }
                else
                {
                    query = LuceneEvents.Instance.OnSearchQueryExecuting(query, searchQuery).Query;

                    if (isPublishedFilter != null)
                    {
                        // Exclude unpublished pages
                        searcher.Search(query, isPublishedFilter, collector);
                    }
                    else
                    {
                        // Search within all the pages
                        searcher.Search(query, collector);
                    }
                }

                ScoreDoc[] hits = collector.TopDocs(skip, take).ScoreDocs;
                List<Document> hitDocuments = new List<Document>();
                for (int i = 0; i < hits.Length; i++)
                {
                    int docId = hits[i].Doc;
                    Document d = searcher.Doc(docId);
                    hitDocuments.Add(d);
                    result.Add(
                        new SearchResultItem
                        {
                            FormattedUrl = d.Get(LuceneIndexDocumentKeys.Path),
                            Link = d.Get(LuceneIndexDocumentKeys.Path),
                            Title = d.Get(LuceneIndexDocumentKeys.Title),
                            Snippet = GetSnippet(d.Get(LuceneIndexDocumentKeys.Content), request.Query)
                        });
                }

                CheckAvailability(result);

                LuceneEvents.Instance.OnSearchResultRetrieving(hitDocuments, result);
            }

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
                if (!string.IsNullOrWhiteSpace(titleText))
                {
                    titleText = HttpUtility.HtmlDecode(titleText);
                }
            }

            return titleText;
        }

        private static void CollectChildrenNodesInnerHtml(HtmlNodeCollection nodesCollection, StringBuilder contentText)
        {
            foreach (var childNode in nodesCollection)
            {
                if (CanIncludeNodeToResults(childNode))
                {
                    if (childNode.ChildNodes != null && childNode.ChildNodes.Count > 0)
                    {
                        CollectChildrenNodesInnerHtml(childNode.ChildNodes, contentText);
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(childNode.InnerText))
                        {
                            contentText.Append(childNode.InnerText);
                        }
                    }
                }
            }
        }

        private static string GetBody(HtmlDocument html)
        {
            var contentText = new StringBuilder();
            var content = html.DocumentNode.SelectSingleNode("//body");

            if (content != null)
            {
                CollectChildrenNodesInnerHtml(content.ChildNodes, contentText);
            }

            var body = contentText.ToString();
            body = HttpUtility.HtmlDecode(body);
            return RemoveDuplicateWhitespace(body);
        }

        private static string RemoveDuplicateWhitespace(string html)
        {
            return Regex.Replace(html, "\\s+", " ").Trim();
        }

        public void CloseWriter()
        {
            try
            {
                if (writer != null)
                {
                    writer.Dispose(true);
                    writer = null;
                }
            }
            catch (Exception ex)
            {
                Log.ErrorFormat("Failed to close Lucene search index writer.", ex);
            }
        }

        public void OptimizeIndex()
        {
            if (writer != null)
            {
                writer.Optimize();
            }
        }

        public void CleanLock()
        {
            bool deleteLockFileOnStart;
            if (bool.TryParse(cmsConfiguration.Search.GetValue(LuceneSearchConstants.ConfigurationKeys.LuceneIndexerDeleteLockFileOnStart), out deleteLockFileOnStart) && deleteLockFileOnStart)
            {
                try
                {
                    var fileSystemPath = cmsConfiguration.Search.GetValue(LuceneSearchConstants.ConfigurationKeys.LuceneFileSystemDirectory);
                    if (string.IsNullOrWhiteSpace(fileSystemPath))
                    {
                        Log.Info("Lucene file system path is not set.");
                        return;
                    }

                    directory = GetLuceneDirectory(fileSystemPath);
                    var path = Path.Combine(directory, IndexWriter.WRITE_LOCK_NAME);
                    if (File.Exists(path))
                    {
                        File.Delete(path);
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(string.Format("Failed to delete write lock file '{0}' in directory '{1}'.", IndexWriter.WRITE_LOCK_NAME, directory), ex);
                }
            }
        }

        public bool StartIndexer()
        {
            var runOnHost = cmsConfiguration.Search.GetValue(LuceneSearchConstants.ConfigurationKeys.LuceneIndexerRunsOnlyOnHost);
            
            if (string.IsNullOrWhiteSpace(runOnHost))
            {
                return true;
            }

            var hostName = "";
            try
            {
                hostName = Environment.MachineName;
            }
            catch (Exception ex)
            {
                Log.ErrorFormat("Failed to check host name. Indexer will not start.", ex);
                return false;
            }

            var startIndexer = String.Equals(runOnHost.Trim(), hostName.Trim(), StringComparison.InvariantCultureIgnoreCase);
            if (!startIndexer)
            {
                Log.WarnFormat("Indexer on host '{0}' will not start, because host name does not match provided in configuration.", hostName);
            }

            return startIndexer;
        }

        private void CloseReader()
        {
            try
            {
                if (reader != null)
                {
                    reader.Dispose();
                    reader = null;
                }
            }
            catch (Exception ex)
            {
                Log.ErrorFormat("Failed to close Lucene search index reader.", ex);
            }
        }

        private static bool CanIncludeNodeToResults(HtmlNode node)
        {
            if (node.NodeType == HtmlNodeType.Comment)
            {
                return false;
            }
            if (defaultExcludedNodeTypes.Contains(node.Name) || configurationExcludedNodeTypes.Contains(node.Name))
            {
                return false;
            }

            if (node.Attributes.Any(a => (a.Name == "id" && (defaultExcludedIds.Contains(a.Value) || configurationExcludedIds.Contains(a.Value)))
                || (a.Name == "class" && (ContainsExcludedClasses(defautlExcludedClasses, a.Value) || ContainsExcludedClasses(configurationExcludedClasses, a.Value)))))
            {
                return false;
            }

            return true;
        }

        private static bool ContainsExcludedClasses(ICollection<string> excludedClasses, string classField)
        {
            var cssClases = classField.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var cssClass in cssClases)
            {
                if (excludedClasses.Contains(cssClass))
                {
                    return true;
                }
            }
            return false;
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
            var match = Regex.Match(text, pattern, RegexOptions.IgnoreCase);
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

        public void Dispose()
        {
            isClosing = true;

            CloseWriter();
            CloseReader();

            try
            {
                if (index != null)
                {
                    index.Dispose();
                }
            }
            catch (Exception ex)
            {
                Log.ErrorFormat("Failed to dispose Lucene search index.", ex);
            }

            try
            {
                if (analyzer != null)
                {
                    analyzer.Close();
                    analyzer.Dispose();
                }
            }
            catch (Exception ex)
            {
                Log.ErrorFormat("Failed to close/dispose Lucene search analyzer.", ex);
            }
        }
    }
}
