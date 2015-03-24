using System;
using System.IO;
using System.Linq;
using System.Reflection;

using Autofac;

using BetterCms.Core.Security;
using BetterCms.Core.Services;

using BetterCms.Events;

using BetterCMS.Module.LuceneSearch.Helpers;
using BetterCMS.Module.LuceneSearch.Services.IndexerService;
using BetterCMS.Module.LuceneSearch.Services.WebCrawlerService;
using BetterCms.Module.Search.Models;

using BetterModules.Core.DataAccess;
using BetterModules.Events;

using HtmlAgilityPack;

using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Search;

using NUnit.Framework;

using ServiceStack.Common.Extensions;

namespace BetterCms.Test.Module.LuceneSearch.ServiceTests
{
    [TestFixture]
    public class IndexerServiceTests : TestBase
    {
        const string FullShortText = "Any section of UI that should update dynamically with Knockout can be handled more simply and in a maintainable fashion.";

        const string FullText = "Knockout is a fast, extensible and simple JavaScript library designed to work with HTML document elements "
                + "using a clean underlying view model. It helps to create rich and responsive user interfaces. "
                + "Any section of UI that should update dynamically (e.g., changing depending on the user’s actions "
                + "or when an external data source changes) with Knockout can be handled more simply and in a maintainable fashion.";

        private const string MiddleText = "... elements using a clean underlying view model. It helps to create rich and responsive user interfaces. "
            + "Any section of UI that should update dynamically (e.g., changing depending on the user’s actions or when...";

        private const string StartText = "Knockout is a fast, extensible and simple JavaScript library designed to work with HTML document "
            + "elements using a clean underlying view model. It helps to create rich and responsive user interfaces. Any section of UI that...";

        private const string EndText = "... of UI that should update dynamically (e.g., changing depending on the user’s actions "
            + "or when an external data source changes) with Knockout can be handled more simply and in a maintainable fashion.";

        const string FullTextForOneLetterSearch = "Any section of UI that should update dynamically with Knockout can be handled more simply and in[...]"
            + "Any section of UI that should update dynamically with Knockout can be handled more simply and in[...]"
            + "Any section of UI that should update dynamically with Knockout can be handled more simply and in a maintainable fashion."
            + "Any section of UI that should update dynamically with Knockout can be handled more simply and in[...]"
            + "Any section of UI that should update dynamically with Knockout can be handled more simply and in[...]";

        private const string FullTextForOneLetterSearchResult = "... in[...]Any section of UI that should update dynamically with Knockout can be handled "
            + "more simply and in a maintainable fashion.Any section of UI that should update dynamically with Knockout can be handled...";

        private const string AuthorizedDocumentPath = "BetterCms.Test.Module.Contents.Documents.page.authorized.htm";

        private bool authorizedDocumentAdded;

        [Test]
        public void Should_Return_Correct_Search_Results()
        {
            var document1 = new HtmlDocument();
            document1.DocumentNode.AppendChild(HtmlNode.CreateNode("<title>Test title</title>"));
            document1.DocumentNode.AppendChild(HtmlNode.CreateNode("<body><p>Body with search phrase test</p></body>"));
            
            var document2 = new HtmlDocument();
            document2.DocumentNode.AppendChild(HtmlNode.CreateNode("<title>Test title</title>"));
            document2.DocumentNode.AppendChild(HtmlNode.CreateNode("<body><p>Body without search phrase</p></body>"));

            var page1 = new PageData { AbsolutePath = "/test-1", Content = document1, Id = Guid.NewGuid(), IsPublished = true};
            var page2 = new PageData { AbsolutePath = "/test-2", Content = document2, Id = Guid.NewGuid(), IsPublished = true };

            var service = new DefaultIndexerService(Container.Resolve<ICmsConfiguration>(), Container.Resolve<IRepository>(),
                Container.Resolve<ISecurityService>(), Container.Resolve<IAccessControlService>());

            if (service.OpenWriter())
            {
                service.AddHtmlDocument(page1);
                service.AddHtmlDocument(page2);
                service.CloseWriter();
            }

            var results = service.Search(new SearchRequest("test"));

            Assert.IsNotNull(results.Items);
            Assert.AreEqual(results.Items.Count, 1, "Should return one item.");
            Assert.IsTrue(results.Items[0].Link == page1.AbsolutePath);
        }

        [Test]
        public void Should_Return_Correct_Snippet_FromMiddleText()
        {
            var document1 = new HtmlDocument();
            document1.DocumentNode.AppendChild(HtmlNode.CreateNode("<title>Test title</title>"));
            document1.DocumentNode.AppendChild(HtmlNode.CreateNode("<body>" + ReplaceStringWithNumber(FullText, 3) + "</body>"));

            var page1 = new PageData { AbsolutePath = "/test-3", Content = document1, Id = Guid.NewGuid(), IsPublished = true };

            var service = new DefaultIndexerService(Container.Resolve<ICmsConfiguration>(), Container.Resolve<IRepository>(),
                Container.Resolve<ISecurityService>(), Container.Resolve<IAccessControlService>());

            if (service.OpenWriter())
            {
                service.AddHtmlDocument(page1);
                service.CloseWriter();
            }

            var results = service.Search(new SearchRequest("section3"));

            Assert.IsNotNull(results.Items);
            Assert.AreEqual(results.Items.Count, 1, "Should return one item.");
            // Should be found the middle of the string, because the key word is in the middle of long text
            Assert.AreEqual(results.Items[0].Snippet, ReplaceStringWithNumber(MiddleText, 3));
        }
        
        [Test]
        public void Should_Return_Correct_Snippet_FromTextStart()
        {
            var document1 = new HtmlDocument();
            document1.DocumentNode.AppendChild(HtmlNode.CreateNode("<title>Test title</title>"));
            document1.DocumentNode.AppendChild(HtmlNode.CreateNode("<body>" + ReplaceStringWithNumber(FullText, 4) + "</body>"));

            var page1 = new PageData { AbsolutePath = "/test-4", Content = document1, Id = Guid.NewGuid(), IsPublished = true };

            var service = new DefaultIndexerService(Container.Resolve<ICmsConfiguration>(), Container.Resolve<IRepository>(),
                Container.Resolve<ISecurityService>(), Container.Resolve<IAccessControlService>());

            if (service.OpenWriter())
            {
                service.AddHtmlDocument(page1);
                service.CloseWriter();
            }

            var results = service.Search(new SearchRequest("extensible4"));

            Assert.IsNotNull(results.Items);
            Assert.AreEqual(results.Items.Count, 1, "Should return one item.");
            // Should be found the start of the string, because the start word is in the start
            Assert.AreEqual(results.Items[0].Snippet, ReplaceStringWithNumber(StartText, 4));
        }
        
        [Test]
        public void Should_Return_Correct_Snippet_FromTextEnd()
        {
            var document1 = new HtmlDocument();
            document1.DocumentNode.AppendChild(HtmlNode.CreateNode("<title>Test title</title>"));
            document1.DocumentNode.AppendChild(HtmlNode.CreateNode("<body>" + ReplaceStringWithNumber(FullText, 5) + "</body>"));

            var page1 = new PageData { AbsolutePath = "/test-5", Content = document1, Id = Guid.NewGuid(), IsPublished = true };

            var service = new DefaultIndexerService(Container.Resolve<ICmsConfiguration>(), Container.Resolve<IRepository>(),
                Container.Resolve<ISecurityService>(), Container.Resolve<IAccessControlService>());

            if (service.OpenWriter())
            {
                service.AddHtmlDocument(page1);
                service.CloseWriter();
            }

            var results = service.Search(new SearchRequest("maintainable5"));

            Assert.IsNotNull(results.Items);
            Assert.AreEqual(results.Items.Count, 1, "Should return one item.");
            // Should be found the end of the string, because the key word is in the end
            Assert.AreEqual(results.Items[0].Snippet, ReplaceStringWithNumber(EndText, 5));
        }
        
        [Test]
        public void Should_Return_Correct_Snippet_Full()
        {
            var document1 = new HtmlDocument();
            document1.DocumentNode.AppendChild(HtmlNode.CreateNode("<title>Test title</title>"));
            document1.DocumentNode.AppendChild(HtmlNode.CreateNode("<body>" + ReplaceStringWithNumber(FullShortText, 6) + "</body>"));

            var page1 = new PageData { AbsolutePath = "/test-6", Content = document1, Id = Guid.NewGuid(), IsPublished = true };

            var service = new DefaultIndexerService(Container.Resolve<ICmsConfiguration>(), Container.Resolve<IRepository>(),
                Container.Resolve<ISecurityService>(), Container.Resolve<IAccessControlService>());

            if (service.OpenWriter())
            {
                service.AddHtmlDocument(page1);
                service.CloseWriter();
            }

            var results = service.Search(new SearchRequest("dynamically6"));

            Assert.IsNotNull(results.Items);
            Assert.AreEqual(results.Items.Count, 1, "Should return one item.");
            // Should be found whole string, because it's too short for crop
            Assert.AreEqual(results.Items[0].Snippet, ReplaceStringWithNumber(FullShortText, 6));
        }
        
        [Test]
        public void Should_Return_Correct_Snippet_FullWord()
        {
            var document1 = new HtmlDocument();
            document1.DocumentNode.AppendChild(HtmlNode.CreateNode("<title>Test title</title>"));
            document1.DocumentNode.AppendChild(HtmlNode.CreateNode("<body>" + FullTextForOneLetterSearch + "</body>"));

            var page1 = new PageData { AbsolutePath = "/test-7", Content = document1, Id = Guid.NewGuid(), IsPublished = true };

            var service = new DefaultIndexerService(Container.Resolve<ICmsConfiguration>(), Container.Resolve<IRepository>(),
                Container.Resolve<ISecurityService>(), Container.Resolve<IAccessControlService>());

            if (service.OpenWriter())
            {
                service.AddHtmlDocument(page1);
                service.CloseWriter();
            }

            var results = service.Search(new SearchRequest("a"));

            Assert.IsNotNull(results.Items);
            Assert.AreEqual(results.Items.Count, 1, "Should return one item.");
            // Should be found separate word "a" excluding "a" in another words
            Assert.AreEqual(results.Items[0].Snippet, FullTextForOneLetterSearchResult);
        }

        [Test]
        public void Should_Delete_DocumentFromindex()
        {
            var document1 = new HtmlDocument();
            document1.DocumentNode.AppendChild(HtmlNode.CreateNode("<title>Deleted document title</title>"));
            document1.DocumentNode.AppendChild(HtmlNode.CreateNode("<body>text which will be deleted</body>"));

            var page1 = new PageData { AbsolutePath = "/test-delete-1", Content = document1, Id = Guid.NewGuid(), IsPublished = true };
            var page2 = new PageData { AbsolutePath = "/test-delete-2", Content = document1, Id = Guid.NewGuid(), IsPublished = true };
            var page3 = new PageData { AbsolutePath = "/test-delete-3", Content = document1, Id = Guid.NewGuid(), IsPublished = true };

            var service = new DefaultIndexerService(Container.Resolve<ICmsConfiguration>(), Container.Resolve<IRepository>(),
                Container.Resolve<ISecurityService>(), Container.Resolve<IAccessControlService>());

            if (service.OpenWriter())
            {
                service.AddHtmlDocument(page1);
                service.AddHtmlDocument(page2);
                service.AddHtmlDocument(page3);
                service.CloseWriter();
            }

            // Search result should return 3 objects
            var results = service.Search(new SearchRequest("deleted"));

            Assert.IsNotNull(results.Items);
            Assert.AreEqual(results.Items.Count, 3, "Should return 3 items.");

            // Delete 2 objects
            if (service.OpenWriter())
            {
                service.DeleteDocuments(new[] { page1.Id, page2.Id });
                service.CloseWriter();
            }

            // Search result should return 1 object
            results = service.Search(new SearchRequest("deleted"));

            Assert.IsNotNull(results.Items);
            Assert.AreEqual(results.Items.Count, 1, "Should return one item.");
            Assert.AreEqual(results.Items[0].Link, page3.AbsolutePath);
        }

        [Test]
        public void Should_Return_Correctly_SavedHtmlDocument()
        {
            var service = new DefaultIndexerService(Container.Resolve<ICmsConfiguration>(), Container.Resolve<IRepository>(),
                    Container.Resolve<ISecurityService>(), Container.Resolve<IAccessControlService>());

            AddAuthorizedDocumentToIndex(service);

            var results = service.Search(new SearchRequest("\"Test page HTML content\""));

            Assert.IsNotNull(results.Items);
            Assert.AreEqual(results.Items.Count, 1, "Should return one item.");
            // Should be found separate word "a" excluding "a" in another words
            Assert.AreEqual(results.Items[0].Snippet, "authorized-html-example Test page HTML content. \"");
            Assert.AreEqual(results.Items[0].Title, "Title with <> HTML entities");
        }
        
        [Test]
        public void Should_Fire_Attached_Delegate()
        {
            var luceneDelegateFired = false;

            var service = new DefaultIndexerService(Container.Resolve<ICmsConfiguration>(), Container.Resolve<IRepository>(),
                    Container.Resolve<ISecurityService>(), Container.Resolve<IAccessControlService>());

            var delegateBefore = LuceneSearchHelper.Search;
            LuceneSearchHelper.Search = (query, filter, arg3) =>
            {
                luceneDelegateFired = true;

                return TopScoreDocCollector.Create(0, true);
            };
            
            var results = service.Search(new SearchRequest("\"Test page HTML content\""));

            Assert.IsTrue(luceneDelegateFired);
            Assert.IsNotNull(results);
            Assert.IsNotNull(results.Items);
            Assert.IsEmpty(results.Items);

            LuceneSearchHelper.Search = delegateBefore;
        }
        
        [Test]
        public void Should_Fire_OnDocumentSavingEvent_AndReturnCorrectData()
        {
            var document1 = new HtmlDocument();
            document1.DocumentNode.AppendChild(HtmlNode.CreateNode("<title>zzz</title>"));
            document1.DocumentNode.AppendChild(HtmlNode.CreateNode("<body>zzz</body>"));

            var page1 = new PageData { AbsolutePath = "/test-on-document-saving", Content = document1, Id = Guid.NewGuid(), IsPublished = true };
            Document document = null;

            DefaultEventHandler<DocumentSavingEventArgs> onDocumentSaving = args => 
            {
                args.Document.Add(new Field("TestLuceneField", "TestLuceneFieldValue", Field.Store.YES, Field.Index.ANALYZED));
            };
            DefaultEventHandler<SearchResultRetrievingEventArgs> onSearchResultRetrieving = args => 
            {
                Assert.AreEqual(args.Documents.Count(), 1);
                Assert.AreEqual(args.ResultItems.Count(), 1);

                document = args.Documents[0];
            };
            DefaultEventHandler<SearchQueryExecutingEventArgs> onSearchQueryExecuting = args => 
            {
                Assert.AreEqual(args.RequestQuery, "Nonsense with no results");

                args.Query = new TermQuery(new Term("content", "zzz"));
            };
                
            Events.LuceneEvents.Instance.DocumentSaving += onDocumentSaving;
            Events.LuceneEvents.Instance.SearchResultRetrieving += onSearchResultRetrieving;
            Events.LuceneEvents.Instance.SearchQueryExecuting += onSearchQueryExecuting;

            var service = new DefaultIndexerService(Container.Resolve<ICmsConfiguration>(), Container.Resolve<IRepository>(),
                    Container.Resolve<ISecurityService>(), Container.Resolve<IAccessControlService>());

            if (service.OpenWriter())
            {
                service.AddHtmlDocument(page1);
                service.CloseWriter();
            }

            var results = service.Search(new SearchRequest("Nonsense with no results"));

            Assert.IsNotNull(results);
            Assert.IsNotNull(results.Items);

            Assert.IsNotNull(document);
            Assert.AreEqual(document.Get("TestLuceneField"), "TestLuceneFieldValue");

            Events.LuceneEvents.Instance.DocumentSaving -= onDocumentSaving;
            Events.LuceneEvents.Instance.SearchResultRetrieving -= onSearchResultRetrieving;
            Events.LuceneEvents.Instance.SearchQueryExecuting -= onSearchQueryExecuting;
        }

        private void InstanceOnDocumentSaving(DocumentSavingEventArgs args)
        {
            throw new NotImplementedException();
        }

        private void AddAuthorizedDocumentToIndex(DefaultIndexerService service)
        {
            if (!authorizedDocumentAdded)
            {
                string html;
                using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(AuthorizedDocumentPath))
                {
                    using (var reader = new StreamReader(stream))
                    {
                        html = reader.ReadToEnd();
                    }
                }

                var document = new HtmlDocument();
                document.LoadHtml(html);

                var page = new PageData { AbsolutePath = "/test-authorized-document", Content = document, Id = Guid.NewGuid(), IsPublished = true };

                if (service.OpenWriter())
                {
                    service.AddHtmlDocument(page);
                    service.CloseWriter();
                }
                authorizedDocumentAdded = true;
            }
        }

        private string ReplaceStringWithNumber(string text, int suffix)
        {
            return text
                .Replace("section", string.Concat("section", suffix))
                .Replace(" a ", string.Concat(" a", suffix, " "))
                .Replace("maintainable", string.Concat("maintainable", suffix))
                .Replace("dynamically", string.Concat("dynamically", suffix))
                .Replace("extensible", string.Concat("extensible", suffix));
        }
    }
}
