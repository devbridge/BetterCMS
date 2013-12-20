using System;

using BetterCMS.Module.LuceneSearch.Services.IndexerService;
using BetterCMS.Module.LuceneSearch.Services.WebCrawlerService;

using HtmlAgilityPack;

using Moq;

using NUnit.Framework;

namespace BetterCms.Test.Module.LuceneSearch.ServiceTests
{
    [TestFixture]
    public class IndexerServiceTests : TestBase
    {
        [Test]
        public void Should_Return_Correct_Search_Results()
        {
            var document1 = new HtmlDocument();
            document1.DocumentNode.AppendChild(HtmlNode.CreateNode("<title>Test title</title>"));
            document1.DocumentNode.AppendChild(HtmlNode.CreateNode("<body><p>Bodywith search phrase test</p></body>"));
            
            var document2 = new HtmlDocument();
            document2.DocumentNode.AppendChild(HtmlNode.CreateNode("<title>Test title</title>"));
            document2.DocumentNode.AppendChild(HtmlNode.CreateNode("<body><p>Body without search phrase</p></body>"));

            var page1 = new PageData { AbsolutePath = "/test1", Content = document1, Id = Guid.NewGuid() };
            var page2 = new PageData { AbsolutePath = "/test2", Content = document2, Id = Guid.NewGuid() };

            var service = new DefaultIndexerService();
            
            service.Open();
            service.AddHtmlDocument(page1);
            service.AddHtmlDocument(page2);
            service.Close();

            var results = service.Search("test");

            Assert.IsTrue(results.Count == 1);
            Assert.IsTrue(results[0] == page1.AbsolutePath);
        }
    }
}
