using System;
using System.Linq;

using BetterCms.Module.Root.Models;

using NHibernate.Linq;

using NUnit.Framework;

namespace BetterCms.Test.Module.Root.ModelTests.MapTests
{
    [TestFixture]
    public class ContentMapTest : IntegrationTestBase
    {
        [Test]
        public void Should_Insert_And_Retrieve_Content_Successfully()
        {
            var content = TestDataProvider.CreateNewContent();
            RunEntityMapTestsInTransaction(content);            
        }

        [Test]
        public void Should_Insert_And_Retrieve_Content_PageContents_Successfully()
        {
            var content = TestDataProvider.CreateNewContent();
            var pageContents = new[]
                {
                    TestDataProvider.CreateNewPageContent(content),
                    TestDataProvider.CreateNewPageContent(content)
                };
            content.PageContents = pageContents;

            SaveEntityAndRunAssertionsInTransaction(
                content,
                result =>
                    {
                        Assert.AreEqual(content, result);
                        Assert.AreEqual(pageContents.OrderBy(f => f.Id), result.PageContents.OrderBy(f => f.Id));
                    });
        }

        [Test]
        public void Should_Insert_And_Retrieve_Content_ContentOptions_Successfully()
        {
            var content = TestDataProvider.CreateNewContent();
            var contentOptions = new[]
                {
                    TestDataProvider.CreateNewContentOption(content),
                    TestDataProvider.CreateNewContentOption(content)
                };
            content.ContentOptions = contentOptions;

            SaveEntityAndRunAssertionsInTransaction(
                content,
                result =>
                    {
                        Assert.AreEqual(content, result);
                        Assert.AreEqual(contentOptions.OrderBy(f => f.Id), result.ContentOptions.OrderBy(f => f.Id));
                    });
        }

        [Test]
        public void Should_Remove_ContentOptions_From_Content()
        {
            var content = TestDataProvider.CreateNewContent();
            var contentOptions = new[]
                {
                    TestDataProvider.CreateNewContentOption(content),
                    TestDataProvider.CreateNewContentOption(content)
                };

            content.ContentOptions = contentOptions;

            RunActionInTransaction(
                session =>
                {
                    session.SaveOrUpdate(content);
                    session.Flush();
                    Guid contentId = content.Id;
                    session.Clear();

                    session.Delete(content.ContentOptions[0]);
                    session.Flush();
                    session.Clear();

                    var dbContent = session.Query<Content>().FetchMany(f => f.ContentOptions).FirstOrDefault(f => f.Id == contentId);
                    Assert.IsNotNull(dbContent);
                    Assert.AreEqual(content, dbContent);
                    Assert.AreEqual(1, dbContent.ContentOptions.Count);
                });
        }
    }
}
